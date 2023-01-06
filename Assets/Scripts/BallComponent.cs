using UnityEngine;

public class BallComponent : InteractiveComponent
{
    private SpringJoint2D m_connectedJoint;
    private Rigidbody2D m_connectedBody;
    private LineRenderer m_lineRenderer;
    private TrailRenderer m_trailRenderer;
    private bool m_hitTheGround = false;

    public float SlingStart = 0.5f;
    public float MaxSpringDistance = 2.5f;

    [Header("Sling attach points offset")]
    public Vector2 FirstSlingArmOffset = new Vector2(0.48f, 0.15f);
    public Vector2 SecondSlingArmOffset = new Vector2(-0.5f, 0f);
    public Vector2 BallSlingOffset = new Vector2(-0.2f, -0.05f);

    public AudioSource m_audioSource;
    [Header("Audio")]
    public AudioClip PullSound;
    public AudioClip ShootSound;
    public AudioClip HitSound;
    public AudioClip RestartSound;

    [Header("Particles")]
    public ParticleSystem ShootParticles;
    public ParticleSystem DragParticles;

    CameraController cameraController;

    private void Start()
    {
        m_startPosition = transform.position;
        m_startRotation = transform.rotation;

        m_connectedJoint = GetComponent<SpringJoint2D>();
        m_connectedBody = m_connectedJoint.connectedBody;
        m_lineRenderer = GetComponent<LineRenderer>();
        m_trailRenderer = GetComponent<TrailRenderer>();
        cameraController = FindObjectOfType<CameraController>();
        m_audioSource = GetComponent<AudioSource>();

        m_lineRenderer.enabled = false;
        m_trailRenderer.enabled = false;
        DragParticles.Stop();

        GameplayManager.OnGamePaused += DoPause;
        GameplayManager.OnGamePlaying += DoPlay;
    }

    private void Update()
    {
        if (transform.position.x > m_connectedBody.transform.position.x + SlingStart)
        {
            m_connectedJoint.enabled = false;
            m_lineRenderer.enabled = false;
            m_trailRenderer.enabled = !m_hitTheGround;
        }

        SetLineRendererPoints();
    }

    public override void DoRestart()
    {
        PlaySound(m_audioSource, RestartSound);
        cameraController.ResetCamPosition();

        base.DoRestart();

        m_connectedJoint.enabled = true;
        m_lineRenderer.enabled = false;
        m_trailRenderer.enabled = false;

        SetLineRendererPoints();
    }

    private void SetLineRendererPoints()
    {
        m_lineRenderer.positionCount = 3;
        Vector2 curBallPos = new Vector2(transform.position.x, transform.position.y);
        var firstSlingArmPosition = m_connectedBody.position + FirstSlingArmOffset;
        var secondSlingArmPosition = m_connectedBody.position + SecondSlingArmOffset;
        var ballSlingPosition = curBallPos + BallSlingOffset;
        m_lineRenderer.SetPositions(new Vector3[] { firstSlingArmPosition, ballSlingPosition, secondSlingArmPosition });
    }

    private void OnMouseDown()
    {
        PlaySound(m_audioSource, PullSound);
        DragParticles.Play();
    }

    private void OnMouseUp()
    {
        DoPlay();
        PlaySound(m_audioSource, ShootSound);
        ShootParticles.Play();
        DragParticles.Stop();
    }

    private void OnMouseDrag()
    {

        m_lineRenderer.enabled = true;
        m_hitTheGround = false;
        DoPause();

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 newBallPos = new Vector3(worldPos.x, worldPos.y);

        float CurJointDistance = Vector3.Distance(newBallPos, m_connectedBody.transform.position);

        if (CurJointDistance > MaxSpringDistance)
        {
            Vector2 direction = (newBallPos - m_connectedBody.position).normalized;
            transform.position = m_connectedBody.position + direction * MaxSpringDistance;
        }
        else
        {
            transform.position = newBallPos;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            PlaySound(m_audioSource, HitSound);
            m_hitTheGround = true;
        }
    }

    public bool IsSimulated()
    {
        return m_rigidbody2d.simulated;
    }

}

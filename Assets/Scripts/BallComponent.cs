using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallComponent : MonoBehaviour
{
    private Vector3 m_startPosition;
    private Quaternion m_startRotation;
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

    private AudioSource m_audioSource;
    [Header("Audio")]
    public AudioClip PullSound;
    public AudioClip ShootSound;
    public AudioClip HitSound;
    public AudioClip RestartSound;
    public AudioClip WoodHit;

    [Header("Particles")]
    public ParticleSystem ShootParticles;
    public ParticleSystem DragParticles;
    public ParticleSystem WoodHitParticles;

    Rigidbody2D m_rigidbody2d;
    CameraController cameraController;

    private void Start()
    {
        m_startPosition = transform.position;
        m_startRotation = transform.rotation;

        m_rigidbody2d = GetComponent<Rigidbody2D>();
        m_connectedJoint = GetComponent<SpringJoint2D>();
        m_connectedBody = m_connectedJoint.connectedBody;
        m_lineRenderer = GetComponent<LineRenderer>();
        m_trailRenderer = GetComponent<TrailRenderer>();
        cameraController = FindObjectOfType<CameraController>();
        m_audioSource = GetComponent<AudioSource>();

        m_lineRenderer.enabled = false;
        m_trailRenderer.enabled = false;
        DragParticles.Stop();
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

        if (Input.GetKeyUp(KeyCode.R))
            Restart();
    }

    private void Restart()
    {
        m_audioSource.PlayOneShot(RestartSound);
        cameraController.ResetCamPosition();
        transform.position = m_startPosition;
        transform.rotation = m_startRotation;

        m_rigidbody2d.velocity = Vector3.zero;
        m_rigidbody2d.angularVelocity = 0.0f;
        m_rigidbody2d.simulated = true;

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
        m_audioSource.PlayOneShot(PullSound);
        DragParticles.Play();
    }

    private void OnMouseUp()
    {
        m_rigidbody2d.simulated = true;
        m_audioSource.PlayOneShot(ShootSound);
        ShootParticles.Play();
        DragParticles.Stop();
    }

    private void OnMouseDrag()
    {
        if (GameplayManager.Instance.Pause)
        {
            return;
        }

        m_hitTheGround = false;
        m_lineRenderer.enabled = true;
        m_rigidbody2d.simulated = false;

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
            m_audioSource.PlayOneShot(HitSound);
            m_hitTheGround = true;
        }

        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Target"))
        {
            m_audioSource.PlayOneShot(WoodHit);
            Instantiate(WoodHitParticles, collision.contacts[0].point, Quaternion.identity);
        }
    }

    public bool IsSimulated()
    {
        return m_rigidbody2d.simulated;
    }
}

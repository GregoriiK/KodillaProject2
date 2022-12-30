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

        m_lineRenderer.enabled = false;
        m_trailRenderer.enabled = false;
    }

    private void Update()
    {
        if (transform.position.x > m_connectedBody.transform.position.x + SlingStart)
        {
            m_connectedJoint.enabled = false;
            m_lineRenderer.enabled = false;
            m_trailRenderer.enabled = !m_hitTheGround;
        }

        RenderSlingLine();

        if (Input.GetKeyUp(KeyCode.R))
            Restart();
    }

    private void Restart()
    {
        cameraController.ResetCamPosition();
        transform.position = m_startPosition;
        transform.rotation = m_startRotation;

        m_rigidbody2d.velocity = Vector3.zero;
        m_rigidbody2d.angularVelocity = 0.0f;
        m_rigidbody2d.simulated = true;

        m_connectedJoint.enabled = true;
        m_lineRenderer.enabled = false;
        m_trailRenderer.enabled = false;

        RenderSlingLine();
    }

    private void RenderSlingLine()
    {
        m_lineRenderer.positionCount = 3;

        var firstSlingArmPosition = m_connectedBody.position + new Vector2(0.48f, 0.15f);
        var secondSlingArmPosition = m_connectedBody.position + new Vector2(-0.5f, 0f);
        var ballSlingPosition = transform.position + new Vector3(-0.2f, -0.05f);
        m_lineRenderer.SetPositions(new Vector3[] { firstSlingArmPosition, ballSlingPosition, secondSlingArmPosition });
    }

    private void OnMouseUp()
    {
        m_rigidbody2d.simulated = true;
    }

    private void OnMouseEnter()
    {
        //Debug.Log("Mouse entering over object");
    }

    private void OnMouseExit()
    {
        //Debug.Log("Mouse leaving object");
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
            m_hitTheGround = true;
        }
    }

    public bool IsSimulated()
    {
        return m_rigidbody2d.simulated;
    }
}

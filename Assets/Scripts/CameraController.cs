using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private BallComponent camTarget;
    private Vector3 originalPosition;
    Rigidbody2D targetRigidbody; 

    private void Start()
    {
        camTarget = FindObjectOfType<BallComponent>();
        originalPosition = transform.position;
        targetRigidbody = camTarget.GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!camTarget.IsSimulated()) return;
        float targetVelocity = targetRigidbody.velocity.magnitude;
        float movementTowardsSpeed = Time.fixedDeltaTime + targetVelocity;
        Vector3 newPosition = camTarget.transform.position + originalPosition; // dla skrócenia linii poni¿ej
        transform.position = Vector3.MoveTowards(transform.position, newPosition , movementTowardsSpeed);
    }
}

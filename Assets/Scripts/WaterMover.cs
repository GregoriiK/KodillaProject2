using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMover : MonoBehaviour
{
    Transform m_transform;

    private void Start()
    {
        m_transform = GetComponent<Transform>();
    }
    void Update()
    {
        float newPos = Camera.main.transform.position.x;
        gameObject.transform.position = new Vector3(newPos,m_transform.position.y, transform.position.z);
        
    }
}

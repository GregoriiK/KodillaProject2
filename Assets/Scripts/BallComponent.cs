using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallComponent : MonoBehaviour
{
    Rigidbody2D m_rigidbody2d;

    private void Start()
    {
        m_rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void OnMouseUp()
    {
        m_rigidbody2d.simulated = true;
    }

    private void OnMouseEnter()
    {
        Debug.Log("Mouse entering over object");
    }

    private void OnMouseExit()
    {
        Debug.Log("Mouse leaving object");
    }

    private void OnMouseDrag()
    {
        if (GameplayManager.Instance.Pause)
        {
            return;
        }
        m_rigidbody2d.simulated = false;

        Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(worldPos.x, worldPos.y, 0);
    }
}

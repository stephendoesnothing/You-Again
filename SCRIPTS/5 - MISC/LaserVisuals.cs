using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserVisuals : MonoBehaviour
{
    [SerializeField] private float defDistanceRay = 100;
    public LineRenderer m_LineRenderer;
    public Transform laserFirePoint;
    Transform m_transform;

    private void Awake()
    {
        m_transform = GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        ShootLaser();
    }

    void ShootLaser()
    {
        int layerMask =
            (1 << LayerMask.NameToLayer("Ground")) |
            (1 << LayerMask.NameToLayer("Platform")) |
            (1 << LayerMask.NameToLayer("Player"));

        RaycastHit2D hit = Physics2D.Raycast(laserFirePoint.position, transform.right, defDistanceRay, layerMask);

        Vector2 endPoint;

        if (hit.collider != null)
        {
            endPoint = hit.point;

            if (hit.collider.CompareTag("Player"))
            {
                if (hit.collider.TryGetComponent(out PlayerHealth playerHealth))
                {
                    playerHealth.TakeDamage(10);
                }
            }
        }
        else
        {
            endPoint = laserFirePoint.position + transform.right * defDistanceRay;
        }

        Draw2DRay(laserFirePoint.position, endPoint);

        // Optional: debug ray
        Debug.DrawLine(laserFirePoint.position, endPoint, Color.red);
    }



    void Draw2DRay(Vector2 startPos, Vector2 endPos)
    {
        m_LineRenderer.SetPosition(0, startPos);
        m_LineRenderer.SetPosition(1, endPos);
    }
}

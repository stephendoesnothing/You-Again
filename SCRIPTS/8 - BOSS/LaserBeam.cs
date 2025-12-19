using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public Transform centerPoint;
    public float rotationSpeed = 30f;
    private float totalRotation = 0f;
    public int damage = 50;

    private float lastDamageTime = -999f;
    public float damageInterval = 0.5f;

    private void Update()
    {
        if (centerPoint == null) return;

        float rotateStep = rotationSpeed * Time.deltaTime;
        totalRotation += rotateStep;

        transform.RotateAround(centerPoint.position, Vector3.forward, rotateStep);

        if (totalRotation >= 360f) Destroy(gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (Time.time - lastDamageTime >= damageInterval)
            {
                if (collision.TryGetComponent(out PlayerHealth playerHealth))
                {
                    playerHealth.TakeDamage(damage);
                    lastDamageTime = Time.time;
                }
            }
        }
    }
}

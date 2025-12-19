using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int damage = 3;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && collision.TryGetComponent(out EnemyHealth health))
        {
            health.TakeDamage(damage);
        }

        if (collision.CompareTag("Enemy") && collision.TryGetComponent(out BossStats stats))
        {
            stats.TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}

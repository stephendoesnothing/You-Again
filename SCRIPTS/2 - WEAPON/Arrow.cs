using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [Header("Arrow Settings")]
    [SerializeField] private float speed = 10f;
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int damage = 3;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Move forward based on arrow's facing direction (right = forward)
        rb.velocity = transform.right * speed;

        // Auto-destroy after lifetime
        Destroy(gameObject, lifeTime);
    }

    private void Update()
    {
        // Rotate arrow to match movement direction
        if (rb.velocity != Vector2.zero)
        {
            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (collision.TryGetComponent(out EnemyHealth health))
            {
                health.TakeDamage(damage);
            }

            if (collision.TryGetComponent(out BossStats stats))
            {
                stats.TakeDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}

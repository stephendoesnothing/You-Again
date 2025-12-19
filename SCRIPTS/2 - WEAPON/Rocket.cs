using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [Header("Rocket Settings")]
    [SerializeField] private float lifeTime = 3f;
    [SerializeField] private int damage = 3;
    [SerializeField] private float speed = 8f;

    [Header("Wiggle")]
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float frequency = 5f;
    private float randomization;

    [Header("Explosion")]
    [SerializeField] private float radius = 2.5f;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] GameObject explosionEffect;

    private Vector2 direction;
    private float timeAlive;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
        direction = transform.right;
        randomization = Random.Range(0f, Mathf.PI * 2f);
    }

    private void Update()
    {
        timeAlive += Time.deltaTime;

        Vector2 perpendicular = new Vector2(-direction.y, direction.x);
        Vector2 offset = perpendicular * Mathf.Sin(timeAlive * frequency + randomization) * amplitude;

        Vector2 moveDir = (direction + offset).normalized;

        transform.position += (Vector3)(moveDir * speed * Time.deltaTime);
        transform.right = moveDir;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Explode();
        }
        if (explosionEffect != null) Instantiate(explosionEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Explode()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);

        foreach(Collider2D hit in hits)
        {
            if(hit.TryGetComponent(out BossStats health))
            {
                health.TakeDamage(damage);
            }
        }  
    }
}

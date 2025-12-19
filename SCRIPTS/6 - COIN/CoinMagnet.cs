using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinMagnet : MonoBehaviour
{
    [Header("Magnet Settings")]
    public float magnetRange = 2f;
    public float rangeUpgrade = 2f;

    private CircleCollider2D magnetCollider;

    public void Awake()
    {
        magnetCollider = GetComponent<CircleCollider2D>();
        if (magnetCollider == null) return;

        magnetCollider.isTrigger = true;
        magnetCollider.radius = magnetRange;
    }

    public void IncreaseRange()
    {
        magnetRange += rangeUpgrade;
        magnetCollider.radius = magnetRange;
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            Vector2 direction = (transform.position - collision.transform.position).normalized;
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();

            if (rb != null) rb.AddForce(direction * 10f);

            // Check if close enough to "collect"
            float distance = Vector2.Distance(transform.position, collision.transform.position);
            if (distance < 0.2f) // You can tweak this threshold
            {
                SoundManager.Instance.PlaySFX("PickCoin");
                Destroy(collision.gameObject);
            }
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, magnetRange);
    }
}

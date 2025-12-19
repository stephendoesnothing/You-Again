using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "BossAttack/MagicBolt")]
public class MagicBolt : BossAttack
{
    [Header("References")]
    public GameObject boltPrefab;

    [Header("Settings")]
    [SerializeField] private float projectileSpeed = 5f;
    [SerializeField] private float spreadAngle = 30f;
    [SerializeField] private int projectileCount = 1;

    public override void ExecuteAttack(Transform boss, Transform firePoint)
    {
        Vector2 dir = (PlayerReference.instance.transform.position - firePoint.position).normalized;

        float baseAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        int shots = Mathf.Max(1, projectileCount);
        float angleStep = (shots > 1) ? spreadAngle / (shots - 1) : 0f;
        float startAngle = baseAngle - (spreadAngle / 2f);

        for(int i = 0; i < shots; i++)
        {
            float angle = startAngle + (angleStep * i);
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject bolt = Instantiate(boltPrefab, firePoint.position, Quaternion.identity);

            Rigidbody2D rb = bolt.GetComponent<Rigidbody2D>();
            if(rb != null)
            {
                rb.velocity = direction * projectileSpeed;
            }
        }       
    }
}

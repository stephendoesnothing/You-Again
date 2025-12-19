using UnityEngine;
using static UnityEngine.Rendering.DebugUI.Table;

[CreateAssetMenu(menuName = ("BossAttack/ArcaneBurst"))]
public class ArcaneBurst : BossAttack
{
    public GameObject projectilePrefab;
    public int projectileCount = 8;
    public float burstSpeed = 4f;

    public override void ExecuteAttack(Transform boss, Transform firePoint)
    {
        for(int i = 0; i < projectileCount; i++)
        {
            float angle = i * (360f / projectileCount);
            Vector2 dir = new Vector2(Mathf.Cos(angle * Mathf.Rad2Deg), Mathf.Sin(angle * Mathf.Deg2Rad));
            GameObject proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            if(proj.GetComponent<Rigidbody2D>() != null)
            {
                proj.GetComponent<Rigidbody2D>().velocity = dir * burstSpeed;
            }
            
        }
    }
}

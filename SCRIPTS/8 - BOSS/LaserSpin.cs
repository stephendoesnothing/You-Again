using UnityEngine;

[CreateAssetMenu(menuName = "BossAttack/LaserSpin")]
public class LaserSpin : BossAttack
{
    public GameObject laserPrefab;
    public float rotationSpeed = 45f;

    public override void ExecuteAttack(Transform boss, Transform firePoint)
    {
        if(laserPrefab == null || boss == null) return;

        GameObject laser = Instantiate(laserPrefab, firePoint.position, firePoint.rotation);
        LaserBeam beam = laser.GetComponent<LaserBeam>();
        if(beam != null )
        {
            beam.centerPoint = boss;
            beam.rotationSpeed = rotationSpeed;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = ("BossAttack/MeteorShower"))]
public class MeteorShower : BossAttack
{
    [Header("References")]
    public GameObject meteorPrefab;

    [Header("Stats")]
    [SerializeField] private int count = 5;
    [SerializeField] private int distanceBetween = 10;
    [SerializeField] private float spawnHeight = 20f;

    public void Start()
    {
        count = Random.Range(5, 12);
    }

    public override void ExecuteAttack(Transform boss, Transform firePoint)
    {
        for(int i = 0; i < count; i++)
        {
            int randomDistance = Random.Range(10, 20);

            Vector3 pos = new Vector3(firePoint.position.x + i * randomDistance - 30, firePoint.position.y + spawnHeight, 0);
            Instantiate(meteorPrefab, pos, Quaternion.identity);
        }
    }

}

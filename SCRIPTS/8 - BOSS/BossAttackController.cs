using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackController : MonoBehaviour
{
    public Transform firePoint;
    public List<BossAttack> attackPatterns;
    public float baseTimeBetweenAttacks = 2f;  // Starting time
    public float minTimeBetweenAttacks = 0.5f; // Minimum time as HP lowers

    private int currentIndex = 0;
    private bool active = true;
    private BossStats bossStats;

    private void Start()
    {
        bossStats = GetComponent<BossStats>();
        StartCoroutine(AttackLoop());
    }

    private IEnumerator AttackLoop()
    {
        while (active)
        {
            // Calculate time between attacks based on current HP %
            float hpPercent = bossStats.GetHealthPercent(); // value between 0 and 1
            float scaledTime = Mathf.Lerp(minTimeBetweenAttacks, baseTimeBetweenAttacks, hpPercent);
            yield return new WaitForSeconds(scaledTime);

            while (DialogueManager.Instance != null && DialogueManager.Instance.isTalking)
                yield return null;

            if (attackPatterns.Count > 0)
            {
                attackPatterns[currentIndex].ExecuteAttack(transform, firePoint);
                currentIndex = (currentIndex + 1) % attackPatterns.Count;
            }
        }
    }
}

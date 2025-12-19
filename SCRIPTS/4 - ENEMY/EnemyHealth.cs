using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    [Header("Settings")]
    public int maxHealth;
    private int currentHealth;

    [Header("Coin")]
    public GameObject coinPrefab;
    [SerializeField] private int coinAmount = 3;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        DropCoins();
        if (currentHealth <= 0) Die();
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }

    private void DropCoins()
    {
        for(int i = 0; i < coinAmount; i++)
        {
            Vector2 spawnPos = transform.position;
            GameObject coin = Instantiate(coinPrefab, spawnPos, Quaternion.identity);

            Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();

            if(rb != null )
            {
                float forceX = Random.Range(-2f, 2f);
                float forceY = Random.Range(6f, 9f);
                rb.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);
            }
        }
    }
}

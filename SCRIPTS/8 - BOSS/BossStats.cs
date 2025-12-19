using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BossStats : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] private int baseHealth = 100;
    private int currentHealth;
    private int level = 1;

    [Header("Coin")]
    public GameObject coinPrefab;
    [SerializeField] private int coinAmount = 3;

    [Header("UI")]
    [SerializeField] private GameObject bossUI;
    [SerializeField] private Slider healthBar;

    private Vector3 originalPos;

    private void Awake()
    {
        originalPos = transform.position;
    }

    public void Respawn()
    {
        currentHealth = baseHealth;  // Reset to full base health
        UpdateHealthUI();

        // Reset any dialogue triggers or other states
        foreach (var trigger in DialogueManager.Instance.dialogueTriggers)
        {
            trigger.triggered = false;
        }
    }


    public void Despawn()
    {
        bossUI.transform.localScale = Vector3.zero;
    }

    private void Start()
    {
        currentHealth = Mathf.RoundToInt(baseHealth * (1 + (level - 1) * 0.5f));
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        if (DialogueManager.Instance.isTalking) return;

        SoundManager.Instance.PlaySFX("Hurt");

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0); // Clamp to 0

        float hpPercent = (float)currentHealth / baseHealth * 100f;
        DialogueManager.Instance.TryTriggerHPDialogue(hpPercent);

        DropCoins();
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.maxValue = baseHealth;
            healthBar.value = currentHealth;
        }
    }

    private void DropCoins()
    {
        for (int i = 0; i < coinAmount; i++)
        {
            Vector2 spawnOffset = new Vector2(Random.Range(-0.5f, 0.5f), 0.3f);
            Vector2 spawnPos = (Vector2)transform.position + spawnOffset;

            GameObject coin = Instantiate(coinPrefab, spawnPos, Quaternion.identity);

            Rigidbody2D rb = coin.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float forceX = Random.Range(-4f, 4f);
                float forceY = Random.Range(6f, 9f);
                rb.AddForce(new Vector2(forceX, forceY), ForceMode2D.Impulse);
            }
        }
    }

    public float GetHealthPercent()
    {
        return (float)currentHealth / baseHealth;
    }


    private void Die()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene("End");
    }
}

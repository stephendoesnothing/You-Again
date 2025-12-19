using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    public int maxHealth;
    private int currentHealth;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI healthText;
    public Slider healthBar;

    [Header("Respawn")]
    public Transform respawnPoint;
    public BossStats bossStats;
    public AudioClip bgMusic;

    private void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(int damage)
    {
        var playerMovement = GetComponent<PlayerMovement>();
        if (playerMovement.isInvulnerable) return;

        SoundManager.Instance.PlaySFX("Hurt");

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            playerMovement.TriggerInvincibility();
        }

        UpdateUI();
    }


    private void Die()
    {
        transform.position = respawnPoint.position;
        currentHealth = maxHealth;
        UpdateUI();

        SoundManager.Instance.PlayMusic(bgMusic, loop: true);

        // implement boss reset

        if (bossStats != null)
        {
            bossStats.Despawn();
            bossStats.Respawn();
        }
    }

    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth = maxHealth;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (healthText != null)
        {
            healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCounter : MonoBehaviour
{
    public static CoinCounter instance;
    public TextMeshProUGUI coinText;
    public int currency = 0;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void AddCoin(int amount)
    {
        currency += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (coinText != null) coinText.text = currency.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class UpgradeNode : MonoBehaviour
{
    [Header("Attributes")]
    [SerializeField] private Color affordableColor = Color.white;
    [SerializeField] private Color unaffordableColor = Color.gray;
    public int cost = 10;

    [Header("References")]
    public List<UpgradeNode> unlocks;
    public List<UpgradeNode> removes;
    public Button upgradeButton;
    public TextMeshProUGUI buttonText;
    public Image buttonIcon;

    [Header("On Buy")]
    public UnityEvent onBuyAction;

    private bool isUnlocked = false;

    private void Start()
    {
        if(upgradeButton == null)
        {
            upgradeButton = GetComponent<Button>();
        }

        upgradeButton.onClick.AddListener(PurchaseUpgrade);
    }

    private void Update()
    {
        if(!isUnlocked)
        {
            bool canAfford = CoinCounter.instance.currency >= cost;
            upgradeButton.interactable = canAfford;
            buttonIcon.color = canAfford ? affordableColor : unaffordableColor;
        }
    }

    private void PurchaseUpgrade()
    {
        if (isUnlocked) return;

        if(CoinCounter.instance.currency >= cost)
        {
            CoinCounter.instance.AddCoin(-cost);
            isUnlocked = true;

            buttonText.text = "Purchased";
            upgradeButton.interactable = false;
            buttonIcon.color = affordableColor;

            foreach(var upgrade in unlocks)
            {
                upgrade.gameObject.SetActive(true);
            }

            foreach (var upgrade in removes)
            {
                upgrade.gameObject.SetActive(false);
            }
        }

        onBuyAction?.Invoke();
    }
}

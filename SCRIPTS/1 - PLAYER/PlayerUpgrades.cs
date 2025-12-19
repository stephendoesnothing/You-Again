using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgrades : MonoBehaviour
{
    [Header("Health")]
    public int bonusHealth = 100;
    public PlayerHealth playerHealth;

    [Header("Weapons")]
    public GameObject bow;
    public GameObject longbow;
    public GameObject crossbow;
    public GameObject staff;

    [Header("Armors")]
    public GameObject woodArmor;
    public GameObject tinArmor;
    public GameObject ironArmor;
    public GameObject goldArmor;

    [Header("Magnet")]
    public CoinMagnet coinMagnet;

    private void DeactivateAllWeapons()
    {
        if (bow != null) bow.SetActive(false);
        if (longbow != null) longbow.SetActive(false);
        if (crossbow != null) crossbow.SetActive(false);
        if (staff != null) staff.SetActive(false);
    }

    public void WoodArmor()
    {
        if (woodArmor != null) woodArmor.SetActive(true);
    }

    public void TinArmor()
    {
        if (woodArmor != null) woodArmor.SetActive(false);
        if (tinArmor != null) tinArmor.SetActive(true);
    }

    public void IronArmor()
    {
        if (tinArmor != null) tinArmor.SetActive(false);
        if (ironArmor != null) ironArmor.SetActive(true);
    }

    public void GoldArmor()
    {
        if (ironArmor != null) ironArmor.SetActive(false);
        if (goldArmor != null) goldArmor.SetActive(true);
    }

    public void ApplyArmorUpgrade()
    {
        playerHealth.IncreaseMaxHealth(bonusHealth);
    }

    public void ApplyBowUpgrade()
    {
        DeactivateAllWeapons();
        if (bow != null) bow.SetActive(true);
    }

    public void ApplyLongbowUpgrade()
    {
        DeactivateAllWeapons();
        if (longbow != null) longbow.SetActive(true);
    }

    public void ApplyCrossbowUpgrade()
    {
        DeactivateAllWeapons();
        if (crossbow != null) crossbow.SetActive(true);
    }

    public void ApplyStaffUpgrade()
    {
        DeactivateAllWeapons();
        if (staff != null) staff.SetActive(true);
    }

    public void ApplyMagnetUpgrade()
    {
        if (coinMagnet != null) coinMagnet.IncreaseRange();
    }
}

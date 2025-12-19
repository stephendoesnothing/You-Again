using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public int value = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            CoinCounter.instance.AddCoin(value);
            SoundManager.Instance.PlaySFX("PickCoin");
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // Rotating the player && weapon sprite

        Vector3 localScale = transform.localScale;
        localScale.y = angle > 90 || angle < -90f ? -1f : 1f;
        transform.localScale = localScale;

        Vector3 playerScale = player.localScale;
        playerScale.x = angle > 90 || angle < -90f ? -1f : 1f;
        player.localScale = playerScale;
    }
}

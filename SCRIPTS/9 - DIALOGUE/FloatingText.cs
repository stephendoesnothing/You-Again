using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public float speed = 0.5f;
    public float fadeSpeed = 0.5f;
    public float lifetime = 2f;
    public float maxRotationAngle = 5f;

    private Text text;
    public Color startColor;
    private float timer = 0f;

    private void Start()
    {
        text = GetComponentInChildren<Text>();
        startColor = text.color;
        startColor.a = 1f;
        text.color = startColor;

        float angle = Random.Range(-maxRotationAngle, maxRotationAngle);
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void Update()
    {
        transform.position += Vector3.up * speed * Time.deltaTime;

        timer += Time.deltaTime;

        float alpha = Mathf.Lerp(1f, 0, timer / lifetime);
        Color c = startColor;
        c.a = alpha;
        text.color = c;
    }
}

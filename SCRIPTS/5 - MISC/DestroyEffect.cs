using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEffect : MonoBehaviour
{
    [SerializeField] private float lifetime = 0.25f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}

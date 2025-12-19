using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFlip : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (PlayerReference.instance.transform.position.x < transform.position.x)
        {
            sr.flipX = false;
        }
        else sr.flipX = true;
    }
}

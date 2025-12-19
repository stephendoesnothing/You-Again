using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [SerializeField] private float dropDuration = 0.4f;

    private PlatformEffector2D currentEffector;

    private void Update()
    {
        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && currentEffector != null)
        {
            StartCoroutine(DisablePlatformTemporarily(currentEffector));
        }
    }

    private IEnumerator DisablePlatformTemporarily(PlatformEffector2D effector)
    {
        effector.rotationalOffset = 180f; // Flip the platform upside down
        yield return new WaitForSeconds(dropDuration);
        effector.rotationalOffset = 0f;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out PlatformEffector2D effector))
        {
            currentEffector = effector;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out PlatformEffector2D effector) && currentEffector == effector)
        {
            currentEffector = null;
        }
    }
}
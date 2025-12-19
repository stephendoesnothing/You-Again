using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    public CameraShake camShake;
    private Animator animator;

    [Header("Gun Settings")]
    [SerializeField] private float bulletSpeed = 20f;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float camShakeIntensity = 1.0f;
    [SerializeField] private float fireDelay = 0.25f; // Delay after animation trigger

    private float nextFireTime;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextFireTime)
        {

            SoundManager.Instance.PlaySFX("Shoot");
            StartCoroutine(ShootWithDelay());
            nextFireTime = Time.time + fireRate;
        }
    }

    private IEnumerator ShootWithDelay()
    {
        if (bulletPrefab == null || firePoint == null) yield break;

        animator.SetTrigger("Fire");
        yield return new WaitForSeconds(fireDelay);

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = firePoint.right * bulletSpeed;
        }

        camShake.Shake(camShakeIntensity);
    }
}

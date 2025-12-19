using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    private Camera mainCam;

    [Header("Settings")]
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private float smoothTime = 0.1f;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        mainCam = Camera.main;
    }

    private void LateUpdate()
    {
        if (player == null || mainCam == null) return;

        Vector3 mouseScreenPos = Input.mousePosition;
        Vector3 mouseWorldPos = mainCam.ScreenToWorldPoint(new Vector3(mouseScreenPos.x, mouseScreenPos.y, mainCam.nearClipPlane));
        mouseWorldPos.z = 0f;

        Vector3 direction = mouseWorldPos - player.position;
        float distance = direction.magnitude;

        if (distance > maxDistance) direction = direction.normalized * maxDistance;

        Vector3 targetPos = player.position + direction;

        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothTime);
    }
}

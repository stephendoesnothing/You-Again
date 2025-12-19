using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Homing : MonoBehaviour
{
    private Transform target;
    private Rigidbody2D rb;

    private Vector2 initialDirection;
    private float speed;
    private bool initialized = false;

    public float rotateSpeed = 200f;

    public void Initialize(Vector2 direction, float moveSpeed)
    {
        initialDirection = direction;
        speed = moveSpeed;
        initialized = true;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = PlayerReference.instance.transform;
    }

    private void FixedUpdate()
    {
        if (!initialized || target == null || rb == null)
            return;

        Vector2 toTarget = ((Vector2)target.position - rb.position).normalized;
        float rotateAmount = Vector3.Cross(toTarget, transform.right).z;

        rb.angularVelocity = -rotateAmount * rotateSpeed;
        rb.velocity = transform.right * speed;
    }

    private void OnEnable()
    {
        // Ensure forward direction is aligned with initial spread
        if (initialized)
            transform.right = initialDirection;
    }
}
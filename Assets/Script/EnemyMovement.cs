using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 0f;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rb.velocity = new Vector2(moveSpeed, 0f);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        moveSpeed *= -1;    // moveSpeed = -moveSpeed; Reverse direction
        FlipSprite();
    }

    private void FlipSprite()
    {
        transform.localScale = new Vector2 (-(Mathf.Sign(rb.velocity.x)), 1f);
    }
}

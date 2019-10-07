using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Rigidbody2D rb;
    private Vector2 originalPos;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        originalPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
    }

    void Update()
    {
        Vector2 screenPoint = Camera.current.WorldToViewportPoint(gameObject.transform.position);
        if (screenPoint.x < 0 || screenPoint.x > 1 || screenPoint.y < 0 || screenPoint.y > 1)
        {
            ResetPlayer();
        }
    }

    void FixedUpdate()
    {
        if (Input.GetAxisRaw("Horizontal") != 0)
        {
            float horiMov = Input.GetAxisRaw("Horizontal") * moveSpeed;

            Vector2 dom = new Vector2(horiMov, 0);
            rb.AddForce(dom);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "Exit")
        {
            ResetPlayer();
            //Do Scene Change
        }
    }

    private void ResetPlayer()
    {
        rb.velocity = new Vector2(0, 0);
        gameObject.transform.position = originalPos;
    }
}

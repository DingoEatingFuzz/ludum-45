using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    private Rigidbody2D rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
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
}

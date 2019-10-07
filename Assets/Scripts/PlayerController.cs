using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public OurNetworkManager network;
    private Rigidbody2D rb;
    private Vector2 originalPos;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        originalPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
    }

    void Update()
    {
        Vector2 screenPoint = Camera.main.WorldToViewportPoint(gameObject.transform.position);
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
            network.NextLevel();
            //Destroy();
            //Do Prefab (or level) change


            //PHASE 3:
            //  a. interstitial you did it graphic
            //  b. uhhhhh timer for 6 seconds
            // PHASE 3 acceptanceCriteria
            //  1. e2e test synchronized transitions

            //PHASE 4:
            //  a. interstitial includes screenshot of victor screen
            //  b. that's it
            //PHASE 4 acceptance criteria
            //  1. e2e test of victor
       

        }
        if (collision.name == "DeathBox")
        {
            ResetPlayer();
        }
    }

    private void ResetPlayer()
    {
        network.ResetLevel();
    }

    public void ResetPlayerPosition()
    {
        rb.velocity = new Vector2(0, 0);
        gameObject.transform.position = originalPos;
    }
}

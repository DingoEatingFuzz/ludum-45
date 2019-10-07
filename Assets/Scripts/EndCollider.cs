using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndCollider : MonoBehaviour
{
    //Apply this to the ending item with a collider and rigid body
    public bool isWin = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("OnTriggerEnter2D");
        isWin = true;
    }
}

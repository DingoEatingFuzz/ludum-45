using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class miniGameTimer : MonoBehaviour
{
    public float miniGameTimeLimit = 5;
    private void Start()
    {
        InvokeRepeating("changeMiniGame",0.0f,miniGameTimeLimit);
    }

    void changeMiniGame()
    {
        //Deal with changing the minigame here.
        //send the RPC here.
        Debug.Log("Change Game");
    }
}

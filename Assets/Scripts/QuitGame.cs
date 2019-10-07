using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour
{
    public OurNetworkManager network;

    public void doExitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void resetGame()
    {
        network.ResetLevel();
    }
}

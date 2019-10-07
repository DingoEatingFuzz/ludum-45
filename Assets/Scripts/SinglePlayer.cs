using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public static class GameManager {
    public static bool isSinglePlayer = false;
}

public class SinglePlayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartSinglePlayer() {
        GameManager.isSinglePlayer = true;
        SceneManager.LoadScene("_Game");
    }
}

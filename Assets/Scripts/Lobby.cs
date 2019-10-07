using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BeardedManStudios.Forge.Networking.Unity.Lobby;
using BeardedManStudios.Forge.Networking.Unity;

[RequireComponent(typeof(LobbyManager))]
public class Lobby : MonoBehaviour
{
    LobbyManager lobby;
    GameObject startButton;
    Text statusText;
    // Start is called before the first frame update
    void Start()
    {
        this.lobby = this.GetComponent<LobbyManager>();
        this.startButton = GameObject.Find("StartGameButton");
        this.statusText = GameObject.Find("StatusText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        var players = this.lobby.LobbyPlayers;
        var isServer = NetworkManager.Instance.IsServer;

        this.startButton.SetActive(isServer && players.Count >= 2);

        if (players.Count >= 2) {
            this.statusText.text = "Good, you're both here!";
        }
    }
}

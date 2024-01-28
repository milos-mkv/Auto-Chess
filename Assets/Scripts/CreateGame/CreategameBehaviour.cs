using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreategameBehaviour : MonoBehaviour
{
    [SerializeField] private Button menuButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private TMP_InputField usernameInput;

    private void Awake() 
    {
        menuButton.onClick.AddListener(() => {
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene(GameScenes.MainMenuScene.ToString());
        });
        hostButton.onClick.AddListener(() => { 
            if (string.IsNullOrWhiteSpace(PlayerConfiguration.playerName))
            {
                Debug.Log("Missing username.");
                return;
            }
            Debug.Log("Starting server!");
            MultiplayerManager.Instance.StartHost();
        });
        joinButton.onClick.AddListener(() => {
            if (string.IsNullOrWhiteSpace(PlayerConfiguration.playerName))
            {
                Debug.Log("Missing username.");
                return;
            }
            Debug.Log("Staring client!");
            MultiplayerManager.Instance.StartClient();

        });

        usernameInput.onSubmit.AddListener((string value) => {
            PlayerConfiguration.playerName = value;
        });

    }

    private void Event_OnClienConnectNotfication(ulong obj, string message)
    {
        Debug.Log("Event_OnClienConnectNotfication " + obj + " " + message);
        MultiplayerManager.Instance.SendClientUsernameServerRpc(PlayerConfiguration.playerName, new ServerRpcParams());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            MultiplayerManager.Instance.SendClientUsernameServerRpc(PlayerConfiguration.playerName, new ServerRpcParams());

        }
    }
}

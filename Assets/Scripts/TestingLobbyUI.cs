using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestingLobbyUI : MonoBehaviour
{
    [SerializeField] private Button createGameButton;
    [SerializeField] private Button joinGameButton;
    
    private void Awake() 
    {
        createGameButton.onClick.AddListener(() => {
            AutoChessMultiplayer.Instance.StartHost();
            NetworkManager.Singleton.SceneManager.LoadScene("CharacterSelectScreen", LoadSceneMode.Single);
        });

        joinGameButton.onClick.AddListener(() => {
            AutoChessMultiplayer.Instance.StartClient();
        });
    }
}

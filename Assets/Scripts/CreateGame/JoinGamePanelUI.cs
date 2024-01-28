using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class JoinGamePanelUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private Button closeButton;

    private void Awake() 
    {
        closeButton.onClick.AddListener(Hide);
    }

    private void Start() 
    {
        MultiplayerManager.Instance.OnTryingToJoinGame += Event_OnTryingToJoinGame;
        MultiplayerManager.Instance.OnFailedToJoinGame += Event_OnFailedToJoinGame;
        MultiplayerManager.Instance.OnManageToJoinGame += Event_OnManageToJoinGame;
        Hide();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Event_OnTryingToJoinGame(ulong clientId)
    {
        Show();
        messageText.text = "Joining game!";
        closeButton.gameObject.SetActive(false);
    }

    private void Event_OnManageToJoinGame(ulong clientId)
    {
        Hide();
    }

    private void Event_OnFailedToJoinGame(ulong clientId)
    {
        Show();
        messageText.text = NetworkManager.Singleton.DisconnectReason;
        closeButton.gameObject.SetActive(true);
    }

    private void OnDestroy() 
    {
        MultiplayerManager.Instance.OnTryingToJoinGame -= Event_OnTryingToJoinGame;
        MultiplayerManager.Instance.OnFailedToJoinGame -= Event_OnFailedToJoinGame;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Netcode;
using UnityEngine.SceneManagement;

public class AutoChessMultiplayer : NetworkBehaviour
{
    public static AutoChessMultiplayer Instance { get; private set; }

    public event EventHandler OnTryingToJoinGame;
    public event EventHandler OnFailedToJoinGame;

    private void Awake() 
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartHost()
    {
        
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.StartHost();
    }

    public override void OnDestroy() 
    {
        UnityEngine.Debug.Log("ASDASDASd");
    }

    public void StartClient()
    {
        OnTryingToJoinGame?.Invoke(this, EventArgs.Empty);
        // NetworkManager.Singleton.OnClientDisconnectCallback += 
        NetworkManager.Singleton.StartClient();
    }

    private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        if (SceneManager.GetActiveScene().name != "SelectCharacterScreen")
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game started!";

        }
        connectionApprovalResponse.Approved = true;
    }


}

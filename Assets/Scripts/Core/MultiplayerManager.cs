using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MultiplayerManager : NetworkBehaviour
{
    public const int MAX_PLAYERS_NUMBER = 2;

    public static MultiplayerManager Instance { get; private set; }

    public event Action<ulong> OnTryingToJoinGame;
    public event Action<ulong> OnFailedToJoinGame;
    public event Action<ulong> OnManageToJoinGame;

    public NetworkList<PlayerNetworkData> playerNetworkDataList;

    private void Awake() 
    {
        Instance = this;
        playerNetworkDataList = new NetworkList<PlayerNetworkData>();


        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
    }

    public void StartHost()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += ApproveClientConnectionCallback;
        NetworkManager.Singleton.OnClientConnectedCallback += NetworkManager_OnClientConnectedCallback;
        NetworkManager.Singleton.StartHost();   
    }

    private void NetworkManager_OnClientConnectedCallback(ulong clientId)
    {
        // When client connects to the server create new playuer for player data list.
        Debug.Log("Client connected: " + clientId);

        PlayerNetworkData playerNetworkData = new PlayerNetworkData {
            clientId = clientId
        };

        playerNetworkDataList.Add(playerNetworkData);


        AasdClientRpc(clientId);
    }

    [ClientRpc]
    public void AasdClientRpc(ulong clientId)
    {
        if (NetworkManager.Singleton.LocalClientId == clientId)
        {
        OnManageToJoinGame?.Invoke(clientId);

        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SendClientUsernameServerRpc(string username, ServerRpcParams clientRpcParams = default)
    {
        for (int i = 0; i < playerNetworkDataList.Count; i++) {
            PlayerNetworkData playerNetworkData = playerNetworkDataList[i];
            if (clientRpcParams.Receive.SenderClientId == playerNetworkData.clientId) {
                Debug.Log(string.Format("Updating client {0} username to {1}", playerNetworkData.clientId, username));
                playerNetworkData.username = username;
                break;
            }   
        }
    }

    // Exectued on host.
    private void ApproveClientConnectionCallback(NetworkManager.ConnectionApprovalRequest connectionApprovalRequest, NetworkManager.ConnectionApprovalResponse connectionApprovalResponse)
    {
        Debug.Log("Approving client connection: " + connectionApprovalRequest.ClientNetworkId);

        if (SceneManager.GetActiveScene().name != GameScenes.CreateGameScene.ToString())
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game started!";
            return;
        }
        if (NetworkManager.Singleton.ConnectedClients.Count >= MAX_PLAYERS_NUMBER)
        {
            connectionApprovalResponse.Approved = false;
            connectionApprovalResponse.Reason = "Game is full!";
            return;
        }
        connectionApprovalResponse.Approved = true;
        connectionApprovalResponse.Reason = "Game started!";
        connectionApprovalResponse.CreatePlayerObject = true;
    }

    public void Shutdown()
    {
        NetworkManager.Singleton.Shutdown();
    }

    public void DisconnectPlayer(NetworkObject player)
    {
        NetworkManager.Singleton.DisconnectClient(player.OwnerClientId);
    }

    public void StartClient()
    {
        OnTryingToJoinGame?.Invoke(OwnerClientId);
        NetworkManager.Singleton.OnClientDisconnectCallback += NetworkManager_OnClientDisconnectCallback;
        NetworkManager.Singleton.StartClient();
    }

    private void NetworkManager_OnClientDisconnectCallback(ulong clientId)
    {
        Debug.Log("CLIENT DISCONNECTED!");
        OnFailedToJoinGame?.Invoke(clientId);
    }
}

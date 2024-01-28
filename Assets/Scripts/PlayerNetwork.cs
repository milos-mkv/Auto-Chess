using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.SceneManagement;


public class PlayerNetwork : NetworkBehaviour
{
    public NetworkVariable<int> randomNumber = new NetworkVariable<int>(1, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    void Start()
    {
        
    }

    public override void OnNetworkSpawn()
    {
        // SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetSceneByName("Gameplay"));
        randomNumber.OnValueChanged += (int prev, int vale) => { Debug.Log(string.Format("Prev: {0} -> New: {1}", prev, vale)); };
    }

    void Update()
    {
        if (!IsOwner)
            return;
        
        Vector3 movedir = new Vector3(0, 0, 0);


        if (Input.GetKey(KeyCode.Space))
            TestServerRpc(new ServerRpcParams());

        if (Input.GetKey(KeyCode.W)) movedir.y =  1f;
        if (Input.GetKey(KeyCode.S)) movedir.y = -1f;
        if (Input.GetKey(KeyCode.A)) movedir.x = -1f;
        if (Input.GetKey(KeyCode.D)) movedir.x =  1f;

        float moveSpeed = 3f;
        transform.position += movedir * moveSpeed * Time.deltaTime;
    }

    [ServerRpc] // Function does not run on client. Only run on server.
    private void TestServerRpc(ServerRpcParams rpcParams)
    {
        Debug.Log("Test Server Rpc" + OwnerClientId);
    }

    [ClientRpc]
    private void TestClientRpc(ClientRpcParams clientRpcParams)
    {
        Debug.Log("Test Client Rpc");
    }
}

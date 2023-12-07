using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;
using UnityEngine.SceneManagement;

public class NetworkClient : IDisposable
{
    private NetworkManager networkManager;

    private const string MENU_SCENE_NAME = "MenuScene";

    public NetworkClient(NetworkManager networkManager)
    {
        this.networkManager = networkManager;

        networkManager.OnClientDisconnectCallback += OnClientDisconnect;
    }

    private void OnClientDisconnect(ulong clientId)
    {
        // Since the id of host is equal to zero by default this assessing process
        // prevent to server wouldnt decide to shut down when any client leaves
        if(clientId != 0 && clientId != networkManager.LocalClientId) { return; }

        if(SceneManager.GetActiveScene().name != MENU_SCENE_NAME)
        {
            SceneManager.LoadScene(MENU_SCENE_NAME);
        }
        if (networkManager.IsConnectedClient)
        {
            networkManager.Shutdown();
        }
    }

    public void Dispose()
    {
        if(networkManager != null)
        {
            networkManager.OnClientDisconnectCallback -= OnClientDisconnect;

        }
    }
}

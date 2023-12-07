using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System.Threading.Tasks;
using Unity.Services.Core;
using UnityEngine.SceneManagement;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Authentication;
using System;
public class ClientGameManager : IDisposable
{
    private const string MENU_SCENE_NAME = "MenuScene";

    private JoinAllocation joinAllocation;
    private NetworkClient networkClient;

    public async Task<bool> InitAsync()
    {
        await UnityServices.InitializeAsync();

        networkClient = new NetworkClient(NetworkManager.Singleton);

        AuthState authState = await AuthenticationWrapper.DoAuth();
        if(authState == AuthState.Authenticated)
        {
            return true;
        }
        return false;
    }

    public async Task StartClientAsync(string joinCode)
    {
        try
        {
            joinAllocation = await Relay.Instance.JoinAllocationAsync(joinCode);
        }
        catch(Exception e)
        {
            Debug.Log(e);
            return;
        }

        UnityTransport transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        RelayServerData relayServerData = new RelayServerData(joinAllocation, "udp");
        transport.SetRelayServerData(relayServerData);

        UserData userData = new UserData
        {
            userName = PlayerPrefs.GetString(NameSelector.PLAYER_NAME_KEY, "Missing Name"),
            userAuthId = AuthenticationService.Instance.PlayerId
        };
        string payload = JsonUtility.ToJson(userData);
        byte[] payloadBytes = System.Text.Encoding.UTF8.GetBytes(payload);
        NetworkManager.Singleton.NetworkConfig.ConnectionData = payloadBytes;
        NetworkManager.Singleton.StartClient();
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene(MENU_SCENE_NAME);
    }

    public void Dispose()
    {
        networkClient?.Dispose();
    }
}

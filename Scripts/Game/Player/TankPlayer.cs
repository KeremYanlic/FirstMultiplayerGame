using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.Netcode;
using Cinemachine;
using Unity.Collections;

public class TankPlayer : NetworkBehaviour
{
    public static event Action<TankPlayer> OnPlayerSpawned;
    public static event Action<TankPlayer> OnPlayerDespawned;

    [field: SerializeField] public Health Health { get; private set; }

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private const int CVM_PRIORITY = 15;

    public NetworkVariable<FixedString32Bytes> PlayerName = new NetworkVariable<FixedString32Bytes>();
    


    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            UserData userData = HostSingleton.Instance.hostGameManager.NetworkServer.GetUserDataByClientID(OwnerClientId);
            PlayerName.Value = userData.userName;

            OnPlayerSpawned?.Invoke(this);
        }

        if (IsOwner)
        {
            cinemachineVirtualCamera = GetComponentInChildren<CinemachineVirtualCamera>();
            cinemachineVirtualCamera.Priority = CVM_PRIORITY;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            OnPlayerDespawned?.Invoke(this);
        }
    }

 
}

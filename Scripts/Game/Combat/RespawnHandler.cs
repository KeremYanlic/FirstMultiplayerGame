using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class RespawnHandler : NetworkBehaviour
{
    [SerializeField] private NetworkObject playerPrefab;

    public override void OnNetworkSpawn()
    {
        if(!IsServer) { return; }

        TankPlayer[] players = FindObjectsOfType<TankPlayer>();
        foreach(TankPlayer player in players)
        {
            HandlePlayerSpawned(player);
        }

        TankPlayer.OnPlayerSpawned += HandlePlayerSpawned;
        TankPlayer.OnPlayerDespawned += HandlePlayerDespawned;
    }
    public override void OnNetworkDespawn()
    {
        if(!IsServer) { return; }

        TankPlayer.OnPlayerSpawned -= HandlePlayerSpawned;
        TankPlayer.OnPlayerDespawned -= HandlePlayerDespawned;
    }

    private void HandlePlayerSpawned(TankPlayer tankPlayer)
    {
        tankPlayer.Health.OnDie += (Health) => HandlePlayerDie(tankPlayer);
    }

  
    private void HandlePlayerDespawned(TankPlayer tankPlayer)
    {
        tankPlayer.Health.OnDie -= (Health) => HandlePlayerDie(tankPlayer);

    }
    private void HandlePlayerDie(TankPlayer player)
    {
        Destroy(player.gameObject);

        StartCoroutine(RespawnPlayer(player.OwnerClientId));
    }
    private IEnumerator RespawnPlayer(ulong ownerClienId)
    {
        yield return null;

        NetworkObject playerInstance = Instantiate(playerPrefab, SpawnPoint.GetRandomSpawnPos(), Quaternion.identity);
        playerInstance.SpawnAsPlayerObject(ownerClienId);
    }
}

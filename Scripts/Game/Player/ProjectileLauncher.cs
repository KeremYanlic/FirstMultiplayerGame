using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class ProjectileLauncher : NetworkBehaviour
{

    [Header("References")]
    [SerializeField] private InputReader inputReader;
    [SerializeField] private CoinWallet coinWallet;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private GameObject clientProjectilePrefab;
    [SerializeField] private GameObject muzzleFlash;
    [SerializeField] private Collider2D playerCollider;

    [Header("Settings")]
    [SerializeField] private float muzzleFlashDuration;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float fireRate;
    [SerializeField] private int costToFire;

    private bool shouldFire;
    private float timer;
    private float muzzleFlashTimer;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;

        inputReader.PrimaryFireEvent += HandlePrimaryFire;
    }
    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;

        inputReader.PrimaryFireEvent -= HandlePrimaryFire;
    }

    private void Update()
    {
        if(muzzleFlashTimer > 0f)
        {
            muzzleFlashTimer -= Time.deltaTime;

            if(muzzleFlashTimer <= 0f)
            {
                muzzleFlash.SetActive(false);
            }
        }

        if(!IsOwner) { return; }

        if(timer > 0f)
        {
            timer -= Time.deltaTime;
        }
        if(!shouldFire) { return; }
        if(timer > 0f) { return; }

        SpawnProjectileServerRpc(projectileSpawnPoint.position, projectileSpawnPoint.up);
        timer = 1 / fireRate;
    }
    [ServerRpc(RequireOwnership = false)]
    private void SpawnProjectileServerRpc(Vector3 spawnPos,Vector3 direction)
    {
        SpawnProjectileClientRpc(spawnPos, direction);
    }
    [ClientRpc]
    private void SpawnProjectileClientRpc(Vector3 spawnPos, Vector3 direction)
    {

        muzzleFlash.SetActive(true);
        muzzleFlashTimer = muzzleFlashDuration;

        GameObject projectileInstance = Instantiate(clientProjectilePrefab, spawnPos, Quaternion.identity);
        projectileInstance.transform.up = direction;

        Physics2D.IgnoreCollision(playerCollider, projectileInstance.GetComponent<Collider2D>());

        if(projectileInstance.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb)) 
        {
            rb.velocity = rb.transform.up * projectileSpeed;
        }

    }
    private void HandlePrimaryFire(bool shouldFire)
    {
        this.shouldFire = shouldFire;
    }

}

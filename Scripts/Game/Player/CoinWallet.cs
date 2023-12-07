using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using System;

public class CoinWallet : NetworkBehaviour
{
    public NetworkVariable<int> TotalCoin = new NetworkVariable<int>();

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!collision.TryGetComponent<Coin>(out Coin coin)) { return; }

        int coinValue = coin.Collect();

        if (!IsServer) return;

        TotalCoin.Value += coinValue;
    }

    public void SpendCoins(int costToFire)
    {
        TotalCoin.Value -= costToFire;
    }
}

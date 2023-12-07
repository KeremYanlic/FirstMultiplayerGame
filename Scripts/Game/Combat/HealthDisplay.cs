using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using System;

public class HealthDisplay : NetworkBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Image healthBarImage;

    public override void OnNetworkSpawn()
    {
        if (!IsClient) return;
        health.CurrentHealth.OnValueChanged += CurrentHealth_OnValueChanged;

        CurrentHealth_OnValueChanged(0, health.CurrentHealth.Value);
    }

    
    public override void OnNetworkDespawn()
    {
        if (!IsClient) return;
    }

    private void CurrentHealth_OnValueChanged(int previousValue, int newValue)
    {
        healthBarImage.fillAmount = (float)newValue / health.MaxHealth;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class PlayerAiming : NetworkBehaviour
{
    [SerializeField] private Transform turretTransform;

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
    }
    private void Update()
    {
        if (!IsOwner) return;

        Vector2 aimScreenPosition = Input.mousePosition;
        Vector2 aimWorldPosition = Camera.main.ScreenToWorldPoint(aimScreenPosition);

        turretTransform.up = new Vector2(aimWorldPosition.x - turretTransform.position.x, aimWorldPosition.y - turretTransform.position.y);
    }
}

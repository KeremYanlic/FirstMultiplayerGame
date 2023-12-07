using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private Transform bodyTransform;
    [SerializeField] private Rigidbody2D tankRB;

    [Header("Settings")]
    [SerializeField] private float movementSpeed = 4f;
    [SerializeField] private float turningSpeed = 30f;

    private Vector2 previousMovementInput;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
    }
    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
    }

    private void Update()
    {
        if (!IsOwner) return;

        previousMovementInput = GetInputs();
        float zRotation = previousMovementInput.x * -turningSpeed * Time.deltaTime;
        bodyTransform.Rotate(0f, 0f, zRotation);
    }
    private void FixedUpdate()
    {
        if (!IsOwner) return;

        tankRB.velocity = (Vector2)bodyTransform.up * previousMovementInput.y * movementSpeed;
    }
    private Vector2 GetInputs()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        return new Vector2(horizontalInput, verticalInput);
    }
}

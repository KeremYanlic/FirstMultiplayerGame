using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Collections;

public class PlayerNameDisplay : MonoBehaviour
{
    [SerializeField] private TankPlayer tankPlayer;
    [SerializeField] private TextMeshProUGUI overheadNameText;

    private void Start()
    {
        OnPlayerNameChanged(string.Empty, tankPlayer.PlayerName.Value);
        tankPlayer.PlayerName.OnValueChanged += OnPlayerNameChanged;
    }

    private void OnPlayerNameChanged(FixedString32Bytes previousValue, FixedString32Bytes newValue)
    {
        overheadNameText.text = newValue.ToString();
    }

    private void OnDestroy()
    {
        tankPlayer.PlayerName.OnValueChanged += OnPlayerNameChanged;

    }
}

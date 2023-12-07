using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NameSelector : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameField;
    [SerializeField] private Button connectBtn;
    [SerializeField] private int minNameLength = 1;
    [SerializeField] private int maxNameLength = 12;

    public const string PLAYER_NAME_KEY = "PlayerName";

    private void Start()
    {
        if(SystemInfo.graphicsDeviceType == UnityEngine.Rendering.GraphicsDeviceType.Null)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            return;
        }

        nameField.text = PlayerPrefs.GetString(PLAYER_NAME_KEY, string.Empty);
        HandleNameChanged();
    }

    public void HandleNameChanged()
    {
        connectBtn.interactable = 
            nameField.text.Length >= minNameLength &&
            nameField.text.Length <= maxNameLength;
    }

    public void Connect()
    {
        PlayerPrefs.SetString(PLAYER_NAME_KEY, nameField.text);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HostSingleton : MonoBehaviour
{
    private static HostSingleton instance;

    public static HostSingleton Instance
    {
        get
        {
            if (instance != null) return instance;

            instance = FindObjectOfType<HostSingleton>();

            if(instance == null)
            {
                return null;
            }

            return instance;
        }
    }

    public HostGameManager hostGameManager
    {
        get; private set;
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void CreateHost()
    {
        hostGameManager = new HostGameManager();
    }
    private void OnDestroy()
    {
        hostGameManager.Dispose();
    }
}

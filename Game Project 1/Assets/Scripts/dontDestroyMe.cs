using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dontDestroyMe : MonoBehaviour
{
    public static dontDestroyMe Instance;
    
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance);
            Instance = this;
        }

        else
        {
            Instance = this;
        }
        
        DontDestroyOnLoad(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = GetComponent<T>();
        }
        else
        {
            Debug.LogWarning("Multiple instance of " + typeof(T).Name + "detected" + ", this instance will not be used"); 
        }
    }
}

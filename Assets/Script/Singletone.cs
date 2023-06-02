using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singletone<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance{ get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this as T;
    }
}

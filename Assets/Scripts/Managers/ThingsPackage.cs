using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThingsPackage : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}

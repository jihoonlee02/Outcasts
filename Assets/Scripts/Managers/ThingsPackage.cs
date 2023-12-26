using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThingsPackage : MonoBehaviour
{
    private void Awake()
    {
        if (FindObjectsOfType<ThingsPackage>().Count() > 1) Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
}

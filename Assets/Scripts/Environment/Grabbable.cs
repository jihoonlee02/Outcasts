using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour
{
    public void Grab()
    {
        
    }

    public void UnGrab()
    {
        GameManager.Instance.Ashe.IsLifting = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Can be destroyed by GameObjects that envoke break on collision
public class Breakable : MonoBehaviour
{
    public void Break()
    {
        gameObject.SetActive(false);
    }
}

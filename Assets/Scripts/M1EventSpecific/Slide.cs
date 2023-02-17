using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    [SerializeField] private GameObject[] information;
    int index = 0;
    private void Start()
    {
        foreach (var info in information)
            info.SetActive(false);
    }

    public void AddInfo()
    {
        information[index].gameObject.SetActive(true);
        if (index < information.Length - 1) index++;
    }

    public void RemoveInfo()
    {
        information[index].gameObject.SetActive(false);
        if (index > 0) index--;
    }

    public void ResetInfo()
    {
        
    }
}

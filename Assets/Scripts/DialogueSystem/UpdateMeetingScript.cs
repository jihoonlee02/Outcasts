using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMeetingScript : MonoBehaviour
{
    [SerializeField] private float[] delay;
    [SerializeField] private GameObject[] orderreveal;
    private float cooldown;
    private int idx = 0;
    private void Start()
    {
        foreach (GameObject go in orderreveal) 
        { 
            go.SetActive(false);
        }
        cooldown = Time.time + delay[idx];
    }
    private void Update()
    {
        if (cooldown < Time.time) 
        {
            orderreveal[idx++].SetActive(true);
            cooldown = Time.time + delay[idx % delay.Length];
        }
        if (idx - 6 >= 0) orderreveal[idx - 6].SetActive(false);
    }
}

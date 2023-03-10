using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateMeetingScript : MonoBehaviour
{
    [SerializeField] private float[] delay;
    [SerializeField] private GameObject[] orderreveal;
    [SerializeField, Range(0.1f, 1f)] private float speed = 1f;
    private float cooldown;
    private int idx = 0;
    private void Start()
    {
        foreach (GameObject go in orderreveal) 
        { 
            go.SetActive(false);
            go.GetComponent<Image>().CrossFadeAlpha(0f, 0f, true);
        }
        cooldown = Time.time + delay[idx];
    }
    private void Update()
    {
        if (cooldown < Time.time) 
        {
            orderreveal[idx].SetActive(true);
            orderreveal[idx].GetComponent<Image>().CrossFadeAlpha(1f, speed, false);
            idx = idx++ % delay.Length;
            cooldown = Time.time + delay[idx];
        }

        if (idx - 6 >= 0) orderreveal[idx - 6].SetActive(false);
    }
}

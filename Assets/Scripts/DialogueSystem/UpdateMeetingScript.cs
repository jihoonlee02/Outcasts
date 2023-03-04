using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMeetingScript : MonoBehaviour
{
    [SerializeField] private float[] delay;
    [SerializeField] private GameObject[] orderreveal;
    [SerializeField, Range(0.1f, 1f)] private float speed = 0.1f;
    private float cooldown;
    private int idx = 0;
    private void Start()
    {
        foreach (GameObject go in orderreveal) 
        { 
            go.SetActive(false);
            go.GetComponent<CanvasGroup>().alpha = 0f;
        }
        cooldown = Time.time + delay[idx];
    }
    private void Update()
    {
        if (cooldown < Time.time) 
        {
            orderreveal[idx].SetActive(true);
            StartCoroutine(FadeIn(orderreveal[idx++].GetComponent<CanvasGroup>()));
            cooldown = Time.time + delay[idx];
        }

        if (idx - 6 >= 0) orderreveal[idx - 6].SetActive(false);
    }

    private IEnumerator FadeIn(CanvasGroup image) 
    { 
        while (image.alpha < 1f)
        {
            image.alpha += 0.05f;
            yield return new WaitForSeconds(0.01f * speed);
        }
    }
}

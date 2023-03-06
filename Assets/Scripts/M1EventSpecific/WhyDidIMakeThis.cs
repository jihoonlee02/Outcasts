using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WhyDidIMakeThis : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(something());
    }

    private void OnDisable()
    {
        GetComponent<Image>().enabled = false;
    }

    private IEnumerator something()
    {
        yield return new WaitForSeconds(2.35f);
        Camera.Instance.CamShaker.StartShaking();
        GameManager.Instance.CurrRoomManager.Tinker.Jump();
        GameManager.Instance.CurrRoomManager.Ashe.Jump();
        yield return new WaitForSeconds(0.35f);
        Camera.Instance.CamShaker.StopShaking();
    }
}

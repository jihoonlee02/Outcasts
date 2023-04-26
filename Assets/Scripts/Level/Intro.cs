using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(begin());
    }

    private IEnumerator begin()
    {
        yield return new WaitForSeconds(2f);
        foreach (Transform child in transform)
        {
            if (child.GetComponent<TextProducer>() != null) child.gameObject.SetActive(true);
            yield return new WaitForSeconds(5f);
        }

        GameManager.Instance.LoadToScene("Level1");
        
    }
}

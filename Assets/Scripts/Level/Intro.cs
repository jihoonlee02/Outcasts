using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intro : MonoBehaviour
{
    private float[] seconds = {2f, 5f, 5f, 4f, 3f, 2f };
    private void Start()
    {
        StartCoroutine(begin());
    }

    private IEnumerator begin()
    {
        yield return new WaitForSeconds(2f);
        int i = 0;
        foreach (Transform child in transform)
        {
            if (child.GetComponent<TextProducer>() != null) child.gameObject.SetActive(true);
            yield return new WaitForSeconds(seconds[i++]);
        }

        GameManager.Instance.LoadToScene("Level1");
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    private float offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = Random.Range(0, 3.0f);
        foreach (Transform child in transform) {
            child.GetComponent<LightFluctuator>().SetOffset(offset);
        }
    }
}

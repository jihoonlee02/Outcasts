using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFluctuator : MonoBehaviour
{
    [SerializeField] float baseLight;
    private UnityEngine.Rendering.Universal.Light2D torchLight;
    private float offset;

    // Start is called before the first frame update
    void Start()
    {
        torchLight = GetComponent<UnityEngine.Rendering.Universal.Light2D>();
    }

    // Update is called once per frame
    void Update()
    {
        torchLight.intensity = Mathf.PingPong(offset + Time.time / 4, 0.1f) + baseLight;
    }

    public void SetOffset(float o) {
        offset = o;
    }
}

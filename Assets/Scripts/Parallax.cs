using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float length, startpos;
    [SerializeField] private GameObject m_camera;
    [SerializeField] private float parallaxEffect;

    private void Start()
    {
        startpos = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;

        if (m_camera == null) m_camera = transform.parent.parent.gameObject;
    }

    private void FixedUpdate()
    {
        float temp = (m_camera.transform.position.x * (1 - parallaxEffect));
        float dist = (m_camera.transform.position.x * parallaxEffect);

        transform.position = new Vector3(startpos + dist, transform.position.y, transform.position.z);

        if (temp > startpos + length) startpos += length;
        else if (temp < startpos - length) startpos -= length;
    }
}

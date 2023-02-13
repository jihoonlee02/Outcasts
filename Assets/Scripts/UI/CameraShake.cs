using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Modifiers")]
    [SerializeField, Range(0f,1f)] private float m_strength = 0.1f;
    [SerializeField, Range(0f, 1f)] private float m_radius = 0.125f;
    [SerializeField, Range(0.1f, 1f)] private float m_speed = 1f;

    #region Technical
    private Vector2 savedPosition;
    #endregion

    public void StartShaking()
    {
        savedPosition = transform.position;
        StartCoroutine(Shake());
    }

    public void StartShaking(float strength = 0.25f, float radius = 0.125f, float speed = 1f)
    {
        m_strength = strength;
        m_radius = radius;
        m_speed = speed;
        savedPosition = transform.position;
        StartCoroutine(Shake());
    }

    public void StopShaking()
    {
        StopAllCoroutines();
        transform.position = savedPosition;
    }

    private IEnumerator Shake()
    {
        float dist = 0f;
        float power = 1 / (m_strength * 1000);
        while (true)
        {
            if (Mathf.Abs(dist) >= m_radius) power *= -1;
            dist += power;
            transform.Translate(power, 0f, 0f);
            yield return new WaitForSeconds(1/(m_speed * 1000f));
        }
    }
}

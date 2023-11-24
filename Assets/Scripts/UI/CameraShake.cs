using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [Header("Modifiers")]
    [SerializeField, Range(0f,1f)] private float m_strength = 0.1f;
    [SerializeField, Range(0f, 1f)] private float m_radius = 0.125f;
    [SerializeField, Range(0.1f, 5f)] private float m_speed = 1f;
    
    #region Technical
    private Vector3 savedPosition;
    private bool isShaking = false;
    private float dist;
    private float power;
    public bool IsShaking => isShaking;
    private float coolDown;
    #endregion

    private void FixedUpdate()
    {
        if (isShaking)
        {
            Vector3 offset = new Vector3(Mathf.Sin(Random.value * 10), Mathf.Sin(Random.value * 10), 0) * m_strength;
            transform.position = savedPosition + offset;
        }    
    }
    public void StartShaking()
    {
        isShaking = true;
        savedPosition = transform.position;
        dist = 0f;
        power = m_strength * (1 / 100f);
    }

    public void StartShaking(float strength = 0.1f, float radius = 0.125f, float speed = 1f)
    {
        m_strength = strength;
        m_radius = radius;
        m_speed = speed;
        isShaking = true;
        savedPosition = transform.position;
        dist = 0f;
        power = 1 / (m_strength * 1000);
    }

    public void StartShakingFor(float seconds)
    {
        StartShaking();
        StartCoroutine(ShakeFor(seconds));
    }

    public void StartShakingFor(float seconds, float strength = 0.25f, float radius = 0.125f, float speed = 1f)
    {
        StartShaking(strength, radius, speed);
        StartCoroutine(ShakeFor(seconds));
    }

    public void StopShaking()
    {
        StopAllCoroutines();
        transform.position = savedPosition;
        isShaking = false;
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
            yield return new WaitForSeconds(1/(m_speed * 10000f));
        }
    }

    private IEnumerator ShakeFor(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        StopShaking();
    }
}

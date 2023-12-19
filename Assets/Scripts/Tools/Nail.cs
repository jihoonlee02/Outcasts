using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Nail : Projectile
{
    [SerializeField] private AudioClip m_impactClip;
    private AudioSource m_audioSource;

    private new void Awake()
    {
        base.Awake();
        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.clip = m_impactClip;
    }

    protected override void ImpactHandler(GameObject cogo)
    {
        if (!impacted &&  cogo.gameObject.layer == LayerMask.NameToLayer("Platforms") && cogo.tag != "head")
        {
            m_rb.velocity = Vector3.zero;
            //m_audioSource.Play();
            transform.SetParent(cogo.transform, true);
            impacted = true;
        }
    }
}

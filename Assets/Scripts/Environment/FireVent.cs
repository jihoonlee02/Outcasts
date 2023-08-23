using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireVent : Invokee
{
    [Header("Fire Vent Specific")]
    [SerializeField]
    private Collider2D m_flameCollider;
    [SerializeField, Tooltip("Auto turns flames on an off")] 
    private bool m_autoFlame = true;
    [SerializeField, Tooltip("Used when Auto Flame active"), Range(0.5f, 10f)] 
    private float activeDuration = 3f;
    [SerializeField, Tooltip("Used when Auto Flame active"), Range(0.5f, 10f)] 
    private float inactiveDuration = 3f;

    #region Technical
    private float durationSwitch = 0f;
    #endregion

    private void Update()
    {
        if (Time.time >= durationSwitch)
        {
            // Purposely Not Using built in Delay
            if (!m_flameCollider.enabled)
            {
                OnActivate(); 
            }
            else
            {
                OnDeactivate();
            }
        }
    }

    protected override void OnActivate()
    {
        /* DEV TEMP*/
        m_flameCollider.gameObject.SetActive(true);
        /**/
        m_flameCollider.enabled = true;
        durationSwitch = Time.time + activeDuration;
    }

    protected override void OnDeactivate()
    {
        /* DEV TEMP*/
        m_flameCollider.gameObject.SetActive(false);
        /**/
        m_flameCollider.enabled = false;
        durationSwitch = Time.time + inactiveDuration;
    }
}

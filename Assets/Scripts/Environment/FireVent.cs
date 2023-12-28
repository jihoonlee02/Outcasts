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
    [SerializeField] private bool flameRise = false;
    [SerializeField, Tooltip("Used when Auto Flame active"), Range(0.5f, 10f)] 
    private float activeDuration = 3f;
    [SerializeField, Tooltip("Used when Auto Flame active"), Range(0.5f, 10f)] 
    private float inactiveDuration = 3f;
    [SerializeField] private float yFlameMax;
    [SerializeField] private float yFlameMin = -4.246f;
    [SerializeField] private float speed = 3f;
    
    private Fire m_associatedFire;
    #region Technical
    private float durationSwitch = 0f;
    
    #endregion
    private new void Start()
    {
        base.Start();
        m_associatedFire = GetComponentInChildren<Fire>();
        durationSwitch = Time.time + delay; 
    }
    private void Update()
    {
        if (m_autoFlame && Time.time >= durationSwitch)
        {
            flameRise = !flameRise;
            durationSwitch = Time.time + (flameRise ? activeDuration : inactiveDuration);
        }

        m_flameCollider.enabled = flameRise;
    }
    private void FixedUpdate()
    {
        m_associatedFire.transform.localPosition = new Vector2(m_associatedFire.transform.localPosition.x, Mathf.Lerp(m_associatedFire.transform.localPosition.y, flameRise ? yFlameMax : yFlameMin, Time.deltaTime * speed));
    }

    protected override void OnActivate()
    {
        flameRise = !flameRise;
        durationSwitch = Time.time + (flameRise ? activeDuration : inactiveDuration);
    }

    protected override void OnDeactivate()
    {
        flameRise = !flameRise;
        durationSwitch = Time.time + (flameRise ? activeDuration : inactiveDuration);
    }

    // Very Permeant Disabbling
    public void DisableVent()
    {
        m_autoFlame = false;
        flameRise = false;
    }
    public void RaiseFlame()
    {
        flameRise = true;
        durationSwitch = Time.time + activeDuration;
    }
    public void DropFlame()
    {
        flameRise = false;
        durationSwitch = Time.time + inactiveDuration;
    }
}

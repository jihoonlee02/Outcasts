using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
[RequireComponent(typeof(TextMeshProUGUI))]
public abstract class TextEffect : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_targetTMP;
    [SerializeField] private bool m_findTMPOnStart;

    protected void Start()
    {
        if (m_findTMPOnStart) m_targetTMP = GetComponentInChildren<TextMeshProUGUI>();
    }

    protected void Update()
    {
        
    }

    protected abstract void EffectStart();
    protected abstract void EffectUpdate();
    protected abstract void EffectFinish();

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : Grabbable
{
    [SerializeField] private bool m_isHeavy;

    public bool IsHeavy => m_isHeavy;
}

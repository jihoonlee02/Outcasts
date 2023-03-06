using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AshePawn : Pawn
{
    [SerializeField] private Tool m_gauntletReference;
    [SerializeField] private Collider2D m_liftingRegion; 
    private bool m_isLifting;
    public bool IsLifting
    {
        get { return m_isLifting;}
        set { m_isLifting = value;}
    }
    protected void Start()
    {
        base.Start();
        CurrentState = m_states.AsheDefaultState();
    }
    protected void Update()
    {
        base.Update();
    }
    public override void PrimaryAction()
    {
        m_gauntletReference.UsePrimaryAction();
    }

    public override void SecondaryAction()
    {
        m_gauntletReference.UseSecondaryAction();
    }
}

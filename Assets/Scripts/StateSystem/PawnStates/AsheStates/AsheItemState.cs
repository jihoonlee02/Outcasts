using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheItemState : State
{
    public AsheItemState(Pawn context, PawnStateFactory factory) : base(context, factory)
    {
        m_isRootState = true;
        m_animationName = "_holdingItem";
        InitializeSubState();
    }
    public override void EnterState()
    {
        m_context.HeldItem.SetActive(true);
        ((AshePawn)m_context).DisablePunch();
        ((AshePawn)m_context).DisableGrab();
        ((AshePawn)m_context).DisableLiftingRegion();
    }
    public override void ExitState()
    {
        m_context.HeldItem.SetActive(false);
        ((AshePawn)m_context).EnablePunch();
        ((AshePawn)m_context).EnableGrab();
        ((AshePawn)m_context).EnableLiftingRegion();
    }
    public override void InitializeSubState()
    {
        if (!m_context.IsGrounded)
        {
            SetSubState(m_factory.Falling());
        }
        else if (m_context.IsJumping)
        {
            SetSubState(m_factory.Jumping());
        }
        else if (m_context.IsMoving)
        {
            SetSubState(m_factory.Moving());
        }
        else
        {
            SetSubState(m_factory.Grounded());
        }

        m_subState.EnterState();
    }

    public override void CheckSwitchState()
    {
        if (!m_context.IsHoldingItem)
        {
            SwitchState(m_factory.AsheDefaultState());
        }
    }
}

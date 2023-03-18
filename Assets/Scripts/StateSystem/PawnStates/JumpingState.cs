using UnityEngine;

public class JumpingState : State
{
    public JumpingState(Pawn context, PawnStateFactory factory) : base(context, factory) 
    {
        m_animationName = "Jump";
    }
    public override void EnterState()
    {
        //Debug.Log("Enter Jumping State");
        m_context.AudioSource.loop = false;
        m_context.AudioSource.pitch = 1f;
        m_context.AudioSource.clip = m_context.Data.Jump;
        m_context.Animator.Play(m_animationName + m_superState.AnimationName);
        m_context.AudioSource.Play();
    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        //CheckSwitchState();
    }

    public override void CheckSwitchState()
    {
        if (!m_context.IsJumping) // On the assumption it is not grounded
        {
            SwitchState(m_factory.Falling());
        }
    }
}

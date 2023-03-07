using UnityEngine;

public class JumpingState : State
{
    public JumpingState(Pawn context, PawnStateFactory factory) : base(context, factory) { }
    public override void EnterState()
    {
        Debug.Log("Enter Jumping State");
        m_context.AudioSource.loop = false;
        m_context.AudioSource.pitch = 1.05f;
        m_context.AudioSource.clip = m_context.Data.Jump;
        m_context.Animator.Play("Jump");
        m_context.AudioSource.Play();
    }

    public override void ExitState()
    {
        m_context.AudioSource.pitch = 1f;
    }

    public override void UpdateState()
    {
        
    }

    public override void CheckSwitchState()
    {
        if (!m_context.IsJumping) // On the assumption it is not grounded
        {
            SwitchState(m_factory.Falling());
        }
    }
}

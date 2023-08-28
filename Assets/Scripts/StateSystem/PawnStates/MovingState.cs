using UnityEngine;
public class MovingState : State
{
    public MovingState(Pawn context, PawnStateFactory factory) : base(context, factory) 
    {
        m_animationName = "Movement";
    }
    public override void EnterState()
    {
        m_context.AudioSource.clip = m_context.Data.Footstep;
        m_context.AudioSource.loop = true;
        m_context.Animator.Play(m_animationName + m_superState.AnimationName);
        m_context.AudioSource.Play();
    }
    float speed, pitchCalc, m_amplifier = 3f;
    public override void UpdateState()
    {
        //Movement and Animation Calculations
        speed = Mathf.Abs(m_context.RB.velocity.x / m_amplifier);
        m_context.Animator.speed = speed;
        pitchCalc = m_context.Data.MinPitch + speed;
        m_context.AudioSource.pitch = pitchCalc > m_context.Data.MaxPitch ? 
            m_context.Data.MaxPitch : pitchCalc < m_context.Data.MinPitch ? 
            m_context.Data.MinPitch : pitchCalc;
    }
    public override void ExitState()
    {
        m_context.Animator.speed = 1;
        m_context.AudioSource.loop = false;
        m_context.AudioSource.pitch = 1;
        m_context.AudioSource.Stop();
    }

    public override void CheckSwitchState()
    {
        if (m_context.IsGrounded && !m_context.IsMoving)
        {
            SwitchState(m_factory.Grounded());
        }
        else if (m_context.IsJumping)
        {
            SwitchState(m_factory.Jumping());
        }
        else if (!m_context.IsGrounded)
        {
            SwitchState(m_factory.Falling());
        }
    }
}

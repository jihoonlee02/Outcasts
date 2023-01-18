using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gauntlet : Tool
{
    private Animator m_animator;

    public override void UsePrimaryAction()
    {
        //m_animator.SetFloat("MoveX", Mathf.Sign(m_user.Animator.GetFloat("MoveX")));
        //m_user.GetComponent<PlayerController>().DisableMovement();
        m_user.Animator.Play("Punch");
    }


}

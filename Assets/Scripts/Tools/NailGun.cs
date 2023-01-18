using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailGun : Tool
{
    [SerializeField] private Projectile projectile;

    public override void UsePrimaryAction()
    {
        if (m_user.Animator.GetFloat("MoveY") > 0.8f)
        {
            Pooler.Instance.Fire(ProjectileType.Nail,
                m_user.transform.position, Vector2.up);
            return;
        }

        Pooler.Instance.Fire(ProjectileType.Nail, 
            m_user.transform.position, Mathf.Sign(m_user.Animator.GetFloat("MoveX")) * Vector2.right);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailGun : Tool
{
    [SerializeField] private Projectile projectile;

    private void Awake()
    {
        m_data = new ToolData();
        m_data.toolName = "NailGun";
    }

    public override void Use()
    {
        base.Use();
        Pooler.Instance.Fire(ProjectileType.Nail, 
            m_user.transform.position, m_user.Animator.GetFloat("MoveX") > 0.1 ? Vector2.right : Vector2.left);
    }
}

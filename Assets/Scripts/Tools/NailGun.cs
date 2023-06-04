using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NailGun : Tool
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private bool directional;
    private AudioSource m_audioSource;
    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.clip = m_user.Data.ScratchPadSounds[0];
    }

    public void UsePrimaryAction(Vector2 inputVector)
    {
        m_audioSource.Play();

        if (directional)
        {
            //Shoot Up or Down
            if (Mathf.Abs(inputVector.y) > 0.9)
            {
                Pooler.Instance.Fire(ProjectileType.Nail,
                    m_user.transform.position, Mathf.Sign(inputVector.y) * Vector2.up);
                return;
            }

            //Shoot Up-Right Diagonally
            if (inputVector.y >= 0.45f && (inputVector.x >= 0.1f))
            {
                Pooler.Instance.Fire(ProjectileType.Nail,
                    m_user.transform.position, new Vector2(0.5f, 0.5f));
                return;
            }

            //Shoot Up-Left Diagonally
            if (inputVector.y >= 0.45f && inputVector.x <= -0.1f)
            {
                Pooler.Instance.Fire(ProjectileType.Nail,
                    m_user.transform.position, new Vector2(-0.5f, 0.5f));
                return;
            }

            //Shoot Down-Right Diagonally
            if (inputVector.y <= -0.45f && inputVector.x >= 0.1f)
            {
                Pooler.Instance.Fire(ProjectileType.Nail,
                    m_user.transform.position, new Vector2(0.5f, -0.5f));
                return;
            }

            //Shoot Down-Left Diagonally
            if (inputVector.y <= -0.45f && inputVector.x <= -0.1f)
            {
                Pooler.Instance.Fire(ProjectileType.Nail,
                    m_user.transform.position, new Vector2(-0.5f, -0.5f));
                return;
            }
        }       

        //Shoot left or right
        Pooler.Instance.Fire(ProjectileType.Nail, 
            m_user.transform.position, Mathf.Sign(m_user.Animator.GetFloat("MoveX")) * Vector2.right);
    }

    public override void UseSecondaryAction()
    {
        m_user.ToggleMovement();
        m_user.ToggleJump();
    }
}

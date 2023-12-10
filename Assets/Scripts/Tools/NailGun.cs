using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class NailGun : Tool
{
    [SerializeField] private Projectile projectile;
    [SerializeField] private bool directional;
    [SerializeField] private int maxAmmo;
    private AudioSource m_audioSource;
    private int currAmmo;

    public AudioSource AudioSource => m_audioSource;
    private Vector2 m_firPos;

    private void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
        m_audioSource.clip = m_user.Data.ScratchPadSounds[0];
        m_firPos = transform.localPosition;
    }

    // Shoot the Nail Gun
    public void UsePrimaryAction(Vector2 inputVector)
    {
        m_audioSource.Play();

        // Depricated and prolly won't get added
        //if (directional)
        //{
        //    //Shoot Up or Down
        //    if (Mathf.Abs(inputVector.y) > 0.9)
        //    {
        //        Pooler.Instance.Fire(ProjectileType.Nail,
        //            m_user.transform.position, Mathf.Sign(inputVector.y) * Vector2.up);
        //        return;
        //    }

        //    //Shoot Up-Right Diagonally
        //    if (inputVector.y >= 0.45f && (inputVector.x >= 0.1f))
        //    {
        //        Pooler.Instance.Fire(ProjectileType.Nail,
        //            m_user.transform.position, new Vector2(0.5f, 0.5f));
        //        return;
        //    }

        //    //Shoot Up-Left Diagonally
        //    if (inputVector.y >= 0.45f && inputVector.x <= -0.1f)
        //    {
        //        Pooler.Instance.Fire(ProjectileType.Nail,
        //            m_user.transform.position, new Vector2(-0.5f, 0.5f));
        //        return;
        //    }

        //    //Shoot Down-Right Diagonally
        //    if (inputVector.y <= -0.45f && inputVector.x >= 0.1f)
        //    {
        //        Pooler.Instance.Fire(ProjectileType.Nail,
        //            m_user.transform.position, new Vector2(0.5f, -0.5f));
        //        return;
        //    }

        //    //Shoot Down-Left Diagonally
        //    if (inputVector.y <= -0.45f && inputVector.x <= -0.1f)
        //    {
        //        Pooler.Instance.Fire(ProjectileType.Nail,
        //            m_user.transform.position, new Vector2(-0.5f, -0.5f));
        //        return;
        //    }
        //}       

        //Shoot left or right
        // Using new Vector3 feels very inefficent for something used a lot
        var sign = Mathf.Sign(m_user.Animator.GetFloat("MoveX"));
        // YES I KNOW THIS IS HARDCODED WTF YOU WANT ME TO DO??
        transform.localPosition = new Vector3(m_firPos.x * sign, isEntered("Falling_gun") ? 0.1938f : m_firPos.y, transform.localPosition.z);
        Pooler.Instance.Fire(ProjectileType.Nail,
            transform.position, sign * Vector2.right);
    }

    private bool isEntered(string stateName)
    {
        return m_user.Animator.GetCurrentAnimatorStateInfo(0).IsName(stateName);
    }

    // Reload the Nail Gun
    public override void UseSecondaryAction()
    {
        //Pooler.Instance.
    }
}

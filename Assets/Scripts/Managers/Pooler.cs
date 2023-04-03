using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pooler : MonoBehaviour
{
    #region Singleton
    private static Pooler m_instance;
    public static Pooler Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<Pooler>();
            }
            return m_instance;
        }
    }
    #endregion

    private int nailCount = 0;

    private Projectile[] nails;

    [SerializeField] private Projectile nailFab;

    private void Awake()
    {
        nails = new Projectile[(int)ProjectileType.Nail];
        for (int i = 0; i < (int)ProjectileType.Nail; i++) 
        {
            nails[i] = Instantiate(nailFab);
            nails[i].transform.SetParent(transform, false);
            nails[i].gameObject.SetActive(false);
        }        
    }

    public void Fire(ProjectileType type, Vector2 spawnPos, Vector2 direction)
    {
        switch (type)
        {
            case ProjectileType.Nail:
                nails[nailCount].gameObject.SetActive(true);
                nails[nailCount].Fire(spawnPos, direction);
                nailCount = (nailCount + 1) % (int)ProjectileType.Nail;
                break;
        }
    }
}

public enum ProjectileType
{
    Nail = 4
}

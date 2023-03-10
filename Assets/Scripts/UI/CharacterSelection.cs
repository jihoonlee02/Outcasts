using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button select_tinker;
    [SerializeField] private UnityEngine.UI.Button select_ashe;

    public UnityEngine.UI.Button Select_Tinker => select_tinker;
    public UnityEngine.UI.Button Select_Ashe => select_ashe;

    public void Display()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    
}

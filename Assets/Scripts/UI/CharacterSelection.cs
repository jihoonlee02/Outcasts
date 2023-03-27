using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private UnityEngine.UI.Button select_tinker;
    [SerializeField] private UnityEngine.UI.Button select_ashe;
    [SerializeField] private TextMeshProUGUI title;

    public UnityEngine.UI.Button Select_Tinker => select_tinker;
    public UnityEngine.UI.Button Select_Ashe => select_ashe;

    public void Display(bool right, int playerNum)
    {
        if (right)
        {
            gameObject.transform.localPosition = new Vector3(-transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        }
        title.text = "Player " + playerNum + " Selection";
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
    
    
}

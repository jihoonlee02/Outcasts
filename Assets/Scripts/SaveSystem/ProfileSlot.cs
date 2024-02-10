using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ProfileSlot : MonoBehaviour
{
    [Header("UIComponent Details")]
    [SerializeField] private TextMeshProUGUI m_profileNameDisplay;
    [SerializeField] private TextMeshProUGUI m_sceneNameDisplay;
    [SerializeField] private TextMeshProUGUI m_scrollCountDisplay;

    private SaveData m_profiledata;
    public SaveData ProfileDat => m_profiledata;
    public void AllocateSlotDetails(SaveData data)
    {
        m_profiledata = data;

        // Update UI
        UpdateSlotUI(data);
    }

    public void AllocateEmptySlot(SaveData data)
    {
        m_profiledata = data;

        // Update UI
        m_profileNameDisplay.text = "Profile: " + data.ProfileName;
    }

    public void UpdateSlotUI(SaveData data)
    {
        if (data.ProfileName != m_profiledata.ProfileName) return;

        // All UI Stuff
        m_profileNameDisplay.text = "Profile: " + data.ProfileName;
        m_sceneNameDisplay.text = "Scene: " + data.CurrScene;
        m_scrollCountDisplay.text = "Scrolls: " + data.ChestsCount; // Change this to icons of scrolls
    }

}

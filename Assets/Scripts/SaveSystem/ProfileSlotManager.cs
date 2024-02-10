using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileSlotManager : MonoBehaviour
{
    #region Singleton
    private static ProfileSlotManager instance;
    public static ProfileSlotManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ProfileSlotManager>();
            }
            return instance;
        }
    }
    #endregion 
    [SerializeField] private GameObject m_profileSlotRef;
    [SerializeField] private Transform m_viewPort;
    private event Action<SaveData> m_onSaveDataChange;
    public Action<SaveData> OnSaveDataChange
    {
        get => m_onSaveDataChange;
        set
        {
            m_onSaveDataChange = value;
        }
    }
    private void Awake()
    {
        FileManagment.GetProfileFileNames();
    }
    public void CreateNewProfileSlot(string profileName)
    {
        if (FileManagment.LoadFromSaveFile(profileName, out var contents))
        {
            return;
        }

        SaveData sd = new SaveData();
        
        // Initialize SaveData
        sd.ProfileName = profileName;
        sd.CurrScene = "Intro";
        FileManagment.WriteToSaveFile(profileName, sd.ToJson());

        // Create UI
        ProfileSlot newProfileSlot = Instantiate(m_profileSlotRef).GetComponent<ProfileSlot>();
        newProfileSlot.AllocateEmptySlot(sd);
        newProfileSlot.transform.SetParent(m_viewPort, false);
    }

    public void InitializeProfileSlots()
    {
        List<string> jsonContents = FileManagment.GetProfileContents();
        foreach (string json in jsonContents)
        {
            SaveData sd = new SaveData();
            sd.LoadFromJson(json);
            ProfileSlot newProfileSlot = Instantiate(m_profileSlotRef).GetComponent<ProfileSlot>();
            newProfileSlot.AllocateSlotDetails(sd);
            newProfileSlot.transform.SetParent(m_viewPort, false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GemTracker : MonoBehaviour
{
    #region Singleton
    private static GemTracker m_instance;
    public static GemTracker Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<GemTracker>();
            }
            return m_instance;
        }
    }
    #endregion
    [Header("UIComponents")]
    [SerializeField] private TextMeshProUGUI m_tinkerGemTracking;
    [SerializeField] private TextMeshProUGUI m_asheGemTracking;
    // Gem Data
    private int m_tinkerGems = 0;
    private int m_asheGems = 0;

    public int TinkerGemsCollected => m_tinkerGems;
    public int AsheGemsCollected => m_asheGems;

    private int m_savedTinkerGems = 0;
    private int m_savedAsheGems = 0;

    public void ResetGemCollectionToLastSave()
    {
        m_asheGems = m_savedAsheGems;
        m_tinkerGems = m_savedTinkerGems;
    }
    public void SaveRecentGemCollection()
    {
        m_savedAsheGems = m_asheGems;
        m_savedTinkerGems = m_tinkerGems;
    }
    public void TinkerCollectsGem()
    {
        m_tinkerGems++;
        m_tinkerGemTracking.text = m_tinkerGems.ToString();
    }
    public void AsheCollectsGem()
    {
        m_asheGems++;
        m_asheGemTracking.text = m_asheGems.ToString();
    }
    public void InitializeGemCount(int a_tinkerGems, int a_asheGems)
    {
        m_tinkerGems = a_tinkerGems;
        m_asheGems = a_asheGems;
        m_tinkerGemTracking.text = m_tinkerGems.ToString();
        m_asheGemTracking.text = m_asheGems.ToString();
    }
    public void ClearGemCount()
    {
        m_tinkerGems = 0;
        m_asheGems = 0;
        m_tinkerGemTracking.text = m_tinkerGems.ToString();
        m_asheGemTracking.text = m_asheGems.ToString();
    }
}

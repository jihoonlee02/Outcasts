using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChestTracker : MonoBehaviour
{
    #region Singleton
    private static ChestTracker instance;
    public static ChestTracker Instance
    {
        get 
        { 
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<ChestTracker>();
            }
            return instance;
        }
    }
    #endregion
    [SerializeField] private int maxNumberOfChests;
    [SerializeField] private int requiredChests;
    [SerializeField] private int currNumberOfChestOpened;
    [SerializeField] private TextMeshProUGUI m_trackerTextBox;
    private bool[] m_foundChests;
    public bool IsAllChestsOpen => (currNumberOfChestOpened / requiredChests) >= 1;
    public bool[] FoundChests => m_foundChests;
    public int NumberOfChestsOpened => currNumberOfChestOpened;

    private Animator m_animator;

    private bool[] m_savedFoundChests;
    private int m_savedCurrNumberofChestOpened;
    private void Awake()
    {
        m_foundChests = new bool[maxNumberOfChests];
        m_animator = GetComponent<Animator>();
        m_trackerTextBox.text = currNumberOfChestOpened.ToString() + "/" + requiredChests.ToString();
    }
    public void ResetChestCollectionToLastSave()
    {
        m_foundChests = m_savedFoundChests;
        currNumberOfChestOpened = m_savedCurrNumberofChestOpened;
    }
    public void SaveRecentChestCollection()
    {
        m_savedFoundChests = m_foundChests;
        m_savedCurrNumberofChestOpened = currNumberOfChestOpened;
    }
    public void SetFoundChests(bool[] a_foundChests, int a_chestsOpenedCount)
    {
        if (a_foundChests == null) Debug.LogError("Can't send Null Chest's Array");
        ResetChestCount();
        for (int i = 0; i < m_foundChests.Length; i++)
        {
            m_foundChests[i] = a_foundChests[i];
        }
        currNumberOfChestOpened = a_chestsOpenedCount;
    }
    public void FoundNewChest(int idx)
    {
        if (m_foundChests[idx]) return;
        m_foundChests[idx] = true;
        currNumberOfChestOpened++;
        StartCoroutine(UpdateChestTracker());
    }
    public bool IsChestFound(int idx)
    {
        if (idx >= m_foundChests.Length) return false;
        return m_foundChests[idx];
    }
    public void ResetChestCount()
    {
        m_foundChests = new bool[maxNumberOfChests];
        currNumberOfChestOpened = 0;
        m_trackerTextBox.color = Color.white;
    }
    private IEnumerator UpdateChestTracker()
    {
        m_animator.Play("ShowChestTracker");
        yield return new WaitForSeconds(2.3f);
        
        if (IsAllChestsOpen)
        {
            m_trackerTextBox.color = Color.green;
            m_trackerTextBox.text = currNumberOfChestOpened.ToString();
        }
        else
        {
            m_trackerTextBox.text = currNumberOfChestOpened.ToString() + "/" + requiredChests.ToString();
        }
        // Hackey As Fucc Bruh
        yield return new WaitForSeconds(1.5f);
        m_animator.Play("HideChestTracker");
    }
}

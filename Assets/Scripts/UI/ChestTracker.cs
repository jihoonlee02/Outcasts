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

    private Animator m_animator;
    private void Awake()
    {
        m_foundChests = new bool[maxNumberOfChests];
        m_animator = GetComponent<Animator>();
        m_trackerTextBox.text = currNumberOfChestOpened.ToString() + "/" + requiredChests.ToString();
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
        yield return new WaitForSeconds(1.5f);
        m_animator.Play("HideChestTracker");
    }
}

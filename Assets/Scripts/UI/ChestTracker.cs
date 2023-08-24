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
    [SerializeField] private int currNumberOfChestOpened;
    [SerializeField] private TextMeshProUGUI m_trackerTextBox;
    private bool[] m_foundChests;

    private Animator m_animator;
    private void Awake()
    {
        m_foundChests = new bool[maxNumberOfChests];
        m_animator = GetComponent<Animator>();
        m_trackerTextBox.text = currNumberOfChestOpened.ToString() + "/" + maxNumberOfChests.ToString();
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
    }
    private IEnumerator UpdateChestTracker()
    {
        m_animator.Play("ShowChestTracker");
        yield return new WaitForSeconds(1.5f);
        m_trackerTextBox.text = currNumberOfChestOpened.ToString() + "/" + maxNumberOfChests.ToString();
        yield return new WaitForSeconds(1.5f);
        m_animator.Play("HideChestTracker");
    }
}

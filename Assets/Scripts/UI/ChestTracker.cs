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
    [SerializeField] private float maxNumberOfChests;
    [SerializeField] private float currNumberOfChestOpened;
    [SerializeField] private TextMeshProUGUI m_trackerTextBox;

    private Animator m_animator;

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        m_trackerTextBox.text = currNumberOfChestOpened.ToString() + "/" + maxNumberOfChests.ToString();
    }

    public void FoundNewChest()
    {
        currNumberOfChestOpened++;
        StartCoroutine(UpdateChestTracker());
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

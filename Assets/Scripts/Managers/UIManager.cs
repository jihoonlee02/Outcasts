using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager instance;
    public static UIManager Instance
    {
        get 
        {
            if (instance == null)
            {
                instance = FindObjectOfType<UIManager>();
                if (instance == null)
                {
                    Debug.LogError("UIManager Prefab is required in Game!");
                }
            }
            return instance; 
        }
    }
    #endregion

    [SerializeField] private EventSystem m_eventSystem;
    [SerializeField] private InputSystemUIInputModule m_inputSystemUIInputModule;

    [Header("Pause Components")]
    [SerializeField] private GameObject m_pauseMenu;
    [SerializeField] private GameObject m_pauseButt;
    [SerializeField] private TextMeshProUGUI m_playerPaused;
    public EventSystem EventSystem => m_eventSystem;

    private void Start()
    {
        if (m_eventSystem == null) m_eventSystem = GetComponentInChildren<EventSystem>();
        if (m_inputSystemUIInputModule == null) m_inputSystemUIInputModule = GetComponent<InputSystemUIInputModule>();
    }

    public void OpenPauseMenu(PlayerController pc = null)
    {
        m_pauseMenu.SetActive(true);
        m_eventSystem.SetSelectedGameObject(m_pauseButt);
        if (pc != null)
        {
            m_inputSystemUIInputModule.actionsAsset = pc.PlayerInput.actions;
            m_playerPaused.text = pc.ControlledPawn.Data.Name + " Paused"; 
        }
    }

    public void ClosePauseMenu()
    {
        m_pauseMenu.SetActive(false);
    }

}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region Singleton
    private static MainMenu m_instance;
    public static MainMenu Instance
    {
        get 
        { 
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<MainMenu>();
            }
            return m_instance; 
        }
    }
    #endregion
    [SerializeField] private DoorTransition m_doorTransition;
    [SerializeField] private Animator m_menuLayoutAnimator;

    [Header("Menu Buttons")]
    [SerializeField] private GameObject m_optionsButton;
    [SerializeField] private GameObject m_startGameButton;

    private TreeNode<string> m_menuNode;
    private TreeNode<string> m_optionsNode;
    private TreeNode<string> m_startGameNode;
    private TreeNode<string> m_currNode;
    private InputActionAsset m_iaa;
    private EventSystem m_eventSystem;

    private void Start()
    {
        m_doorTransition.OpenDoors();
        m_menuNode = new TreeNode<string>("MainMenu");
        m_optionsNode = new TreeNode<string>("Options");
        m_startGameNode = new TreeNode<string>("StartGame");
        m_menuNode.AddChildren(new TreeNode<string>[]{m_optionsNode, m_startGameNode});
        m_currNode = m_menuNode;

        m_iaa = GetComponent<InputSystemUIInputModule>().actionsAsset;
        m_eventSystem = GetComponent<EventSystem>();
        m_iaa.FindAction("Cancel").performed += BackAPanel;
    }

    public void StartGame()
    {
        m_doorTransition.CloseDoors();
        StartCoroutine(DelaySceneLoad(2f));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private IEnumerator DelaySceneLoad(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        SceneManager.LoadSceneAsync("Setup");
    }

    #region View Methods

    public void ViewOptions()
    {
        m_menuLayoutAnimator.Play("Options");
        m_currNode = m_optionsNode;
    }

    public void ViewStartGame()
    {
        m_menuLayoutAnimator.Play("StartGame");
        m_currNode = m_startGameNode;
    }

    public void ViewMainMenu()
    {
        m_menuLayoutAnimator.SetTrigger("MainMenu");
        m_currNode = m_menuNode;
    }

    // Inefficent, yet does the job
    public void BackAPanel(InputAction.CallbackContext context)
    {
        if (m_currNode.Data == "Options")
        {
            m_eventSystem.SetSelectedGameObject(m_optionsButton);
        }
        else if (m_currNode.Data == "StartGame")
        {
            m_eventSystem.SetSelectedGameObject(m_startGameButton);
        }
        
        if (m_currNode.Parent != null && m_currNode.Parent.Data == "MainMenu")
        {
            ViewMainMenu();
        }
    }

    #endregion
}

//public class GoBackPanel : ICancelHandler
//{
//    public void OnCancel(BaseEventData eventData)
//    {
//        Debug.Log("I Did Cancel");
//        MainMenu.Instance.BackAPanel();
//    }
//}

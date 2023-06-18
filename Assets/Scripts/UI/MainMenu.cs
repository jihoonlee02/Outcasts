using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private DoorTransition m_doorTransition;
    [SerializeField] private Animator m_menuLayoutAnimator;

    private void Start()
    {
        m_doorTransition.OpenDoors();
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
    }

    public void ViewStartGame()
    {
        m_menuLayoutAnimator.Play("StartGame");
    }

    public void ViewMainMenu()
    {
        m_menuLayoutAnimator.SetTrigger("MainMenu");
    }

    #endregion
}

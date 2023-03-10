using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private DoorTransition m_doorTransition;

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
        SceneManager.LoadSceneAsync("Hub");
    }
}

 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    [Header("Level Attributes")]
    [SerializeField] private ExitDoor m_tinkerExitDoor;
    [SerializeField] private ExitDoor m_asheExitDoor;
    [SerializeField] private Transform m_tinkerSpawn;
    [SerializeField] private Transform m_asheSpawn;
    [SerializeField] private AudioClip m_music;
    [SerializeField] private bool useSetupDefault = false;
    [SerializeField] private string m_nextScene;

    [Header("Level Dialogue Special")]
    [SerializeField] private DialogueProducer[] m_externalDialogues;
    public DialogueProducer[] ExternalDialogues => m_externalDialogues;

    [Header("Dev Details")]
    [SerializeField] private bool isSetupScene = false;
    [SerializeField] private UnityEvent invokeAtStart;
    [SerializeField] private GameObject levelThings;

    private bool exited = false;
    private void Start() {
        //if (GameObject.Find("LevelThings") == null
        //    && GameObject.Find("LevelThings SoloController Variant") == null && !isSetupScene)
        //{
        //    Instantiate(levelThings);
        //}
        GameManager.Instance.LevelManager = this;
        GameManager.Instance.Tinker.transform.position = m_tinkerSpawn.position;
        GameManager.Instance.Ashe.transform.position = m_asheSpawn.position;
        GameManager.Instance.Tinker.gameObject.SetActive(true);
        GameManager.Instance.Ashe.gameObject.SetActive(true);

        if (AudioManager.Instance.CurrentAudio != m_music)
            AudioManager.Instance.SetAudioClip(m_music);

        m_tinkerSpawn.gameObject.SetActive(false);
        m_asheSpawn.gameObject.SetActive(false);

        GameManager.Instance.DoorTransition.OpenDoors();
        if (isSetupScene)
        {
            return;
        }
        AudioManager.Instance.PlayAudio();
        invokeAtStart.Invoke();
    }

    private void Update() 
    {
        // Though this is in the update method, it should only get invoked once...
        // ...hopefully
        if (!exited && m_asheExitDoor != null && m_tinkerExitDoor != null 
            && m_asheExitDoor.OnDoor && m_tinkerExitDoor.OnDoor) OnLevelExit();
    }

    public void OnLevelExit()
    {
        // So that Tinker n' Ashe don't start invoking this a billion times
        GameManager.Instance.Tinker.gameObject.SetActive(false);
        GameManager.Instance.Ashe.gameObject.SetActive(false);

        //DialogueManager.Instance.DisplayDialogue();

        //This shouldn't be done immeditly
        if (useSetupDefault) GameManager.Instance.TransitionToNextScene();
        else GameManager.Instance.LoadToScene(m_nextScene);
        exited = true;

    }
    public void PausePawnControl()
    {
        GameManager.Instance.ActivateCinematic();
        GameManager.Instance.AshePC?.DisablePawnControl();
        GameManager.Instance.TinkerPC?.DisablePawnControl();
        GameManager.Instance.SC?.DisablePawnControl();
    }
    public void ResumePawnControl()
    {
        GameManager.Instance.DeactivateCinematic();
        GameManager.Instance.AshePC?.EnablePawnControl();
        GameManager.Instance.TinkerPC?.EnablePawnControl();
        GameManager.Instance.SC?.EnablePawnControl();    
    }
    public void FaceTinkerRight()
    {
        GameManager.Instance.Tinker.Animator.SetFloat("MoveX", 1);
    }
    public void FaceAsheRight()
    {
        GameManager.Instance.Ashe.Animator.SetFloat("MoveX", 1);
    }
    public void FaceTinkerLeft()
    {
        GameManager.Instance.Tinker.Animator.SetFloat("MoveX", -1);
    }
    public void FaceAsheLeft()
    {
        GameManager.Instance.Ashe.Animator.SetFloat("MoveX", -1);
    }
    public void AsheJump(float force)
    {
        GameManager.Instance.Ashe.Jump(force);
    }
    public void TinkerJump(float force)
    {
        GameManager.Instance.Tinker.Jump(force);
    }
    public void ReloadLevel()
    {
        GameManager.Instance.ReloadCurrentScene();
    }
    public void StopMusic()
    {
        AudioManager.Instance.StopAudio();
    }
    public void PauseMusic()
    {
        AudioManager.Instance.PauseAudio();
    }
    public void PlayMusic()
    {
        AudioManager.Instance.PlayAudio();
    }
    public void ChangeMusic(AudioClip clip)
    {
        AudioManager.Instance.SetAudioClip(clip);
    }
    public void ChangeMusicVolume(float volume)
    {
        AudioManager.Instance.SetVolume(volume);
    }
    public void ChangeNextScene(string nextScene)
    {
        m_nextScene = nextScene;
    }
    public void DisplayOnScreenMessage(string text)
    {
        GameManager.Instance.DisplayOnScreenMessage(text);
    }
    public void AlertTinker(int alertType)
    {
        GameManager.Instance.Tinker.EnableAlert((AlertType)alertType);
    }
    public void DisableAlertTinker(int alertType)
    {
        GameManager.Instance.Tinker.DisableAlert((AlertType)alertType);
    }
    public void AlertAshe(int alertType)
    {
        GameManager.Instance.Ashe.EnableAlert((AlertType)alertType);
    }
    public void DisableAlertAshe(int alertType)
    {
        GameManager.Instance.Ashe.DisableAlert((AlertType)alertType);
    }
}

// Temp Level Struct
// If one will be made it will have its own attachable script to
// a prefab
//[Serializable, CreateAssetMenu(menuName = "Data/LevelData")]
//public class Level : ScriptableObject
//{
//    public Vector2 tinkerSpawn;
//    public Vector2 asheSpawn;
//    public string sceneName;
//    public ExitDoor tinkerExitDoor;
//    public ExitDoor asheExitDoor;
//    public Level nextLevel;
//}


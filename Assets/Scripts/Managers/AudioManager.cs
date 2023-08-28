using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton 
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
            }

            return instance;
        }
    }
    #endregion
    private AudioSource m_AudioSource;
    public AudioClip CurrentAudio => m_AudioSource.clip;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        m_AudioSource = GetComponent<AudioSource>();
    }

    public void SetAudioClip(AudioClip clip)
    {
        m_AudioSource.clip = clip;
    }

    public void PlayAudio() 
    { 
        if (!m_AudioSource.isPlaying) m_AudioSource.Play();
    }

    public void StopAudio()
    {
        m_AudioSource.Stop();
    }
    public void PauseAudio()
    {
        m_AudioSource.Pause();
    }
    public void SetVolume(float volume)
    {
        m_AudioSource.volume = volume;
    }
}

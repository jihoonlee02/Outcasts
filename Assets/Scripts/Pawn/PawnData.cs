using UnityEngine;

[CreateAssetMenu(menuName = "Data/PawnData"), System.Serializable]
public class PawnData : ScriptableObject
{
    [Header("Pawn Description")]
    [SerializeField] private string m_name;

    public string Name => m_name;

    [Header("Pawn Audio Clips")]
    [SerializeField] private AudioClip m_voice;
    [SerializeField] private AudioClip m_footstep;
    [SerializeField] private AudioClip m_jump;
    [SerializeField] private AudioClip[] m_scratchPadSounds;
    [SerializeField, Range(-1f, 2f)] private float min_pitch = 0.8f;
    [SerializeField, Range(-1f, 2f)] private float max_pitch = 1.5f;

    public AudioClip Voice => m_voice;
    public AudioClip Footstep => m_footstep;
    public AudioClip Jump => m_jump;
    public AudioClip[] ScratchPadSounds => m_scratchPadSounds;
    public float MinPitch => min_pitch;
    public float MaxPitch => max_pitch;
}

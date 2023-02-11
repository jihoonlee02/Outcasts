using UnityEngine;

[CreateAssetMenu(menuName = "Pawns/PawnData"), System.Serializable]
public class PawnData : ScriptableObject
{
    [Header("Pawn Description")]
    [SerializeField] private string m_name;

    [Header("Pawn Audio Clips")]
    [SerializeField] private AudioClip m_voice;
    [SerializeField] private AudioClip m_footstep;
    [SerializeField] private AudioClip m_jump;
    [SerializeField] private AudioClip[] m_scratchPadSounds;

    public AudioClip Voice => m_voice;
    public AudioClip Footstep => m_footstep;
    public AudioClip Jump => m_jump;
    public AudioClip[] ScratchPadSounds => m_scratchPadSounds;
}

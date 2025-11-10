using UnityEngine;

public enum SoundType
{
    JUMP,
    LAND,
    FOOTSTEP,
    HURT,
    DEATH,
    COIN,
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] m_SoundList;
    private static SoundManager m_Instance;
    private AudioSource m_AudioSource;

    private void Awake()
    {
        m_Instance = this;
    }

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound( SoundType sound, float volume = 1)
    {
        m_Instance.m_AudioSource.PlayOneShot(m_Instance.m_SoundList[(int)sound], volume);
    }
}

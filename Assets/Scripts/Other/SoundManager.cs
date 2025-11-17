using UnityEngine;
using System;
using System.Collections;

public enum SoundType
{
    JUMP,
    LAND,
    FOOTSTEP,
    HURT,
    HEAL,
    DEATH,
    COIN,
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    private CustomLogger CL;
    // Array of the sound list components
    [SerializeField] private SoundList[] m_SoundList;
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
    
    public static void PlaySound( SoundType sound, int clipIndex, float volume)
    {
        // Get the array of clips for the specified sound-type
        AudioClip[] clips = m_Instance.m_SoundList[(int)sound].sounds;
        
        // Check that the request is valid (e.g: if you select '3' when there's only 2 sounds)
        if (clipIndex >= 0 && clipIndex < clips.Length)
        {
            // Get that specific clip
            AudioClip clip = clips[clipIndex];
            
            // Play that sound!
            m_Instance.m_AudioSource.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogWarning($"<b><color=grey>[AUDIO]</color> Sound index <color=yellow>{clipIndex}</color> is out of range</b> (Clip range maximum: {clips.Length})");
        }
    }
    
    public static void PlayRandomSound( SoundType sound, float volume)
    {
        // Get the array of clips for the specified sound-type
        AudioClip[] clips = m_Instance.m_SoundList[(int)sound].sounds;
        
        // Select a random audio clip from the array
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        
        // Play that sound!
        m_Instance.m_AudioSource.PlayOneShot(randomClip, volume);
    }

    # if UNITY_EDITOR
    private void OnEnable()
    {
        // Get the names in the SoundType enum
        string[] m_HeaderNames = Enum.GetNames(typeof(SoundType));
        
        // Make sure the size of the array is always the same as the number of names in the enum
        Array.Resize(ref m_SoundList, m_HeaderNames.Length);
        
        // Now rename each of the elements to match the names in the enum
        for (int i = 0; i < m_SoundList.Length; i++)
        {
            m_SoundList[i].m_HeaderName = m_HeaderNames[i];
        }
    }
    #endif
}

[Serializable]
public struct SoundList
{
    public AudioClip[] sounds { get => m_Sounds; }
    [HideInInspector] public string m_HeaderName;
    // Array of sounds
    [SerializeField] private AudioClip[] m_Sounds;
}
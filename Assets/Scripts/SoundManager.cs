using UnityEngine;

public enum SoundType
{
    Jump,
    Land,
    Footstep,
    
    Coin,
    
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager m_Instance;

    private void Awake()
    {
        m_Instance = this;
    }

    public static void PlaySound( /*SoundType, volume*/)
    {
        
    }
}

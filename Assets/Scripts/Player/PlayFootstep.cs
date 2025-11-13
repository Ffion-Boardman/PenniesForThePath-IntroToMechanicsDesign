using UnityEngine;

public class PlayFootstep : MonoBehaviour
{
    [SerializeField] CharacterMovement m_CharacterMovement;
    private int StepType = 0;
    
    public void PlaySound()
    {
        if (StepType == 0)
        {
            SoundManager.PlaySound(SoundType.FOOTSTEP, StepType, m_CharacterMovement.m_FootstepVolume);
            StepType = 1;
        }
        else
        {
            SoundManager.PlaySound(SoundType.FOOTSTEP, StepType, m_CharacterMovement.m_FootstepVolume);
            StepType = 0;
        }
        
    }
}

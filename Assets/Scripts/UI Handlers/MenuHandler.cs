using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_ClickParticles;
    private UIDocument m_UIDocument;
    private Button m_Play;
    private Button m_Exit;
    private Coroutine m_PlayCoroutine;
    private Coroutine m_ExitCoroutine;
    [SerializeField] private AudioSource m_MusicSource;
    
    private void Awake()
    {
        m_UIDocument = GetComponent<UIDocument>();

        if (m_UIDocument == null)
        {
            Debug.LogError("No IDocument found!");
        }
        
        m_Play = m_UIDocument.rootVisualElement.Q<Button>("Playbtn");
        m_Exit = m_UIDocument.rootVisualElement.Q<Button>("Exitbtn");
    }

    private void Handle_PlayGame(ClickEvent clickEvent)
    {
        Debug.Log("Play Game pressed!");
        StartCoroutine(C_PlayCoroutine());
    }

    IEnumerator C_PlayCoroutine()
    {
        // Lower music volume
        m_MusicSource.volume = 0;
        
        // Play the right sound
        SoundManager.PlaySound(SoundType.UI, 0, 1);
        
        // Wait for the sound and particles to be done
        yield return new WaitForSeconds(2);
        
        // Now load the next scene
        SceneManager.LoadScene("Sandbox");
    }

    private void Handle_Exit(ClickEvent clickEvent)
    {
        Debug.Log("Exit pressed!");
        SoundManager.PlaySound(SoundType.UI, 1, 1);
        Application.Quit();
    }

    private void OnEnable()
    {
        m_Play.RegisterCallback<ClickEvent>(Handle_PlayGame);
        m_Exit.RegisterCallback<ClickEvent>(Handle_Exit);
    }

    private void OnDisable()
    {
        m_Play.UnregisterCallback<ClickEvent>(Handle_PlayGame);
        m_Exit.UnregisterCallback<ClickEvent>(Handle_Exit);
    }
}

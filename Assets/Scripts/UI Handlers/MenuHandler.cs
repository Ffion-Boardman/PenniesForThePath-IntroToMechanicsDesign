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
    private Camera m_MainCamera;
    
    private void Awake()
    {
        // Getting the camera to handle click-particles
        m_MainCamera = Camera.main;
        
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
        
        // Get the location of the click
        Vector2 m_MousePosition = Mouse.current.position.ReadValue();
        
        // Make it Vector3 so that it's visible
        Vector3 m_WorldPoint = m_MainCamera.ScreenToWorldPoint(new Vector3(m_MousePosition.x, m_MousePosition.y, m_MainCamera.nearClipPlane + 10f));
        
        // Play the VFX at the mouse location
        m_ClickParticles.transform.position = m_WorldPoint;
        m_ClickParticles.Play();
        
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

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MenuHandler : MonoBehaviour
{
    private UIDocument m_UIDocument;
    private Button m_Play;
    private Button m_Exit;
    
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
        SceneManager.LoadScene("Sandbox");
    }

    private void Handle_Exit(ClickEvent clickEvent)
    {
        Debug.Log("Exit pressed!");
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

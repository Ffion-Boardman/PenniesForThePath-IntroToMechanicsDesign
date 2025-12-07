using System;
using UnityEngine;
using UnityEngine.UIElements;

public class HealthUI : MonoBehaviour
{
    private VisualElement[] m_Hearts;
    private HealthComponent m_HealthComponent;
    private VisualElement m_HeartContainer;
    
    [SerializeField] private UIDocument m_UIDocument;
    [SerializeField] private string m_ContainerName = "HeartContainer";

    [SerializeField] private Sprite m_EmptyHeartSprite;
    [SerializeField] private Sprite m_FullHeartSprite;
    
    private Texture2D m_EmptyHeartTexture;
    private Texture2D m_FullHeartTexture;

    private void Awake()
    {
        // Convert the sprites to be textures
        m_EmptyHeartTexture = m_EmptyHeartSprite.texture;
        m_FullHeartTexture = m_FullHeartSprite.texture;
        
        // Get the root of the UI document
        VisualElement root = m_UIDocument.rootVisualElement;
        
        m_HeartContainer = root.Q<VisualElement>(m_ContainerName);
        Debug.Assert(m_HeartContainer != null, $"<b><color=red>[ERROR]</color><color=grey>[UI]</color> Could not find Visual Element named '<color=yellow>{m_ContainerName}</color>'</b>");
    }

    public void Initialize(HealthComponent healthComponent)
    {
        m_HealthComponent = healthComponent;
        m_HealthComponent.OnDamaged += UpdateHearts;
        
        // On the first link, initialize the hearts
        CreateHearts(m_HealthComponent.MaxHealth);
        UpdateHearts(m_HealthComponent.CurrentHealth, m_HealthComponent.MaxHealth, 0);
    }

    private void OnDestroy()
    {
        if (m_HealthComponent != null)
        {
            m_HealthComponent.OnDamaged -= UpdateHearts;
        }
    }

    private void CreateHearts(int maxHealth)
    {
        // Clear anything that might already be there
        m_HeartContainer.Clear();
        m_Hearts = new VisualElement[maxHealth];

        for (int i = 0; i < maxHealth; i++)
        {
            // Create a visual for the heart
            VisualElement heart = new VisualElement();
            heart.name = "Heart_" +i;
            
            // Set the initial image
            heart.style.backgroundImage = m_FullHeartTexture;
            
            // Add the styling class to them
            heart.AddToClassList("heart-icon");
            
            // Add the heart to the container
            m_HeartContainer.Add(heart);
            
            m_Hearts[i] = heart;
        }
    }

    private void UpdateHearts(int currentHealth, int maxHealth, int inboundDamage)
    {
        // Go through all the hearts we have
        for (int i = 0; i < maxHealth; i++)
        {
            // If the current i is less than the current health, then the heart should be full
            if (i < currentHealth)
            {
                m_Hearts[i].style.backgroundImage = m_FullHeartTexture;
            }
            else
            {
                // Otherwise, the heart is empty
                m_Hearts[i].style.backgroundImage = m_EmptyHeartTexture;
            }
        }
    }
}

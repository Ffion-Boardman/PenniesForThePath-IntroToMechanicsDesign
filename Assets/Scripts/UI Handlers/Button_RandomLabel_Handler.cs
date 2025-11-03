using System;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Button_RandomLabel_Handler : MonoBehaviour
{
    private VisualElement m_uiRoot;
    private ScrollView m_scrollArea;
    
    private void Awake()
    {
        #region RootElementCreation
        VisualElement rootElement = new VisualElement();
        rootElement.name = "container";
        
        GetComponent<UIDocument>().rootVisualElement.Add(rootElement);
        m_uiRoot = rootElement;
        #endregion
        
        #region ButtonCreation
        // Creating the button
        Button button = new Button();
        
        // Names and labels
        button.text = "Press me!";
        button.name = "btn_labelspawner";
        
        // OnClick event
        button.RegisterCallback<ClickEvent>(Handle_LabelSpawn);
        button.AddToClassList("button");
        
        // Adding the button to the scene
        m_uiRoot.Add(button);
        #endregion
    }

    private void Handle_LabelSpawn(ClickEvent clickEvent)
    {
        #region ScrollViewCreation
        if (m_scrollArea == null)
        {
            ScrollView list = new ScrollView();
            list.name = "scr_labelcontainer";
            list.AddToClassList("scrollview");
            
            m_uiRoot.Add(list);
            m_scrollArea = list;
        }
        #endregion
        
        Label label = new Label();
        label.name = "lbl_randomnumberlabel";
        label.text = Random.Range(Int32.MinValue, Int32.MaxValue).ToString();
        label.AddToClassList("label");
        m_scrollArea.Add(label);
        
        Debug.Log("Handle_LabelSpawn clicked!");
    }
}

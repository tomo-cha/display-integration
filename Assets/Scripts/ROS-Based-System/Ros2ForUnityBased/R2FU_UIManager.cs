using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public sealed class R2FU_UIManager : MonoBehaviour
{
    //[SerializeField] GameObject sampleButtonUI;
    [SerializeField] UIDocument _uiDocument;
    
    TextField inpElem1, inpElem2;
    Label label;
    VisualElement headerElement;
    [SerializeField] GameObject InformationObject;
    Color errorColor = new Color(0.75f, 0f, 0f);
    bool enableTransition = false;
    float _h;

    float lastTime = 0;
    float duration = 1 / 30f;
    void Start()
    {
        var connectElement = _uiDocument.rootVisualElement.Q<Label>("Connect");
        connectElement.AddManipulator(new Clickable(ButtonClicked));

        var quitElement = _uiDocument.rootVisualElement.Q<Label>("Quit");
        quitElement.AddManipulator(new Clickable(QuitButtonClicked));

        label = _uiDocument.rootVisualElement.Q<Label>("DescriptionText1");

        inpElem1 = _uiDocument.rootVisualElement.Q<TextField>("NamespaceInputField");
        inpElem2 = _uiDocument.rootVisualElement.Q<TextField>("ScreenHeightInputField");

        headerElement = _uiDocument.rootVisualElement.Q<VisualElement>("Header");
        _h = Screen.height * 0.1f;

     //   InformationObject = GameObject.Find("Information");
    }

    void ButtonClicked()
    {
        if(inpElem1.text != "" && inpElem2.text != "")
        {
            enableTransition = true;

            InformationObject.GetComponent<ValueTransport>().rosNamespace = inpElem1.text;
            InformationObject.GetComponent<ValueTransport>().screenHeight = float.Parse(inpElem2.text);
            
            DontDestroyOnLoad(InformationObject);            
        }
        else
        {
            label.text = "Settings Menu --[ERROR] Please Fill All Forms.";
            label.style.color = errorColor;
        }     
    }

    // Update is called once per frame
    void Update()
    {
        if(enableTransition)
        {
            if(Time.time - lastTime > duration)
            {
                _h += (Screen.height - _h) * (1080f / Screen.height * 0.125f);
                headerElement.style.height = _h;

                lastTime = Time.time;
            }

            if (_h > Screen.height * 0.99) SceneManager.LoadScene("R2FU-Client");
            
        }
    }

    void QuitButtonClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}

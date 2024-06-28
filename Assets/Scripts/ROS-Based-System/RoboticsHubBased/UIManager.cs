using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine.UI;

public sealed class UIManager : MonoBehaviour
{
    //[SerializeField] GameObject sampleButtonUI;
    [SerializeField] UIDocument _uiDocument;
    
    TextField inpElem1, inpElem2, inpElem3, inpElem4, inpElem5, inpElem6;
    Label label;
    VisualElement headerElement;
    ROSConnection ros;
    GameObject rosObject;
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

        inpElem1 = _uiDocument.rootVisualElement.Q<TextField>("IPTextField1");
        inpElem2 = _uiDocument.rootVisualElement.Q<TextField>("IPTextField2");
        inpElem3 = _uiDocument.rootVisualElement.Q<TextField>("IPTextField3");
        inpElem4 = _uiDocument.rootVisualElement.Q<TextField>("IPTextField4");

        inpElem5 = _uiDocument.rootVisualElement.Q<TextField>("NamespaceInputField");
        inpElem6 = _uiDocument.rootVisualElement.Q<TextField>("ScreenHeightInputField");

        headerElement = _uiDocument.rootVisualElement.Q<VisualElement>("Header");
        _h = Screen.height * 0.1f;

        rosObject = GameObject.Find("ROSConnectionPrefab(Clone)");
        if(rosObject != null && rosObject.GetComponent<ValueTransport>().connectionError)
        {
            label.text = "Settings Menu --[ERROR] Connection Failed.";
            label.style.color = errorColor;
        }
    }

    void ButtonClicked()
    {
        if(inpElem1.text != "" && inpElem2.text != "" && inpElem3.text != "" && inpElem4.text != "" && inpElem5.text != "" && inpElem6.text != "")
        {
            enableTransition = true;
            string ipAddress = inpElem1.text + "." + inpElem2.text + "." + inpElem3.text + "." + inpElem4.text;

            ros = ROSConnection.GetOrCreateInstance();
            ros.ConnectOnStart = false;
            ros.RosIPAddress = ipAddress;
            ros.RosIPAddress = ipAddress;
            ros.RosPort = 10000;
            ros.ShowHud = false;
            ros.NetworkTimeoutSeconds = 1f;        

            rosObject = GameObject.Find("ROSConnectionPrefab(Clone)");
            rosObject.AddComponent<ValueTransport>();
            rosObject.GetComponent<ValueTransport>().rosNamespace = inpElem5.text;
            rosObject.GetComponent<ValueTransport>().screenHeight = float.Parse(inpElem6.text);
            
            ros.Connect();

            DontDestroyOnLoad(rosObject);            
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

            if (_h > Screen.height * 0.99)
            {
                rosObject.GetComponent<ValueTransport>().connectionError = ros.HasConnectionError;
                if(ros.HasConnectionError)
                {                    
                    SceneManager.LoadScene("IPAddressInput");
                    ros.Disconnect();
                }
                else
                {
                    SceneManager.LoadScene("ROS-Client");
                }
            }
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

    private void OnApplicationQuit()
    {
        if(enableTransition)
        {
            ros.Disconnect();
        }
    }
}

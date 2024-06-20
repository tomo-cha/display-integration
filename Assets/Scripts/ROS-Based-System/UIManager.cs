using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using Unity.Robotics.ROSTCPConnector;

public sealed class UIManager : MonoBehaviour
{
    //[SerializeField] GameObject sampleButtonUI;
    [SerializeField] UIDocument _uiDocument;
    
    TextField inpElem1, inpElem2, inpElem3, inpElem4, inpElem5;
    VisualElement headerElement;
    ROSConnection ros;
    bool enableTransition = false;
    float _h;

    float lastTime = 0;
    float duration = 1 / 30f;
    void Start()
    {
        var connectElement = _uiDocument.rootVisualElement.Q<Label>("Connect");
        connectElement.AddManipulator(new Clickable(ButtonClicked));

        inpElem1 = _uiDocument.rootVisualElement.Q<TextField>("IPTextField1");
        inpElem2 = _uiDocument.rootVisualElement.Q<TextField>("IPTextField2");
        inpElem3 = _uiDocument.rootVisualElement.Q<TextField>("IPTextField3");
        inpElem4 = _uiDocument.rootVisualElement.Q<TextField>("IPTextField4");

        inpElem5 = _uiDocument.rootVisualElement.Q<TextField>("NamespaceInputField");

        headerElement = _uiDocument.rootVisualElement.Q<VisualElement>("Header");
        _h = Screen.height * 0.1f;
    }

    void ButtonClicked()
    {
        enableTransition = true;
        string ipAddress = inpElem1.text + "." + inpElem2.text + "." + inpElem3.text + "." + inpElem4.text;
        Debug.Log("ip address: " +ipAddress);
        Debug.Log("Clicked");

        ros = ROSConnection.GetOrCreateInstance();
        ros.ConnectOnStart = false;
        ros.RosIPAddress = ipAddress;
        ros.RosIPAddress = ipAddress;
        ros.RosPort = 10000;
        ros.ShowHud = false;
        ros.NetworkTimeoutSeconds = 1f;        

        var rosObject = GameObject.Find("ROSConnectionPrefab(Clone)");
        rosObject.AddComponent<RosNamespaceManager>();
        rosObject.GetComponent<RosNamespaceManager>().rosNamespace = inpElem5.text;

        DontDestroyOnLoad(rosObject);
       
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("screen height: " + Screen.height + "/ _h: " + _h);

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
                ros.Connect();
                SceneManager.LoadScene("ROS-Client");
            }
        }

    }
    private void OnApplicationQuit()
    {
        ros.Disconnect();
    }
}

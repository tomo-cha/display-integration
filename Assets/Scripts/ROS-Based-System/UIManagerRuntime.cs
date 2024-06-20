using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

using Unity.Robotics.ROSTCPConnector;
using Unity.VisualScripting;

public class UIManagerRuntime : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] UIDocument _uiDocument;
    VisualElement headerElement;
    ROSConnection ros;
    bool enableTransition = false;
    float _h;

    float lastTime = 0;
    float duration = 1 / 30f;

    void Start()
    {
        var settingsElement = _uiDocument.rootVisualElement.Q<Label>("Settings");
        settingsElement.AddManipulator(new Clickable(SettingsButtonClicked));

        var quitElement = _uiDocument.rootVisualElement.Q<Label>("Quit");
        quitElement.AddManipulator(new Clickable(QuitButtonClicked));

        headerElement = _uiDocument.rootVisualElement.Q<VisualElement>("Header");
        _h = Screen.height * 0.1f;

        ros = ROSConnection.GetOrCreateInstance();
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
                SceneManager.LoadScene("IPAddressInput");
            }
        }
    }

    void SettingsButtonClicked()
    {
        ros.Disconnect();
        var rosObject = GameObject.Find("ROSConnectionPrefab(Clone)");
        
        if(rosObject != null)
        {
            GameObject.Destroy(rosObject);
        }        

        Debug.Log("Settings Button");
        enableTransition = true;
        
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
        ros.Disconnect();
    }
}

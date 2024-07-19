using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UIElements;
public class UITransition : MonoBehaviour
{
    [SerializeField] UIDocument _uiDocument;
    VisualElement headerElement;
    float _h;
    bool enableTransition = true;
    float lastTime = 0;
    float duration = 1 / 30f;
    // Start is called before the first frame update
    void Start()
    {
        headerElement = _uiDocument.rootVisualElement.Q<VisualElement>("Header");
        _h = Screen.height;

        Debug.Log("aaaaa");

        enableTransition = true;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(enableTransition)
        {
            if(Time.time - lastTime > duration)
            {
                _h += (Screen.height * 0.05f - _h) * (1080f / Screen.height * 0.125f);
                headerElement.style.height = _h;

                lastTime = Time.time;
            }

            if (_h < Screen.height * 0.05f)
            {
                Debug.Log("Moving Stop");
                headerElement.style.height  = Screen.height * 0.05f;
                enableTransition = false;

                this.gameObject.SetActive(false);
            }
        }

        
        
    }
}

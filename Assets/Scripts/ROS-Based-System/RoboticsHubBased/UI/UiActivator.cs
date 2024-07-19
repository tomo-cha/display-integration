using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiActivator : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject UiGameObject;
    public bool showGUI = false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    // #if UNITY_EDITOR
    //     if(Input.GetKeyUp(KeyCode.Escape))
    //     {
    //         showGUI = !showGUI;            
    //     }
    // #else
        if(Input.GetMouseButtonUp(0))
        {
            var mousePosition = Input.mousePosition;
            //Debug.Log(mousePosition);
            if(mousePosition.y > Screen.height * 0.8)
            {
                showGUI = !showGUI;
            }
        }

    //#endif

        UiGameObject.SetActive(showGUI);
    }
}

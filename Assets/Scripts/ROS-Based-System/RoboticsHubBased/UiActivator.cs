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
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            showGUI = !showGUI;
        }

        UiGameObject.SetActive(showGUI);
    }
}

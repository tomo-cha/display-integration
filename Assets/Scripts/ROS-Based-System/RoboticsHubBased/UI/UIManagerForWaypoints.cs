using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class UIManagerForWaypoints : MonoBehaviour
{
    [SerializeField] UIDocument _uiDocument;

    // Start is called before the first frame update
    void Start()
    {
        var resetButtonElement = _uiDocument.rootVisualElement.Q<Label>("Reset");
        resetButtonElement.AddManipulator(new Clickable(ResetClicked));
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ResetClicked()
    {
        Debug.Log("Reset Button is Clicked");
       
    }
}

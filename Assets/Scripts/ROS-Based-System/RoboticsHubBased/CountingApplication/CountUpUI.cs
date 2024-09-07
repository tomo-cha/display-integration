using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UIElements;
using UnityEngine.SceneManagement;


public class CountUpUI : MonoBehaviour
{
    [SerializeField] UIDocument _uiDocument;

    Label countButtonElement, timeRemainElement;
    float duration = 0.2f;
    float pressedTime = 0f;

    [SerializeField] Color defaultColor;
    [SerializeField] Color pressedColor;
    [SerializeField] float playableTime = 10f;
    [SerializeField] string nextSceneName;

    public int num_playerCount;
    float timeRemain = 0f;
    float beginTime;
    
    // Start is called before the first frame update
    void Start()
    {
        num_playerCount = 0;
        timeRemain = playableTime;

        countButtonElement = _uiDocument.rootVisualElement.Q<Label>("Count");
        countButtonElement.AddManipulator(new Clickable(ButtonPressed));
        countButtonElement.style.backgroundColor = defaultColor;
        
        timeRemainElement = _uiDocument.rootVisualElement.Q<Label>("TimeRemainSec");

        beginTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Time.time - pressedTime > duration)
        {
            countButtonElement.style.backgroundColor = defaultColor;
        }

        timeRemain = playableTime - (Time.time - beginTime);

        if(timeRemain < 0)
        {
           SceneManager.LoadScene(nextSceneName);
        }

        timeRemainElement.text = timeRemain.ToString("F2");

       
    }

    void ButtonPressed()
    {
        countButtonElement.style.backgroundColor = pressedColor;

        num_playerCount ++;
        Debug.Log("Button Pressed ---" + num_playerCount + "times!!!");

        pressedTime = Time.time;
    }
}

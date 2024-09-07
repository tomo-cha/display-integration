using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class ResultUI : MonoBehaviour
{
    [SerializeField] UIDocument _uiDocument;
    [SerializeField] string nextSceneName;
    GameObject dataContainer;
    Label realResultElement, userResultElement;
    int num_playerCount, num_realCount;

    bool doneRealCount, donePlayerCount;

    [SerializeField] float max_interval = 0.2f;
    float interval;
    float minInterval = 0.05f;
    float lastTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        donePlayerCount = false;
        doneRealCount = false;

        realResultElement = _uiDocument.rootVisualElement.Q<Label>("RealResult");
        userResultElement = _uiDocument.rootVisualElement.Q<Label>("UserResult");

        var countButtonElement = _uiDocument.rootVisualElement.Q<Label>("Restart");
        countButtonElement.AddManipulator(new Clickable(ButtonPressed));

        dataContainer = GameObject.Find("PlayDataManager");

        num_playerCount = 0;
        num_realCount = 0;
        
        interval = max_interval;
    }

    // Update is called once per frame
    void Update()
    {
        if(!doneRealCount)
        {
            if(Time.time - lastTime > interval)
            {
                num_realCount ++;
                lastTime = Time.time;
                interval -= 0.01f;
            }
           

            if(num_realCount > dataContainer.GetComponent<CountDataContainer>().num_realCount)
            {
                num_realCount = dataContainer.GetComponent<CountDataContainer>().num_realCount;
                doneRealCount = true;

                interval = max_interval;                
            }

            realResultElement.text = num_realCount.ToString();

        }

        if(doneRealCount && !donePlayerCount)
        {
            if(Time.time - lastTime > interval)
            {
                num_playerCount ++;
                lastTime = Time.time;
                interval -= 0.01f;
            }
            
            if(num_playerCount > dataContainer.GetComponent<CountDataContainer>().num_playerCount)
            {
                num_playerCount = dataContainer.GetComponent<CountDataContainer>().num_playerCount;
                donePlayerCount = true;
            }

            userResultElement.text = num_playerCount.ToString();
        }
        
    }

    void ButtonPressed()
    {
        SceneManager.LoadScene(nextSceneName);

        GameObject.Destroy(dataContainer);
    }
}

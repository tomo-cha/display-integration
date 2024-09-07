using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CountDataContainer : MonoBehaviour
{
    [SerializeField] GameObject gamePlayObject;
    [SerializeField] string activatableName;
    public int num_playerCount, num_realCount;
    bool initRecieveFlag = false;
    int initValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        if(activatableName == SceneManager.GetActiveScene().name)
        {
            num_playerCount = 0;
            num_realCount = 0;
            initValue = 0;
            DontDestroyOnLoad(this);
        } 
    }

    // Update is called once per frame
    void Update()
    {
        if(activatableName == SceneManager.GetActiveScene().name)
        {
        
            if(!initRecieveFlag)
            {
                initValue = gamePlayObject.GetComponent<RealCountSubscriber>().num_realCount;
                initRecieveFlag = true;
            }
            else
            {
                num_realCount = gamePlayObject.GetComponent<RealCountSubscriber>().num_realCount - initValue;

            }

            num_playerCount = gamePlayObject.GetComponent<CountUpUI>().num_playerCount;
        }      

    }
}

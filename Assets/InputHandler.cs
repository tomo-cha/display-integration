using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // EventSystemのために必要

public class InputHandler : MonoBehaviour
{
    public InputField inputField;
    private GameObject fieldObject;
    //public Text PortTxt;
    public static int SetPortN;
    public OscDis_Receive oscDisReceive; // OscDis_Receiveへの参照
    
     void Start()
    {
        // InputFieldのonEndEditイベントにOnInputTextメソッドをアタッチする
        inputField = inputField.GetComponent<InputField>();
        fieldObject = inputField.gameObject;

         if (oscDisReceive == null)
         //maincameraのインスペクターにターゲットとなるゲームオブジェクトがアサインされていないとエラーが表示されるようになる
        {
            Debug.LogError("OscDis_Receive is not assigned in the inspector");
        }
    }

   public void InputText()
    {
        //stirngの値をintに変換する
        if (int.TryParse(inputField.text, out SetPortN))
        {
            if (oscDisReceive != null)
            {
                //InputFieldに入力された文字をPortNumberとして設定
                oscDisReceive.SetPortObjectPos(SetPortN);
            }
            else
            {
                Debug.LogError("oscDisReceive is null in InputText");
            }
        }
        else
        {
            Debug.LogError("Invalid port number");
        }
    }


    void Update() {
    //Enterキーが押されたときにinputFieldのゲームオブジェクトを非表示にする
        if (Input.GetKey(KeyCode.Return)){
            fieldObject.SetActive(false);
        }
    }
 
}
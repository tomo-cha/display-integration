using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PortNumber : MonoBehaviour
{
    //InputFieldを格納するための変数
    InputField inputField;


    // Start is called before the first frame update
    void Start()
    {
        //InputFieldコンポーネントを取得
        inputField = GameObject.Find("InputField").GetComponent<InputField>();
    }


    //入力された名前情報を読み取ってコンソールに出力する関数
    public void GetInputName()
    {
        //InputFieldからテキスト情報を取得する
        string name = inputField.text;
        Debug.Log(name);

        //入力フォームのテキストを空にする
        inputField.text = "";
    }
}
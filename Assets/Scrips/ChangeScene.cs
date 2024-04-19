using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//Unityエンジンのシーン管理プログラムを利用する

public class ChangeScene : MonoBehaviour
{
    public void change_tomokichi_scene()
    {
        SceneManager.LoadScene("tomokichi");//sceneを呼び出します
    }
    public void change_masaya_scene()
    {
        SceneManager.LoadScene("masaya");
    }
    public void change_main_scene()
    {
        SceneManager.LoadScene("main");
    }
}

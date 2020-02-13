using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
   //用来现实安卓发过来的内容
    public Text text;
    public Button btn;
  #if UNITY_ANDROID
    private AndroidJavaObject javaObj;
  #endif
    void Start()
    {
#if UNITY_ANDROID
        //通过该API来实例化导入的arr中对应的类
        javaObj = new AndroidJavaObject("com.pub.myunitylib.Unity2Android");
#endif
        btn.onClick.AddListener(() =>
        {
#if UNITY_ANDROID
            //通过API来调用原生代码的方法
            bool success = javaObj.Call<bool>("ShowToast","this is unity");
            if(success)
            {
                //请求成功
                Debug.Log("call android suc");
            }
            else
            {
                Debug.Log("call android failed");
            }
            
#elif UNITY_IOS
           CallIOSBridge.CallIOSNativeFunction();
#endif

        });   

    }

    private void FixedUpdate()
    {
#if UNITY_ANDROID
        if (javaObj != null)
        {
            int status = javaObj.Call<int>("CheckNetReachability");
            switch (status)
            {
                case -1:
                    text.text = "网络不可用";
                    break;
                case 1:
                    text.text = "WIFI";
                    break;
                case 2:
                    text.text = "2G";
                    break;
                case 3:
                    text.text = "3G";
                    break;
                case 4:
                    text.text = "4G";
                    break;
            }
        }
#endif
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /// <summary>
    /// 原生层通过该方法传回信息
    /// </summary>
    /// <param name="content"></param>
    public void FromAndroid(string content)
    {
        text.text = content;
    }
}

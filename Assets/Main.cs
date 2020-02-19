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

    private NetReachAbility netReachAbility;
    private void Awake()
    {
        netReachAbility = gameObject.AddComponent<NetReachAbility>();
        var status = netReachAbility.NetState;
        ShowState(status);
    }
//    public void OnGUI()
//    {
//        if (GUILayout.Button("SERIALIZE"))
//        {
//            string myJson = JsonWriter.Serialize(myPC);
//            Debug.Log(myJson);
//        }
//    }

    void Start()
    {
        btn.onClick.AddListener(() =>
        {
            //测试LitJson
            var res = Test.TestJson.Test();
            text.text = res;
        });

        
//#if UNITY_ANDROID
//        //通过该API来实例化导入的arr中对应的类
//        var javaObj = new AndroidJavaObject("com.pub.myunitylib.Unity2Android");
//#endif
//        btn.onClick.AddListener(() =>
//        {
//#if UNITY_ANDROID
//            //通过API来调用原生代码的方法
//            bool success = javaObj.Call<bool>("ShowToast","this is unity");
//            if(success)
//            {
//                //请求成功
//                Debug.Log("call android suc");
//            }
//            else
//            {
//                Debug.Log("call android failed");
//            }
//            
//#elif UNITY_IOS
//           CallIOSBridge.CallIOSNativeFunction();
//#endif
//        });   

    }

    private void ShowState(RealNetState state)
    {
        switch (state)
        {
            case RealNetState.NETWORK_NOT_REACH:
                text.text = "网络不可用";
                break;
            case RealNetState.NETWORK_WIFI:
                text.text = "WIFI";
                break;
            case RealNetState.NETWORK_CLASS_2_G:
                text.text = "2G";
                break;
            case RealNetState.NETWORK_CLASS_3_G:
                text.text = "3G";
                break;
            case RealNetState.NETWORK_CLASS_4_G:
                text.text = "4G";
                break;
            case RealNetState.NETWORK_CLASS_UNKNOWN:
                text.text = "UnKown Network";
                break;
        }
    }
    private void FixedUpdate()
    {
        var status = netReachAbility.NetState;
      //  ShowState(status);
    }

    // Update is called once per frame
    /// <summary>
    /// 原生层通过该方法传回信息
    /// </summary>
    /// <param name="content"></param>
    public void FromAndroid(string content)
    {
        text.text = content;
    }
}

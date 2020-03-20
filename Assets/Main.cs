using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;
using Logic;
using NetMsg.Common;

public class Main : MonoBehaviour
{

   //用来现实安卓发过来的内容
    public Text text;
    public Button btn;

    private NetReachAbility netReachAbility;
    private void Awake()
    {
        //netReachAbility = gameObject.AddComponent<NetReachAbility>();
        //var status = netReachAbility.NetState;
        //ShowState(status);
        NetWorkService.Instance.Init();
        
        XLogger.Info("Game Start`````````````````");
        XLogger.Info("Game Start22`````````````````");
        Application.quitting += AppQuit;
    }

    void AppQuit()
    {
        Debug.Log("AppQuit ending after " + Time.time + " seconds");
        Framework.XLogger.Info("App is Quiting========");
        NetWorkService.Instance.SendTcp(EMsgSC.C2L_LeaveRoom,new Msg_C2L_LeaveRoom());
        NetWorkService.Instance.UnInit();
    }
    
    void AppQuit2(string args)
    {
        Debug.Log("AppQuit ending after " + Time.time + " seconds");
        Framework.XLogger.Info("App is Quiting========");
        NetWorkService.Instance.SendTcp(EMsgSC.C2L_LeaveRoom,new Msg_C2L_LeaveRoom());
        NetWorkService.Instance.UnInit();
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
        NetWorkService.Instance.ConnectSrv();
        
//        btn.onclick.addlistener(() =>
//        {
//            //测试litjson
////            var res = test.testjson.test();
////            framework.xlogger.info(res);
////            text.text = res;
//
//            //send msg
//            networkservice.instance.sendtcp(emsgsc.c2l_test,new msg_c2l_test());
//            
//        });

        
#if UNITY_ANDROID
        //通过该API来实例化导入的arr中对应的类
        var javaObj = new AndroidJavaObject("com.pub.myunitylib.Unity2Android");
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
           
            NetWorkService.Instance.SendTcp(EMsgSC.C2L_Test,new Msg_C2L_Test());
#elif UNITY_IOS
           CallIOSBridge.CallIOSNativeFunction();
#endif
        });
        
#if UNITY_STANDALONE_WIN
       // List<LoginInfo> caches = LitJson.JsonMapper.ToObject<List<LoginInfo>>(data);
#else
    List<LoginInfo> caches = JsonConvert.DeserializeObject<List<LoginInfo>>(data);
#endif

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
      //  var status = netReachAbility.NetState;
      //  ShowState(status);
    }

    private void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit ending after " + Time.time + " seconds");
        XLogger.Info("Game Exit````````````````````");
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

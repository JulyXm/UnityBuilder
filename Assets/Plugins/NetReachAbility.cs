using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public enum RealNetState
{
    NETWORK_NOT_REACH = -1,            //网络不可用
    NETWORK_CLASS_UNKNOWN = 0,         //未知网络类型
    NETWORK_WIFI = 1,                  //wifi
    NETWORK_CLASS_2_G = 2,             //2g
    NETWORK_CLASS_3_G = 3,             //3g
    NETWORK_CLASS_4_G = 4,             //4g
    
}
public class NetReachAbility : MonoBehaviour
{
    // Start is called before the first frame update
    private static NetReachAbility inst;
    private RealNetState realNetState;
#if UNITY_ANDROID
    private AndroidJavaObject javaObj;
#endif
    
#if UNITY_ANDROID || UNITY_IOS
    //初始化ReachAbility
    [DllImport("__Internal")]
    public static extern  int InitNetReachAbility();
    //销毁ReachAbility
    [DllImport("__Internal")]
    public static extern  void UnInitNetReachAbility();
#endif 
    
    public static NetReachAbility GetInstance
    {
        get { return inst; }
    }

    /// <summary>
    /// 获取当前网络状态
    /// </summary>
    public RealNetState NetState
    {
        get
        {
        #if UNITY_IOS
            return realNetState;
        #elif UNITY_ANDROID
            realNetState  = (RealNetState)javaObj.Call<int>("CheckNetReachability");
            return realNetState;
            #else
            return RealNetState.NETWORK_CLASS_UNKNOWN;
        #endif
        }
    }

    void Awake()
    {
        inst = this;
        this.gameObject.name = "NetReachAbility";
        DontDestroyOnLoad(this.gameObject);
#if UNITY_ANDROID
        //通过该API来实例化导入的arr中对应的类
        javaObj = new AndroidJavaObject("com.pub.myunitylib.Unity2Android");
#elif UNITY_IOS
        int netState = NetReachAbility.InitNetReachAbility();
        realNetState = (RealNetState)netState;
#endif
    }

    void Start()
    {
    }

    private void OnDestroy()
    {
#if UNITY_IOS
         NetReachAbility.UnInitNetReachAbility();
#endif
    }

    /// <summary>
    /// IOS网络状态改变的回调
    /// </summary>
    /// <param name="status"></param>
    void OnNetStatus(string status)
    {
        try
        {
            realNetState = (RealNetState) int.Parse(status);
        }
        catch (Exception e)
        {
            realNetState = RealNetState.NETWORK_CLASS_UNKNOWN;
        }
    }
    
}

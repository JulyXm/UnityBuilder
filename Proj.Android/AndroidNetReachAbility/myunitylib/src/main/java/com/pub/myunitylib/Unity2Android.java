package com.pub.myunitylib;

import android.app.Activity;
import android.content.Context;
import android.widget.Toast;

import java.lang.reflect.InvocationTargetException;
import java.lang.reflect.Method;


public class Unity2Android {
    //unity项目启动时的上下文
    private Activity unityActivity;
    private Context context;

    /**
     * 利用反射机制获取unity项目的上下文
     * @return
     */
    Activity getActivity(){
        if(null == unityActivity) {
            try {
                Class<?> classtype = Class.forName("com.unity3d.player.UnityPlayer");
                Activity activity = (Activity) classtype.getDeclaredField("currentActivity").get(classtype);
                unityActivity = activity;
                context = activity;
            } catch (ClassNotFoundException e) {
                System.out.println(e.getMessage());
            } catch (IllegalAccessException e) {
                System.out.println(e.getMessage());
            } catch (NoSuchFieldException e) {
                System.out.println(e.getMessage());
            }
        }
        return unityActivity;
    }

    /**
     * 调用Unity的方法
     * @param gameObjectName    调用的GameObject的名称
     * @param functionName      方法名
     * @param args              参数
     * @return                  调用是否成功
     */
    public boolean CallUnity(String gameObjectName, String functionName, String args){
        try {
            Class<?> classtype = Class.forName("com.unity3d.player.UnityPlayer");
            Method method =classtype.getMethod("UnitySendMessage", String.class,String.class,String.class);
            method.invoke(classtype,gameObjectName,functionName,args);
            return true;
        } catch (ClassNotFoundException e) {
            System.out.println(e.getMessage());
        } catch (NoSuchMethodException e) {
            System.out.println(e.getMessage());
        } catch (IllegalAccessException e) {
            System.out.println(e.getMessage());
        } catch (InvocationTargetException e) {

        }
        return false;
    }

    /*
        检测网络是否可用
     */
    public int CheckNetReachability(){
        if(unityActivity == null)
            getActivity();
       return NetReachability.CheckNetReachability(context);
    }

    /**
     * Toast显示unity发送过来的内容
     * @param content           消息的内容
     * @return                  调用是否成功
     */
    public boolean ShowToast(String content){
        Toast.makeText(getActivity(),content,Toast.LENGTH_SHORT).show();
        //这里是主动调用Unity中的方法，该方法之后unity部分会讲到
        CallUnity("MainCamera","FromAndroid", "hello unity i'm android");
        return true;
    }
}

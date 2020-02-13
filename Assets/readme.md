![example](docs/example.png)

# Custom Actions
```
public class BuildAction_CustomAction : BuildAction
{
    // 字段的数值会被保存 
    public string Msg;

    // 属性的数值不会被保存。非常可能丢失
    public string Msg {get; set;}

    public BuildAction_CustomAction(string msg)
    {
        this.Msg = msg;
    }

    public override BuildState OnUpdate()
    {
        Debug.Log(Msg);
        
        // 状态被设置成Success后，一下次tick会进入下一个任务
        return BuildState.Success;

        // 状态被设置成Failure后，一下次tick会结束任务队列
        // return BuildState.Failure;
    }

    public override BuildProgress GetProgress()
    {
        // 返回空不现实进度条
        return null;

        // 返回具体参数现实进度条
    }
}
```

# BatchModeExample
```
// cmd
"x:\x\Unity.exe" -projectpath "x:\Project" -executeMethod BuildMechineExample.Build -batchmode
```

C#代码中使用`BuildMechine.Run(true)`而不是`BuildMechine.Rune(false)`

# 注意事项
- 内部使用 UnityEngine.JsonUtility。如果自定义BuildAction里边使用Properties和JsonUtility不兼容的List或者Dictionary或者Array。会导致Action的数据丢失。
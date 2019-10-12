using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using R7BehaviorTree;

public class BehaviorTreeDemo : MonoBehaviour
{
    public BehaviorTreeSetting[] BehaviorTreeSettings;

    // Use this for initialization
    void Start()
    {
        //注册Log接收函数
        BehaviorTreeManager.Instance.LogHandler += CollectLogs;

        //通过配置加载行为树
        for (int i = 0; i < BehaviorTreeSettings.Length; i++)
        {
            BehaviorTreeSetting behaviorTreeSetting = BehaviorTreeSettings[i];
            TextAsset textAsset = Resources.Load<TextAsset>(behaviorTreeSetting.Path);
            BehaviorTreeManager.Instance.LoadBehaviorData((int)behaviorTreeSetting.BehaviorTreeType, textAsset.bytes);
        }

        //收集C#所有的Proxy信息
        CSharpProxyManager.Instance.Initalize();

        //初始化lua环境
        gameObject.AddComponent<LuaSetting>().Initalize();

        gameObject.AddComponent<Game>();
    }

    // Update is called once per frame
    private void Update()
    {
        BehaviorTreeManager.Instance.OnUpdate(Time.deltaTime);
    }

    private void CollectLogs(ELogType logType, string msg)
    {
        switch (logType)
        {
            case ELogType.Info:
                Debug.Log(msg);
                break;
            case ELogType.Warnning:
                Debug.LogWarning(msg);
                break;
            case ELogType.Error:
                Debug.LogError(msg);
                break;
        }
    }
}
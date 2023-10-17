using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInitialize : MonoSingleton<GameInitialize>
{
    [Serializable]
    public struct LoadPrefabConfig
    {
        public string name;
        public Vector3 pos;
    }

    public bool ShowFrame;                    //显示帧数
    public int TargetFrame;                   //限定帧数
    public bool ShowDebugGrid;

    [Header("开启更新")] public bool update;
    public List<LoadPrefabConfig> firstLoadPrefabs;
    public List<LoadPrefabConfig> secondLoadPrefabs;
    public List<LoadPrefabConfig> otherLoadPrefabs;

    public event Action GameInitEvent;

    bool gameInit_;

    protected override void Awake()
    {
        base.Awake();
        LogUtil.Log("Log GameInitialize Awake");

        gameInit_ = false;
        Application.targetFrameRate = TargetFrame;
        Application.runInBackground = true;
       
        if (ShowFrame)
        {
            gameObject.AddComponent<FrameRate>();
        }
    }

    private void Start()
    {
        LogUtil.Log("Log GameInitialize Start");
        GameUpdate.Instance.StartGameUpdate(update);
    }

    //进入游戏
    public IEnumerator EnterGame()
    {
        var resourMgr = ResourceManager.Instance;
        int count = 0;
        count = firstLoadPrefabs.Count;
        foreach (var item in firstLoadPrefabs)
        {
            resourMgr.CreatInstanceAsync(item.name, (obj, parma) =>
            {
                LogUtil.Log("Log EnterGame success" + obj + "  " + parma);
                obj.name = item.name;
                obj.transform.localPosition = item.pos;
                count--;
            });
        }

        while (count > 0)
        {
            yield return null;
        }

        count = secondLoadPrefabs.Count;
        foreach (var item in secondLoadPrefabs)
        {
            resourMgr.CreatInstanceAsync(item.name, (obj, parma) =>
            {
                obj.name = item.name;
                obj.transform.localPosition = item.pos;
                count--;
            });
        }

        while (count > 0)
        {
            yield return null;
        }

        count = otherLoadPrefabs.Count;
        foreach (var item in otherLoadPrefabs)
        {
            resourMgr.CreatInstanceAsync(item.name, (obj, parma) =>
            {
                obj.name = item.name;
                obj.transform.localPosition = item.pos;
                count--;
            });
        }

        while (count > 0)
        {
            yield return null;
        }

        LogUtil.Log("11111" + count);

        UIManager.Instance.RegisterListener();
        //CameraController.Instance.RegisterListenner();


        // 加载系统配置 todo
        //ConfigManager.Instance.LoadAllConfigs();
        //yield return new WaitUntil(() => { return ConfigManager.Instance.IsLoaded; });

  
        resourMgr.PreLoads();

        while (!resourMgr.PreLoadFinish)
        {
            yield return null;
        }

        OnGameInit();

        var uiMgr = UIManager.Instance;
        uiMgr.AsyncLoadPreLoadingPanels(SceneType.All);
        while (uiMgr.HasWaite)
        {
            yield return null;
        }
        LogUtil.Log("Game Initialize Pre loading finish!!!!");
        yield return new WaitForEndOfFrame();

        OnIntoGame();
        //LevelManager.Instance.StartLevel(Global.LOGIN_LEVEL_NAME);
    }

    void OnGameInit()
    {
        //_gameInit = true;
        //CacheResource.CheckCacheDir();
        //GameInitEvent?.Invoke();
    }

    void OnIntoGame()
    {
        
    }
}

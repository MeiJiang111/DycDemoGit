using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class GameInitialize : MonoSingleton<GameInitialize>
{
    [Serializable]
    public struct LoadPrefabConfig
    {
        public string name;
        public Vector3 pos;
    }

    public bool ShowFrame;                     //显示帧数
    private int TargetFrame = 60;              //限定帧数
    public bool ShowDebugGrid;

    [Header("开启更新")] public bool update;
    public List<LoadPrefabConfig> firstLoadPrefabs;
    public List<LoadPrefabConfig> secondLoadPrefabs;
    public List<LoadPrefabConfig> otherLoadPrefabs;

    public event Action GameInitEvent;

    bool _gameInit;

    protected override void Awake()
    {
        base.Awake();

        _gameInit = false;
        Application.targetFrameRate = TargetFrame;
        Application.runInBackground = true;
       
        if (ShowFrame)
        {
            gameObject.AddComponent<FrameRate>();
        }
    }

    private void Start()
    {
        var resource = ResourceManager.Instance;
        GameUpdate.Instance.StartGameUpdate(update);
    }

    public IEnumerator EnterGame()
    {
        var resourMgr = ResourceManager.Instance;
        int count = 0;
        count = firstLoadPrefabs.Count;

        foreach (var item in firstLoadPrefabs)
        {
            resourMgr.CreatInstanceAsync(item.name, (obj, parma) =>
            {
                obj.name = item.name;
                obj.transform.localPosition = item.pos;
                count--;
            });
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class GameUpdate:MonoSingleton<GameUpdate>
{
    public enum UpdateState
    {
        None,
        Init,
        VerifyVersion,             //验证
        VerifyVersionSuccess,
        Download,
        Finish,
        Failed,
    }

    public Action<UpdateState> UpdateStateChangedEvent;
    public Action<float> DownLoadProcessChangeEvent;

    UpdateState _state;
    string _lastName;
    string _lastErr;
    List<string> updateCatalogs;
    Coroutine updateCoroution;

    public UpdateState CurState
    {
        get 
        {
            return _state; 
        }

        set
        {
            _state = value;
            UpdateStateChangedEvent?.Invoke(_state);
        }
    }

    protected override void Awake()
    {
        _state = UpdateState.None;
        updateCatalogs = new List<string>();
    }

    void Start()
    {
        
    }

    
    void Update()
    {
        
    }

    public void StartGameUpdate(bool update_ = true)
    {
#if UNITY_EDITOR
        if (!update_)
        {
            CurState = UpdateState.Finish;
            //编辑器下可以跳过更新
            UpdateFinished();
            return;
        }
#endif
        //ResourceManager.Instance.AddressableErrorEvent += OnAddressableErrored;
        //updateCoroution = StartCoroutine(StartGameUpdateImple());
    }

    public void UpdateFinished()
    {
        StartCoroutine(GameInitialize.Instance.EnterGame());
    }
}

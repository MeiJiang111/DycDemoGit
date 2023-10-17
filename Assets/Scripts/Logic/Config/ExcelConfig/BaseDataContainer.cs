using System;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using Newtonsoft.Json;

/*
��������������
*/

[Serializable]
public abstract class BaseDataContainer : ScriptableObject
{

    /// <summary>
    /// ���ض������ļ�
    /// </summary>
    public abstract void Load();


    /// <summary>
    /// ��ö�Ӧ�����ļ���
    /// </summary>
    /// <returns></returns>
    public abstract string GetConfigName();

    public T DeserializeObject<T>(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            return default(T);
        }

        T t = default(T);
        try
        {
            t = JsonConvert.DeserializeObject<T>(json);
        }
        catch (Exception e)
        {
            //LogUtils.LogError(e.StackTrace);
            //throw e;
        }
        return t;
    }

    public virtual void OnLoaded() { }

}

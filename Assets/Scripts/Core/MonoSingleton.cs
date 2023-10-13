using UnityEngine;


/// <summary>
/// 泛型单例基类 任何继承自该类的类，都是单例类
/// </summary>
/// <typeparam name="T">泛型</typeparam>
public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType(typeof(T)) as T;
                if (instance == null) instance = new GameObject("Chinar Single of " + typeof(T).ToString(), typeof(T)).GetComponent<T>();
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null) instance = this as T;
    }

    private void OnApplicationQuit()
    {
        instance = null;
    }
}

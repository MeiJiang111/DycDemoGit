using UnityEngine;


/// <summary>
/// ���͵������� �κμ̳��Ը�����࣬���ǵ�����
/// </summary>
/// <typeparam name="T">����</typeparam>
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

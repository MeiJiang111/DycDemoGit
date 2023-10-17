using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Reflection;
using Newtonsoft.Json;
using UnityEditor;
using System.Text;


public class CreateConfigRes
{
    /// <summary>
    /// 配置文件路径
    /// </summary>
    private static string configPath = Application.dataPath + "/ExcelJson/";

    /// <summary>
    /// 生成的Res文件路径
    /// </summary>
    private static string configResPath = "Assets/ResResources/Config/Excel/";

    /// <summary>
    /// 语言包代码路径
    /// </summary>
    //private static string languageCodePath = "Assets/Scripts/Logic/Config/ExcelConfig/LanID.cs";

    private static Dictionary<string, FileInfo> filesMap = new Dictionary<string, FileInfo>();

    public static Dictionary<string, BaseDataContainer> containers = new Dictionary<string, BaseDataContainer>();

    public static void CreateRes()
    {
        GetAllCongfig(configPath);
        InitContainers();
        SerializationData();
    }

    /// <summary>
    ///  获得所有的config文件
    /// </summary>
    /// <param name="filePath"></param>
    private static void GetAllCongfig(string filePath)
    {
        filesMap.Clear();     //初始化
         
        DirectoryInfo folder = new DirectoryInfo(filePath);
        FileInfo[] chldFiles = folder.GetFiles("*.json");
        if (chldFiles.Length > 0)
        {
            foreach (FileInfo fileInfo in chldFiles)
            {
                filesMap.Add(fileInfo.Name, fileInfo);
            }
        }

        DirectoryInfo[] chldFolders = folder.GetDirectories();
        foreach (DirectoryInfo chldFolder in chldFolders)
        {
            GetAllCongfig(chldFolder.FullName);
        }
    }

    //序列化json到asset
    private static void SerializationData()
    {
        Debug.Log("==== Config文件开始解析，生成Asset文件 ====");
        StringBuilder sb = new StringBuilder();
        foreach (KeyValuePair<string, FileInfo> kv in filesMap)
        {
            var lastWriteTime = kv.Value.LastWriteTime;
            //超过时间不重新生成配置
//             var timeSpan = System.DateTime.Now - lastWriteTime;
//             if (timeSpan.TotalMinutes > 60)
//             {
//                 continue;
//             }
            Debug.Log("解析配置文件:" + kv.Key);
            sb.Length = 0;
            string typeName = kv.Key.Replace(".json", "") + "Container";
            BaseDataContainer container = null;
            containers.TryGetValue(typeName, out container);
            if (container != null)
            {
                string line = "";
                using (StreamReader sr = new StreamReader(kv.Value.FullName))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        sb.Append(line);
                    }
                }
                BaseDataContainer obj = null;
                try
                {
                    obj = (BaseDataContainer)JsonConvert.DeserializeObject(sb.ToString(), container.GetType());
                }
                catch (System.Exception ex)
                {
                    Debug.LogErrorFormat("json {0} error! by ex {1}", kv.Key, ex.ToString());
                }

                if (!Directory.Exists(configResPath))
                {
                    Directory.CreateDirectory(configResPath);
                }
                container = obj;
                string outPath = configResPath + container.GetConfigName() + ".asset";
                AssetDatabase.DeleteAsset(outPath);
                AssetDatabase.CreateAsset(container, outPath);
                AssetDatabase.SaveAssets();
                InitLanguageCode(obj);
            }
        }
        AssetDatabase.Refresh();
        Debug.Log("====Config文件解析，生成Asset文件完成 ==== ");
    }

    
    private static void InitContainers()
    {
        containers.Clear();
        List<Type> types = new List<Type>();
        foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (typeof(BaseDataContainer).IsAssignableFrom(type))
                {
                    if (type.IsClass && !type.IsAbstract)
                    {
                        types.Add(type);
                    }
                }
            }
        }
        //加载配置表
        foreach (Type type in types)
        {
            CreateContainer(type);
        }


    }

    /// <summary>
    /// 创建容器
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="type"></param>
    /// <returns></returns>
    public static void CreateContainer(Type type)
    {
        BaseDataContainer instance = (BaseDataContainer)ScriptableObject.CreateInstance(type);
        containers.Add(type.Name, instance);
    }

    private static void InitLanguageCode(BaseDataContainer container)
    {
        //if(!(container is GlobalStringContainer))
        //{
        //    return;
        //}
        //GlobalStringContainer con = container as GlobalStringContainer;

        //if (File.Exists(languageCodePath))
        //    File.Delete(languageCodePath);

        //FileStream file = File.Open(languageCodePath, FileMode.OpenOrCreate);
        //StringBuilder sb = new StringBuilder(1024);
        //sb.AppendLine("class LanID");
        //sb.AppendLine("{");
        //foreach(var bean in con.dataList)
        //{
        //    sb.AppendLine("   /// <summary>");
        //    sb.Append("   /// ");
        //    sb.AppendLine(bean.ZH_TW);
        //    sb.AppendLine("   /// </summary>");
        //    sb.AppendLine(string.Format("   public const int ID_{0}={1};", bean.Num, bean.Num));
        //    sb.AppendLine();
        //}
        //sb.Append("}");
        //byte[] data = System.Text.Encoding.UTF8.GetBytes(sb.ToString());
        //file.Write(data,0,data.Length);
        //file.Close();
    }
}

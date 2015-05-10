using UnityEngine;
using System.Collections;
using System.IO;

public class IOAction
{
    #region 单例
    private static readonly IOAction instance = new IOAction();

    private IOAction() { }

    public static IOAction GetInstance()
        {
               return instance;
        }
    #endregion

    /// <summary>
    /// 返回出入路径下的全部文件夹列表
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string[] getAllDirectory(string path)
    {
        return Directory.GetDirectories(path);
    }
}

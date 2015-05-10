using UnityEngine;
using System.Collections;
using System.IO;

public class WebAction {
    static public WWW www;
    #region 单例
    private static readonly WebAction instance = new WebAction();

    private WebAction() { }

    public static WebAction GetInstance()
        {
               return instance;
        }
    #endregion

    /// <summary>
    /// 下载图片方法
    /// </summary>
    /// <param name="link">图片地址</param>
    /// <param name="path">保存路径（包含文件名）</param>
    /// <returns></returns>
    static public IEnumerator getNetImage(string link, string path)
    {
        www = new WWW(link);
        Texture2D getTexture;
        yield return www;
        getTexture = www.texture;

        byte[] _texturebyte = getTexture.EncodeToPNG();
        File.WriteAllBytes(path, _texturebyte);
    }
}

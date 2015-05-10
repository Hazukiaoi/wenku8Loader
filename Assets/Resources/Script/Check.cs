using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Net;
using System.IO;

public class Check : MonoBehaviour {

    public RawImage ri;
    Texture2D tex2d;
    string link = @"http://img.wenku8.cn/image/1/1846/1846s.jpg";
    string path;

    Stream  streamImage;


	// Use this for initialization
	void Start () {
        path = Application.dataPath + "/Resources/Temp/1846s.png";
        getTex();
	}
    private void getTex()
    {

        System.Net.WebClient webClient = new System.Net.WebClient();
        webClient.Headers.Set("User-Agent", "Microsoft Internet Explorer");//设置头部

        try
        {
            /*streamImage =*/ webClient.OpenRead(link);
            webClient.DownloadFile(link, path);
            //streamImage.Dispose();
        }
        catch
        {
            Debug.Log("error");
        }
    }
}

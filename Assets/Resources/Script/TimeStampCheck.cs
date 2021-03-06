using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Diagnostics;
using expandMath;
using programType;

public class TimeStampCheck : MonoBehaviour {

    [DllImport("TimeStamp")]
    private static extern long GetCycleCount();

    [DllImport("TimeStamp")]
    public static extern int GetFrequency();

    //[DllImport("DLL")]

	//public static extern int MyADD(int x, int y);
	long s;
	string log;
	Polar p;
	public int runtime = 100;

	void Start(){
		long start, end;
		Vector3 v3position;
		Vector2 v2position;
		int frequency = GetFrequency ();

		//Quaternion方法
		log += runtime + "次的Quaternion: \n\r";
		for(int i = 0;i < runtime;i++){
			p = new Polar(Random.Range(0,360),Random.Range(0,500));
			start = GetCycleCount();
			//转换代码
			v3position = p.Polar2Vector3(Vector3.up);

			end = GetCycleCount();
			log += i + ":\t" + v3position + "\t ;time:" +(double)(end - start)/frequency + "\n\r"; 
		}
		log += "\n\r";

		//Sincos方法
		log += runtime + "次的sin&cos: \n\r";
		for(int i = 0;i < runtime;i++){
			p = new Polar(Random.Range(0,360),Random.Range(0,500));
			start = GetCycleCount();
			//转换代码
			v2position = p.Polar2Vector2();

			end = GetCycleCount();
			log += i + ":\t" + v2position + "\t ;time:" + (double)(end - start)/frequency + "\n\r"; 
		}
		System.IO.File.WriteAllText (@"C:\RunLog.txt", log);
	}

	/*void Start () {
        long start,end;


        int frequency = GetFrequency();
        start = GetCycleCount();
        //for (long i = 0; i < 999999999; i++)
        //{
        //    s++;
        //}
        s = MyADD(1, 1);
        end = GetCycleCount();

        UnityEngine.Debug.Log((double)(end-start)/frequency + " , " + s);
	}*/


	
}

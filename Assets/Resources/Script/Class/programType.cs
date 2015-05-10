 using UnityEngine;
using System.Collections;
namespace programType{
	//触摸类型
	public enum touchType{
		relase,
		drop,
		move,
		longpress,
		touchend
	}
	//页面类型
	public enum pageType{
		HotToday = 0,
		TheNew = 1,
		HotNew = 2,
		WeekList = 3,
		Seach = 4
	}
	//极坐标
	public class Polar{
		private float _angle;
		private float _distance;
		public Polar(){
			_angle = 0;
			_distance = 0;
		}
		public Polar(float Angle,float Distance){
			_angle = Angle;
			_distance = Distance;
		}
		public float Angle{
			get{return _angle;}
			set{_angle = value;}
		}
		public float Distance{
			get{return _distance;}
			set{_distance = value;}
		}

	}
}
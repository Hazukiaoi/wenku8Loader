using UnityEngine;
using System.Collections;
using programType;

namespace expandMath{
	public static class ExpandMath{
//		public static Vector2 operator*(Vector2 v1,Vector2 v2){ 
//			return new Vector2 (v1.x * v2.x, v1.y, v2.y);
//		}
		public static Vector2 V2Multi(this Vector2 v2_1,Vector2 v2_2){
			return new Vector2 (v2_1.x * v2_2.x, v2_1.y * v2_2.y);		
		}

//		public static Vector2 V2Add(this Vector2 v2_1,Vector2 v2_2){
//			return new Vector2 (v2_1.x + v2_2.x, v2_1.y + v2_2.y);
//		}
//		public static Vector2 v2MultiFloat(this Vector2 v2,float f){
//			return new Vector2 (v2.x * f, v2.y * f);
//		}

		/// <summary>
		/// Vector2 to polar.
		/// </summary>
		/// <returns>Polar.</returns>
		/// <param name="v2">V2.</param>
		public static Polar V2toPolar(this Vector2 v2){
//			float Angle = Mathf.Atan (v2.y / v2.x);
//			float Distance = Mathf.Sqrt (v2.x * v2.x + v2.y + v2.y);
			return new Polar (Mathf.Atan (v2.y / v2.x),Mathf.Sqrt (v2.x * v2.x + v2.y + v2.y));
		}



		/// <summary>
		/// Polar2s the vector2.极坐标转直角坐标(四元素方法，Vector3坐标转换,v3为绕轴)
		/// </summary>
		/// <returns>The vector2.</returns>
		/// <param name="p">P.</param>
		/// <param name="v3">V3.</param>
		public static Vector3 Polar2Vector3(this Polar p,Vector3 v3){
			Vector3 position = new Vector3 (p.Distance,0,0);
			position = Quaternion.AngleAxis (p.Angle, v3) * position;
			//return new Vector2 (position.x, position.y);
			return position;
		}
		/// <summary>
		/// Polar2s the vector2.极坐标转直角坐标(角度换算方法)
		/// </summary>
		/// <returns>The vector2.</returns>
		/// <param name="p">P.</param>
		public static Vector2 Polar2Vector2(this Polar p){
			float radian = p.Angle * Mathf.PI / 180;
			Vector2 position = new Vector2 (p.Distance*Mathf.Cos(radian),p.Distance*Mathf.Sin(radian));
			return position;
		}
		public static string ToString2 (this Polar p){
			string s = "(Angle " + p.Angle + ";Distance " + p.Distance + ")";
			return s; 
		}
	}
}
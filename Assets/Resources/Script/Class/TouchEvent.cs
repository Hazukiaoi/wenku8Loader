using UnityEngine;
using System.Collections;
using programType;

public class TouchEvent {
	private TouchEvent(){}
	static public readonly TouchEvent instance = new TouchEvent ();
	
	static float longPressTime = 0.2f;//进入长按的时间
	static bool drop = false;
	static bool move = false;
	static touchType tP;
    static float staticSize = 0.2f; //判断为长按的最大偏移半径
	/// <summary>
	/// Touchs the event.
	/// </summary>
	/// <returns>The event.</returns>
	/// <param name="t">T.</param>
	/// <param name="presstime">Presstime.</param>
	static public touchType tEvent(Touch t,ref float presstime){
        if (Mathf.Abs(t.deltaPosition.x) > staticSize && Mathf.Abs(t.deltaPosition.y) > staticSize)
        {
			if (drop){
//				拖动状态
				tP = touchType.drop;
			}else{
//				移动状态
				tP = touchType.move;
				move = true;
			}
		}
        if (Mathf.Abs(t.deltaPosition.x) < staticSize && Mathf.Abs(t.deltaPosition.y) < staticSize && move == false && drop == false)
        {
//			按压时间大于0.3秒的时候进入长按状态
			presstime += Time.deltaTime;
			if(presstime > longPressTime){
				tP = touchType.longpress;
				drop = true;
			}
		}
		if(t.phase == TouchPhase.Ended){
//			松手时候如果按压时间少于长按时间就是单击
			if(presstime < longPressTime){
				tP = touchType.relase;
			}else{
//				否则就是结束触摸
				tP = touchType.touchend;
			}
//			重置状态
			drop = false;
			move = false;
			presstime = 0.0f;
		}
		return tP;
	}
}

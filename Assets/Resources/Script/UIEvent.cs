//#define DEBUG
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using programType;
using expandMath;

public class UIEvent : MonoBehaviour
{
    #region 定义
    public Text MenuTitle;
    public Image hotboxCenter;//热盒中心
    public Image hotboxTarget;//热盒目标


    //热盒菜单图标
    public Image Icon_HotToday;
    public Image Icon_TheNew;
    public Image Icon_HotNew;
    public Image Icon_WeekList;
    public Image Icon_Seach;
    public Image Icon_Back;


    //滑出速度；
    public float outSpeed = 10;
    public AnimationCurve outAniCurve;

    Image[] AllHotBoxIconList;//列表
    List<Image> NowHotBoxIconList = new List<Image>();//所在页面的热盒列表
    List<Vector2> NowHotBoxIconTargetPositionList = new List<Vector2>();//热盒图标对应位置列表
    struct HotBoxIconData
    {
        public string name { get; set; }
        public Vector2 position { get; set; }
        public HotBoxIconData(string _name, Vector2 _position)
        {
            name = _name;
            position = _position;
        }
    } //记录图标名与坐标的结构体
    List<HotBoxIconData> HotBoxDistanceList = new List<HotBoxIconData>();//用于计算与手指触摸点距离的列表
    Image SelectionIcon;//用于记录选中的对象


    float pressTime = 0;//获取按压时间
    touchType getTouchType;//获取触摸类型
    Vector2 screenScale; //屏幕缩放比例
    Vector2 ScreenSize; //屏幕尺寸
    Vector2 Halfscreen; //一半的屏幕

    public float hotBoxMenuIconDistancePresent = 0.2f;//热盒菜单滑出距离对横屏百分比
    float hotBoxMenuIconDistance;//实际滑出距离
    public float hotBoxMenuIconAngle = 22.5f;//热盒菜单滑出角度
    Vector2 hotBoxCenterPosition = new Vector2(-1000,-1000);//中心位置
    Vector2 hotBoxTagPosition = new Vector2(-1000,-1000);//手指位置
    Vector2 oneScale = new Vector2(1, 1);//一倍放大
    Vector2 oneFiveScale = new Vector2(1.5f, 1.5f);//1.5倍放大
    Vector2 notUseIconPosition = new Vector2(-1000, -1000);//储存不需要使用的图标位置

    Color LerpColorStart, lerpColorEnd;

    bool isOut = true;//判断是滑出还是滑入状态
    bool isRight = true;//判断是左边还是右边
    bool isHotMenuClose = false;//判断是否处于热盒菜单收回状态
    bool isSelection = false;//判断是否处于选中某个按钮的状态

    Touch touchPoint;//获取触摸点
    bool hotBoxIconMove = false;//图标是否需要滑出
    float iconMoveT = 0;

    static public Action dropAction;
    #endregion

    //启动程序自动处理初始数据
    void Awake()
    {
        ScreenSize = new Vector2(Screen.width, Screen.height);
        Halfscreen = ScreenSize * 0.5f;
        screenScale = new Vector2(1080 / ScreenSize.x, 1920 / ScreenSize.y);

        AllHotBoxIconList = new Image[6]{//此处排序必须与pageType一致，back为最后一项
            Icon_HotToday,
            Icon_TheNew,
            Icon_HotNew,
            Icon_WeekList,
            Icon_Seach,
            Icon_Back
        };

        LerpColorStart = new Color(1, 1, 1, 0);
        lerpColorEnd = new Color(1, 1, 1, 1);

        NowHotBoxIconList = new List<Image>();//建立页面对应图标列表

        hotBoxMenuIconDistance = hotBoxMenuIconDistancePresent * ScreenSize.y * screenScale.y;//获得自适应后的热盒图标距离

        for (int i = 0; i < AllHotBoxIconList.Length; i++)
        {
            AllHotBoxIconList[i].gameObject.SetActive(false);
            AllHotBoxIconList[i].canvasRenderer.SetColor(LerpColorStart);
        }   

    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            touchPoint = Input.GetTouch(0);
            getTouchType = TouchEvent.tEvent(touchPoint, ref pressTime);
            switch (getTouchType)
            {
                #region drop
                case touchType.drop:
                    MenuTitle.text = "drop";
                    if (iconMoveT < 1 && iconMoveT > 0)
                    {
                        hotBoxIconMove = true;//允许插值；
                    }

                    if (SelectionIcon != null)
                    {
                        SelectionIcon.rectTransform.localScale = oneScale;
                    }

                    hotBoxTagPosition = hotboxTarget.rectTransform.anchoredPosition = screenScale.V2Multi(touchPoint.position - Halfscreen);
                   
                    HotBoxDistanceList.Sort
                           ( 
                           (HotBoxIconData i1, HotBoxIconData i2) =>
                           {
                               return Vector2.Distance(i1.position, hotBoxTagPosition).CompareTo(Vector2.Distance(i2.position, hotBoxTagPosition));
                           }
                           );//根据每个图标距离手指位置排序
                    SelectionIcon = NowHotBoxIconList.Find((Image i) => { return i.name == HotBoxDistanceList[0].name; });//找出排序后在列表上第一个的对象

                    if (SelectionIcon != null)
                    {
                        SelectionIcon.rectTransform.localScale = oneFiveScale;     //放大图标
                    }

                    break;
                #endregion
                #region longpress
                case touchType.longpress:
                    MenuTitle.text = "longpress";
                    isHotMenuClose = false; //设置热盒菜单为未关闭状态
                    if (HotBoxDistanceList.Count > 0)
                    {
                        HotBoxDistanceList.Clear();
                    }

                    for (int i = 0; i < NowHotBoxIconList.Count; i++)   //激活全部菜单图标
                    {
                        NowHotBoxIconList[i].gameObject.SetActive(true);
                    }

                    hotBoxTagPosition = hotBoxCenterPosition = screenScale.V2Multi(touchPoint.rawPosition - Halfscreen);//设置中心点位置
                    hotboxCenter.rectTransform.anchoredPosition = hotBoxCenterPosition;//将中心点摆到对应位置

                    if (hotBoxCenterPosition.x > 0)  //判断左右
                    {
                        isRight = true;

                    }
                    else
                    {
                        isRight = false;
                    }

                    setNowHotBoxList(ProgramData.instance.NowPage, hotBoxTagPosition, isRight);  //设置当前页面的热盒图标位置

                    for (int i = 0; i < NowHotBoxIconList.Count; i++)//把当前每一个图标的名字与位置数据存入列表用于比较
                    {
                        HotBoxDistanceList.Add(new HotBoxIconData(NowHotBoxIconList[i].name, NowHotBoxIconTargetPositionList[i]));
                    }

                    hotBoxIconMove = true;//允许插值；
                    isOut = true;//设置为向外运动

                    break;
                #endregion

                #region move
                case touchType.move:
                    MenuTitle.text = "Move";
                    //if (dropAction != null)
                    //{
                    //    dropAction();
                    //}

                    break;
                #endregion

                #region relase
                case touchType.relase:
                    MenuTitle.text = "Relase";
                    break;
                case touchType.touchend:
                    MenuTitle.text = "touchEnd";
                    hotboxTarget.rectTransform.anchoredPosition = notUseIconPosition;
                    hotboxCenter.rectTransform.anchoredPosition = notUseIconPosition;

                    if (SelectionIcon != null)
                    {
                        if (SelectionIcon == Icon_Back)
                        {
                            MenuTitle.text = "back";
                        }
                        else
                        {
                            pageType pt = pageType.HotToday;
                            if (SelectionIcon == Icon_HotToday)
                            {
                                pt = pageType.HotToday;
                            }
                            else if (SelectionIcon == Icon_TheNew)
                            {
                                pt = pageType.TheNew;
                            }
                            else if (SelectionIcon == Icon_HotNew)
                            {
                                pt = pageType.HotNew;
                            }
                            else if (SelectionIcon == Icon_WeekList)
                            {
                                pt = pageType.WeekList;
                            }
                            else if (SelectionIcon == Icon_Seach)
                            {
                                pt = pageType.Seach;
                            }
                            ProgramData.instance.NowPage = pt;
                            MenuTitle.text = pt.ToString();
                        }
                    }
                    

                    hotBoxIconMove = true;//允许插值；
                    isOut = false; //设置图标状态为收回
                    if(HotBoxDistanceList.Count > 0)
                    {
                        HotBoxDistanceList.Clear();
                    }
                    break;
                #endregion
            }

        }
        if (hotBoxIconMove)
        {                                        //当允许被插值的时候则插值
            lerpToPosition(hotBoxTagPosition, ref iconMoveT, isOut);
        }

        //if (iconMoveT <= 0 && isHotMenuClose == false && SelectionIcon != null)//当菜单图标全部被收起同时热盒菜单处于打开状态同时有选中对象的时候
        //{
        //    SelectionIcon.rectTransform.localScale = oneScale;
        //    SelectionIcon.rectTransform.anchoredPosition = notUseIconPosition;
        //    SelectionIcon = null;
        //    isHotMenuClose = true;
        //}
            
    }
    //设置图标和坐标位置
    void setNowHotBoxList(pageType pt, Vector2 centerPosition, bool isRight)
    {
        NowHotBoxIconList.Clear();
        NowHotBoxIconTargetPositionList.Clear();
        int Num = 0;//执行到的序号
        for (int i = 0; i < AllHotBoxIconList.Length - 1; i++)
        {
            if ((int)pt != i)
            {//添加所在页面图标到列表，排除所在页面
                NowHotBoxIconList.Add(AllHotBoxIconList[i]);
                //判断左右
                float angle = 0;
                if (isRight)
                {
                    angle = (90 - hotBoxMenuIconAngle * Num) + 90;
                }
                else
                {
                    angle = hotBoxMenuIconAngle * Num;
                }
                Polar p = new Polar(angle, hotBoxMenuIconDistance);
                NowHotBoxIconTargetPositionList.Add(centerPosition + p.Polar2Vector2());
                Num++;
            }
        }
        NowHotBoxIconList.Add(AllHotBoxIconList[5]);
        Polar _p = new Polar(90, hotBoxMenuIconDistance);
        NowHotBoxIconTargetPositionList.Add(centerPosition + _p.Polar2Vector2());
    }

    //插值移动到目标
    void lerpToPosition(Vector2 centerPosition, ref float t, bool isOut)
    {
        for (int i = 0; i < NowHotBoxIconList.Count; i++)
        {
            NowHotBoxIconList[i].rectTransform.anchoredPosition = Vector2.Lerp(centerPosition,
                                                                               NowHotBoxIconTargetPositionList[i],
                                                                               outAniCurve.Evaluate(t)
                                                                               );
            NowHotBoxIconList[i].canvasRenderer.SetColor(Color.Lerp(LerpColorStart, lerpColorEnd, outAniCurve.Evaluate(t)));
        }
        if (isOut)
        {
            t += Time.deltaTime * outSpeed;
        }
        else
        {
            t -= Time.deltaTime * outSpeed;
        }

        //限制范围在0-1之间
        if (t >= 1)
        {
            hotBoxIconMove = false;
            t = 1;
        }
        if (t <= 0)
        {
            hotBoxIconMove = false;
            t = 0;
            for (int i = 0; i < NowHotBoxIconList.Count; i++)
            {
                NowHotBoxIconList[i].canvasRenderer.SetColor(LerpColorStart);
                NowHotBoxIconList[i].gameObject.SetActive(false);
            }
        }
    }
}
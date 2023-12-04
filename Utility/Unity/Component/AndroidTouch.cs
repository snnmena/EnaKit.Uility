using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Yoziya
{
    public class AndroidTouch : MonoBehaviour
    {

        private int isforward;//标记摄像机的移动方向
                              //记录两个手指的旧位置
        private Vector2 oposition1 = new Vector2();
        private Vector2 oposition2 = new Vector2();

        Vector2 m_screenPos = new Vector2(); //记录手指触碰的位置
        Vector3 m_screenPos3 = new Vector3();

        //用于判断是否放大
        bool isEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
        {
            //函数传入上一次触摸两点的位置与本次触摸两点的位置计算出用户的手势
            float leng1 = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
            float leng2 = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));
            if (leng1 < leng2)
            {
                //放大手势
                return true;
            }
            else
            {
                //缩小手势
                return false;
            }
        }

        void Start()
        {
            Input.multiTouchEnabled = true;//开启多点触碰
        }

        void Update()
        {
            // 单点触碰移动摄像机
            if (Input.GetMouseButtonDown(0))
            {
                m_screenPos3 = Input.mousePosition;   //记录鼠标刚触碰的位置
            }
            if (Input.GetMouseButton(0)) //鼠标在屏幕上移动，移动摄像机
            {
                Vector3 deltaPosition = Input.mousePosition - m_screenPos3; //计算鼠标的移动距离
                m_screenPos3 = Input.mousePosition; // 更新鼠标当前的位置

                //由于鼠标的移动速度相对于屏幕可能比较快，因此可能需要调整下面这行代码中的缩放因子，使得摄像机的移动速度适中
                transform.Translate(new Vector3(deltaPosition.x * Time.deltaTime, deltaPosition.y * Time.deltaTime, 0));
            }
            if (Input.touchCount <= 0)
                return;
            if (Input.touchCount == 1) //单点触碰移动摄像机
            {
                if (Input.touches[0].phase == TouchPhase.Began)
                    m_screenPos = Input.touches[0].position;   //记录手指刚触碰的位置
                if (Input.touches[0].phase == TouchPhase.Moved) //手指在屏幕上移动，移动摄像机
                {
                    transform.Translate(new Vector3(Input.touches[0].deltaPosition.x * Time.deltaTime, Input.touches[0].deltaPosition.y * Time.deltaTime, 0));
                }
            }

            else if (Input.touchCount > 1)//多点触碰
            {
                //记录两个手指的位置
                Vector2 nposition1 = new Vector2();
                Vector2 nposition2 = new Vector2();

                //记录手指的每帧移动距离
                Vector2 deltaDis1 = new Vector2();
                Vector2 deltaDis2 = new Vector2();

                for (int i = 0; i < 2; i++)
                {
                    Touch touch = Input.touches[i];
                    if (touch.phase == TouchPhase.Ended)
                        break;
                    if (touch.phase == TouchPhase.Moved) //手指在移动
                    {

                        if (i == 0)
                        {
                            nposition1 = touch.position;
                            deltaDis1 = touch.deltaPosition;
                        }
                        else
                        {
                            nposition2 = touch.position;
                            deltaDis2 = touch.deltaPosition;

                            if (isEnlarge(oposition1, oposition2, nposition1, nposition2)) //判断手势伸缩从而进行摄像机前后移动参数缩放效果
                                isforward = 1;
                            else
                                isforward = -1;
                        }
                        //记录旧的触摸位置
                        oposition1 = nposition1;
                        oposition2 = nposition2;
                    }
                    //移动摄像机
                    Camera.main.transform.Translate(isforward * Vector3.forward * Time.deltaTime * (Mathf.Abs(deltaDis2.x + deltaDis1.x) + Mathf.Abs(deltaDis1.y + deltaDis2.y)));
                }
            }
        }
    }
}

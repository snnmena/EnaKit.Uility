using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;

namespace Yoziya.manjuu
{
    public class StartProcedure : Procedure
    {
        public override void Enter()
        {
            Debug.Log("开始游戏");
            // 加载闪屏动画
            
            // 给动画结束添加监听，让动画结束进入检测是否需要更新的界面
        }

        public override void Exit()
        {
            Debug.Log("结束");
        }

        public override void Update()
        {
            
        }
    }
}

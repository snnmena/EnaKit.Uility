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
            Debug.Log($"开始游戏（{this.GetType().Name}）");
            // 加载闪屏动画
            ResourceManager.LoadAsset("Start", delegate { });
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

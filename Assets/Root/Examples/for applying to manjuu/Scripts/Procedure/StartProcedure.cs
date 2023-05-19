//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using static UnityEngine.UI.Image;

//namespace Yoziya.manjuu
//{
//    public class StartProcedure : AbstractProcedure
//    {
//        public override async void Enter()
//        {
//            Debug.Log($"开始游戏（{this.GetType().Name}）");
//            // 加载闪屏动画
//            GameObject a = await ResourceManager.Load<GameObject>("Start"/*, delegate { }*/);
//            Manjuu.SetUI<StartController>(a);
//        }
//                public override void Exit()
//        {
//            Debug.Log("结束");
//        }

//        public override void Update()
//        {
            
//        }
//    }
//}

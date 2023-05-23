using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yoziya.manjuu
{
    public class InputSystem : Mode
    {
        protected override void Init()
        {
            
        }
        public static event Action KeyAction = delegate { };
        public static event Action MouseAction = delegate { };

        public void Update()
        {
            // 检查键盘输入
            if (UnityEngine.Input.anyKeyDown)
            {
                KeyAction.Invoke();
            }

            // 检查鼠标输入
            if (UnityEngine.Input.GetMouseButtonDown(0) || UnityEngine.Input.GetMouseButtonDown(1) || UnityEngine.Input.GetMouseButtonDown(2))
            {
                MouseAction.Invoke();
            }
        }
    }
}

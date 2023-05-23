using System.Collections;
using UnityEngine;

namespace Yoziya.manjuu
{
    public class Loop : MonoBehaviour,IController
    {
        private InputSystem mInput;
        public IApp GetApp()
        {
            return Manjuu.Interface;
        }

        public void Initialize()
        {
            throw new System.NotImplementedException();
        }

        void Start()
        {
            mInput = this.GetMode<InputSystem>();
        }

        void FixedUpdate()
        {
            mInput.Update();
        }
    }
}
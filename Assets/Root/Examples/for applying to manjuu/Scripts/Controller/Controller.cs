using System.Collections;
using UnityEngine;

namespace Yoziya.manjuu
{
    public abstract class Controller : MonoBehaviour, IController
    {
        public virtual void Initialize()
        {

        }
        void Start()
        {
            Register();
        }
        public IApp GetApp()
        {
            return Manjuu.Interface;
        }

        private void OnDestroy()
        {
            UnRegister();
        }
        abstract protected void Register();
        abstract protected void UnRegister();
    }
}
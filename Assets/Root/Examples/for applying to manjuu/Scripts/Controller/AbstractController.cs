using System.Collections;
using UnityEngine;

namespace Yoziya.manjuu
{
    public abstract class AbstractController : MonoBehaviour, IController
    {
        private IOCContainer mContainer = new IOCContainer();
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
        abstract public void Register();
        abstract public void UnRegister();
    }
}
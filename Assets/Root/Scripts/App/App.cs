using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yoziya
{
    public class App : MonoBehaviour
    {
        private IOCContainer mContainer;
        private HashSet<ISystem> mSystems = new HashSet<ISystem>();
        private HashSet<IModel> mModels = new HashSet<IModel>();
        private ProcedureManager mProcedureManager = new ProcedureManager();
        protected virtual void Awake()
        {
            // App 子类中覆写和配置 Model、System、Procedure
        }
        protected virtual void Start()
        {
            foreach (ISystem system in mSystems)
            {
                system.Initialize();
            }

            foreach (IModel model in mModels)
            {
                model.Initialize();
            }
        }
        private void Update()
        {

        }
        private void FixedUpdate()
        {
            mProcedureManager.UpdateProcedure();
        }
        
        public void ChangeProcedure(IProcedure procedure)
        {
            mProcedureManager.ChangeProcedure(procedure);
        }
    }

    #region IOC

    public class IOCContainer
    {
        private Dictionary<Type, object> mInstances = new Dictionary<Type, object>();

        public void Register<T>(T instance)
        {
            var key = typeof(T);

            if (mInstances.ContainsKey(key))
            {
                mInstances[key] = instance;
            }
            else
            {
                mInstances.Add(key, instance);
            }
        }

        public T Get<T>() where T : class
        {
            var key = typeof(T);

            if (mInstances.TryGetValue(key, out var retInstance))
            {
                return retInstance as T;
            }

            return null;
        }
    }

    #endregion
}

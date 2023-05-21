using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yoziya
{
    public interface IApp
    {
        void RegisterState<T>(T state) where T : IState;
        void RegisterMode<T>(T mode) where T : IMode;
        T GetState<T>() where T : class, IState;
        T GetMode<T>() where T : class, IMode;
    }
    public abstract class App<T> : IApp where T : App<T>, new()
    {
        public static Action<T> OnRegisterPatch = architecture => { };
        private static T mApp;
        private HashSet<IState> mStatesCache = new HashSet<IState>();
        private HashSet<IMode> mModesCache = new HashSet<IMode>();
        private IOCContainer mContainer = new IOCContainer();
        public static IApp Interface
        {
            get
            {
                if (mApp == null)
                {
                    mApp = new T();
                    mApp.Initialize();
                    OnRegisterPatch?.Invoke(mApp);
                    foreach (var architectureModel in mApp.mStatesCache)
                    {
                        architectureModel.Initialize();
                    }
                    mApp.mStatesCache.Clear();
                    foreach (var architectureSystem in mApp.mModesCache)
                    {
                        architectureSystem.Initialize();
                    }
                    mApp.mModesCache.Clear();
                }
                return mApp;
            }
        }

        public void RegisterState<T>(T state) where T : IState
        {
            state.SetArchitecture(this);
            mContainer.Register<T>(state);
            mStatesCache.Add(state);
        }
        public void RegisterMode<T>(T mode) where T : IMode
        {
            mode.SetArchitecture(this);
            mContainer.Register<T>(mode);
            mModesCache.Add(mode);
        }
        public T GetState<T>() where T : class, IState
        {
            return mContainer.Get<T>();
        }
        public T GetMode<T>() where T : class, IMode
        {
            return mContainer.Get<T>();
        }
        protected abstract void Initialize();
    }

    /// <summary>
    /// 数据层、状态层
    /// 能发送事件
    /// </summary>
    #region State

    public interface IState : IBelongToApp, ICanSetApp
    {
        void Initialize();
    }

    public abstract class State : IState
    {
        private IApp mApp;
        IApp IBelongToApp.GetApp()
        {
            return mApp;
        }
        void ICanSetApp.SetArchitecture(IApp app)
        {
            mApp = app;
        }
        void IState.Initialize()
        {
            OnInit();
        }
        protected abstract void OnInit();
    }

    #endregion

    /// <summary>
    /// 定义游戏规则
    /// 能获取其他Mode、State，能订阅和发送事件
    /// </summary>
    #region Mode

    public interface IMode : IBelongToApp, ICanSetApp, ICanGetMode
    {
        void Initialize();
    }

    public abstract class Mode : IMode
    {
        private IApp mApp;
        IApp IBelongToApp.GetApp()
        {
            return mApp;
        }
        void ICanSetApp.SetArchitecture(IApp app)
        {
            mApp = app;
        }
        void IMode.Initialize()
        {
            OnInit();
        }
        protected abstract void OnInit();
    }

    #endregion

    /// <summary>
    /// 定义游戏对象，以及它能做什么，能订阅事件，控制游戏对象在收到什么事件时执行Command
    /// </summary>
    #region Controller

    public interface IController : ICanGetMode, ICanGetState
    {
        void Initialize();
    }

    #endregion

    #region Solution

    public interface ISolution
    {

    }

    #endregion

    #region Rule

    public interface IBelongToApp
    {
        IApp GetApp();
    }
    public interface ICanSetApp
    {
        void SetArchitecture(IApp app);
    }

    public interface ICanGetState : IBelongToApp { }
    public interface ICanGetMode : IBelongToApp { }

    public static class RuleExtend
    {
        public static T GetState<T>(this ICanGetState self) where T : class, IState
        {
            return self.GetApp().GetState<T>();
        }

        public static T GetMode<T>(this ICanGetMode self) where T : class, IMode
        {
            return self.GetApp().GetMode<T>();
        }
    }

    #endregion

    #region Event

    public interface IEvent
    {

    }

    public class TypeEventSystem
    {
        private readonly Events mEvents = new Events();


        public static readonly TypeEventSystem Global = new TypeEventSystem();

        public void Send<T>() where T : new()
        {
            mEvents.GetEvent<Event<T>>()?.Trigger(new T());
        }

        public void Send<T>(T e)
        {
            mEvents.GetEvent<Event<T>>()?.Trigger(e);
        }
    }

    public class Event : IEvent
    {
        private Action mOnEvent = () => { };

        public void Trigger()
        {
            mOnEvent?.Invoke();
        }
    }

    public class Event<T> : IEvent
    {
        private Action<T> mOnEvent = e => { };

        public void Trigger(T t)
        {
            mOnEvent?.Invoke(t);
        }
    }

    public class Events
    {
        private static Events mGlobalEvents = new Events();

        public static T Get<T>() where T : IEvent
        {
            return mGlobalEvents.GetEvent<T>();
        }


        public static void Register<T>() where T : IEvent, new()
        {
            mGlobalEvents.AddEvent<T>();
        }

        private Dictionary<Type, IEvent> mTypeEvents = new Dictionary<Type, IEvent>();

        public void AddEvent<T>() where T : IEvent, new()
        {
            mTypeEvents.Add(typeof(T), new T());
        }

        public T GetEvent<T>() where T : IEvent
        {
            IEvent e;

            if (mTypeEvents.TryGetValue(typeof(T), out e))
            {
                return (T)e;
            }

            return default;
        }

        public T GetOrAddEvent<T>() where T : IEvent, new()
        {
            var eType = typeof(T);
            if (mTypeEvents.TryGetValue(eType, out var e))
            {
                return (T)e;
            }

            var t = new T();
            mTypeEvents.Add(eType, t);
            return t;
        }
    }

    #endregion

    #region Command

    public interface ICommand
    {
        void Execute();
    }

    #endregion
}

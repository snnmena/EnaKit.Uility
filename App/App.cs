using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yoziya
{
    public interface IApp
    {
        void RegisterState<TState>(TState state) where TState : IState;
        void RegisterMode<TMode>(TMode mode) where TMode : IMode;
        TState GetState<TState>() where TState : class, IState;
        TMode GetMode<TMode>() where TMode : class, IMode;
        void SendCommand<T>(T command) where T : ICommand;

        TResult SendCommand<TResult>(ICommand<TResult> command);

        TResult SendQuery<TResult>(IQuery<TResult> query);

        void SendEvent<T>() where T : new();
        void SendEvent<T>(T e);

        IUnRegister RegisterEvent<T>(Action<T> onEvent);
        void UnRegisterEvent<T>(Action<T> onEvent);
    }
    public abstract class App<T> : IApp where T : App<T>, new()
    {
        public static Action<T> OnRegisterPatch = App => { };
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
                    foreach (var AppModel in mApp.mStatesCache)
                    {
                        AppModel.Initialize();
                    }
                    mApp.mStatesCache.Clear();
                    foreach (var AppSystem in mApp.mModesCache)
                    {
                        AppSystem.Initialize();
                    }
                    mApp.mModesCache.Clear();
                }
                return mApp;
            }
        }

        public void RegisterState<TState>(TState state) where TState : IState
        {
            state.SetApp(this);
            mContainer.Register<TState>(state);
            mStatesCache.Add(state);
        }
        public void RegisterMode<TMode>(TMode mode) where TMode : IMode
        {
            mode.SetApp(this);
            mContainer.Register<TMode>(mode);
            mModesCache.Add(mode);
        }
        public TState GetState<TState>() where TState : class, IState
        {
            return mContainer.Get<TState>();
        }
        public TMode GetMode<TMode>() where TMode : class, IMode
        {
            return mContainer.Get<TMode>();
        }
        public TResult SendCommand<TResult>(ICommand<TResult> command)
        {
            return ExecuteCommand(command);
        }

        public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            ExecuteCommand(command);
        }

        protected virtual TResult ExecuteCommand<TResult>(ICommand<TResult> command)
        {
            command.SetApp(this);
            return command.Execute();
        }

        protected virtual void ExecuteCommand(ICommand command)
        {
            command.SetApp(this);
            command.Execute();
        }

        public TResult SendQuery<TResult>(IQuery<TResult> query)
        {
            return DoQuery<TResult>(query);
        }

        protected virtual TResult DoQuery<TResult>(IQuery<TResult> query)
        {
            query.SetApp(this);
            return query.Do();
        }

        private TypeEventSystem mTypeEventSystem = new TypeEventSystem();

        public void SendEvent<TEvent>() where TEvent : new()
        {
            mTypeEventSystem.Send<TEvent>();
        }

        public void SendEvent<TEvent>(TEvent e)
        {
            mTypeEventSystem.Send<TEvent>(e);
        }

        public IUnRegister RegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            return mTypeEventSystem.Register<TEvent>(onEvent);
        }

        public void UnRegisterEvent<TEvent>(Action<TEvent> onEvent)
        {
            mTypeEventSystem.UnRegister<TEvent>(onEvent);
        }
        protected abstract void Initialize();
    }

    /// <summary>
    /// 数据层、状态层
    /// 能发送事件
    /// </summary>
    #region State

    public interface IState : IBelongToApp, ICanSetApp, ICanSendEvent
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
        void ICanSetApp.SetApp(IApp app)
        {
            mApp = app;
        }
        void IState.Initialize()
        {
            Init();
        }
        protected abstract void Init();
    }

    #endregion

    /// <summary>
    /// 定义游戏规则
    /// 能获取其他Mode、State，能订阅和发送事件
    /// </summary>
    #region Mode

    public interface IMode : IBelongToApp, ICanSetApp, ICanGetMode, ICanRegisterEvent, ICanSendEvent
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
        void ICanSetApp.SetApp(IApp app)
        {
            mApp = app;
        }
        void IMode.Initialize()
        {
            Init();
        }
        protected abstract void Init();
    }

    #endregion

    /// <summary>
    /// 定义游戏对象，以及它能做什么，能订阅事件，控制游戏对象在收到什么事件时执行Command
    /// </summary>
    #region Controller

    public interface IController : IBelongToApp, ICanGetMode, ICanGetState, ICanSendCommand, ICanSendQuery, ICanRegisterEvent
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
        void SetApp(IApp app);
    }

    public interface ICanGetState : IBelongToApp { }
    public interface ICanGetMode : IBelongToApp { }
    public interface ICanRegisterEvent : IBelongToApp { }
    public interface ICanSendCommand : IBelongToApp { }
    public interface ICanSendEvent : IBelongToApp { }
    public interface ICanSendQuery : IBelongToApp { }

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

        public static IUnRegister RegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent)
        {
            return self.GetApp().RegisterEvent<T>(onEvent);
        }

        public static void UnRegisterEvent<T>(this ICanRegisterEvent self, Action<T> onEvent)
        {
            self.GetApp().UnRegisterEvent<T>(onEvent);
        }

        public static void SendCommand<T>(this ICanSendCommand self) where T : ICommand, new()
        {
            self.GetApp().SendCommand<T>(new T());
        }

        public static void SendCommand<T>(this ICanSendCommand self, T command) where T : ICommand
        {
            self.GetApp().SendCommand<T>(command);
        }

        public static TResult SendCommand<TResult>(this ICanSendCommand self, ICommand<TResult> command)
        {
            return self.GetApp().SendCommand(command);
        }

        public static TResult SendQuery<TResult>(this ICanSendQuery self, IQuery<TResult> query)
        {
            return self.GetApp().SendQuery(query);
        }
    }

    #endregion

    #region Event

    public interface IUnRegister
    {
        void UnRegister();
    }

    public interface IUnRegisterList
    {
        List<IUnRegister> UnregisterList { get; }
    }

    public static class IUnRegisterListExtension
    {
        public static void AddToUnregisterList(this IUnRegister self, IUnRegisterList unRegisterList)
        {
            unRegisterList.UnregisterList.Add(self);
        }

        public static void UnRegisterAll(this IUnRegisterList self)
        {
            foreach (var unRegister in self.UnregisterList)
            {
                unRegister.UnRegister();
            }

            self.UnregisterList.Clear();
        }
    }

    /// <summary>
    /// 自定义可注销的类
    /// </summary>
    public struct CustomUnRegister : IUnRegister
    {
        /// <summary>
        /// 委托对象
        /// </summary>
        private Action mOnUnRegister { get; set; }

        /// <summary>
        /// 带参构造函数
        /// </summary>
        /// <param name="onDispose"></param>
        public CustomUnRegister(Action onUnRegsiter)
        {
            mOnUnRegister = onUnRegsiter;
        }

        /// <summary>
        /// 资源释放
        /// </summary>
        public void UnRegister()
        {
            mOnUnRegister.Invoke();
            mOnUnRegister = null;
        }
    }

    public class UnRegisterOnDestroyTrigger : MonoBehaviour
    {
        private readonly HashSet<IUnRegister> mUnRegisters = new HashSet<IUnRegister>();

        public void AddUnRegister(IUnRegister unRegister)
        {
            mUnRegisters.Add(unRegister);
        }

        public void RemoveUnRegister(IUnRegister unRegister)
        {
            mUnRegisters.Remove(unRegister);
        }

        private void OnDestroy()
        {
            foreach (var unRegister in mUnRegisters)
            {
                unRegister.UnRegister();
            }

            mUnRegisters.Clear();
        }
    }

    public static class UnRegisterExtension
    {
        public static IUnRegister UnRegisterWhenGameObjectDestroyed(this IUnRegister unRegister, GameObject gameObject)
        {
            var trigger = gameObject.GetComponent<UnRegisterOnDestroyTrigger>();

            if (!trigger)
            {
                trigger = gameObject.AddComponent<UnRegisterOnDestroyTrigger>();
            }

            trigger.AddUnRegister(unRegister);

            return unRegister;
        }

        public static IUnRegister UnRegisterWhenGameObjectDestroyed<T>(this IUnRegister self, T component)
            where T : Component
        {
            return self.UnRegisterWhenGameObjectDestroyed(component.gameObject);
        }
    }

    public class TypeEventSystem
    {
        private readonly EasyEvents mEvents = new EasyEvents();


        public static readonly TypeEventSystem Global = new TypeEventSystem();

        public void Send<T>() where T : new()
        {
            mEvents.GetEvent<EasyEvent<T>>()?.Trigger(new T());
        }

        public void Send<T>(T e)
        {
            mEvents.GetEvent<EasyEvent<T>>()?.Trigger(e);
        }

        public IUnRegister Register<T>(Action<T> onEvent)
        {
            var e = mEvents.GetOrAddEvent<EasyEvent<T>>();
            return e.Register(onEvent);
        }

        public void UnRegister<T>(Action<T> onEvent)
        {
            var e = mEvents.GetEvent<EasyEvent<T>>();
            if (e != null)
            {
                e.UnRegister(onEvent);
            }
        }
    }

    public interface IOnEvent<T>
    {
        void OnEvent(T e);
    }

    public static class OnGlobalEventExtension
    {
        public static IUnRegister RegisterEvent<T>(this IOnEvent<T> self) where T : struct
        {
            return TypeEventSystem.Global.Register<T>(self.OnEvent);
        }

        public static void UnRegisterEvent<T>(this IOnEvent<T> self) where T : struct
        {
            TypeEventSystem.Global.UnRegister<T>(self.OnEvent);
        }
    }

    public interface IEasyEvent
    {
    }

    public class EasyEvent : IEasyEvent
    {
        private Action mOnEvent = () => { };

        public IUnRegister Register(Action onEvent)
        {
            mOnEvent += onEvent;
            return new CustomUnRegister(() => { UnRegister(onEvent); });
        }

        public void UnRegister(Action onEvent)
        {
            mOnEvent -= onEvent;
        }

        public void Trigger()
        {
            mOnEvent?.Invoke();
        }
    }

    public class EasyEvent<T> : IEasyEvent
    {
        private Action<T> mOnEvent = e => { };

        public IUnRegister Register(Action<T> onEvent)
        {
            mOnEvent += onEvent;
            return new CustomUnRegister(() => { UnRegister(onEvent); });
        }

        public void UnRegister(Action<T> onEvent)
        {
            mOnEvent -= onEvent;
        }

        public void Trigger(T t)
        {
            mOnEvent?.Invoke(t);
        }
    }

    public class EasyEvent<T, K> : IEasyEvent
    {
        private Action<T, K> mOnEvent = (t, k) => { };

        public IUnRegister Register(Action<T, K> onEvent)
        {
            mOnEvent += onEvent;
            return new CustomUnRegister(() => { UnRegister(onEvent); });
        }

        public void UnRegister(Action<T, K> onEvent)
        {
            mOnEvent -= onEvent;
        }

        public void Trigger(T t, K k)
        {
            mOnEvent?.Invoke(t, k);
        }
    }

    public class EasyEvent<T, K, S> : IEasyEvent
    {
        private Action<T, K, S> mOnEvent = (t, k, s) => { };

        public IUnRegister Register(Action<T, K, S> onEvent)
        {
            mOnEvent += onEvent;
            return new CustomUnRegister(() => { UnRegister(onEvent); });
        }

        public void UnRegister(Action<T, K, S> onEvent)
        {
            mOnEvent -= onEvent;
        }

        public void Trigger(T t, K k, S s)
        {
            mOnEvent?.Invoke(t, k, s);
        }
    }

    public class EasyEvents
    {
        private static EasyEvents mGlobalEvents = new EasyEvents();

        public static T Get<T>() where T : IEasyEvent
        {
            return mGlobalEvents.GetEvent<T>();
        }


        public static void Register<T>() where T : IEasyEvent, new()
        {
            mGlobalEvents.AddEvent<T>();
        }

        private Dictionary<Type, IEasyEvent> mTypeEvents = new Dictionary<Type, IEasyEvent>();

        public void AddEvent<T>() where T : IEasyEvent, new()
        {
            mTypeEvents.Add(typeof(T), new T());
        }

        public T GetEvent<T>() where T : IEasyEvent
        {
            IEasyEvent e;

            if (mTypeEvents.TryGetValue(typeof(T), out e))
            {
                return (T)e;
            }

            return default;
        }

        public T GetOrAddEvent<T>() where T : IEasyEvent, new()
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

    public interface ICommand : IBelongToApp, ICanSetApp, ICanGetState, ICanGetMode, ICanSendEvent, ICanSendCommand, ICanSendQuery
    {
        void Execute();
    }

    public interface ICommand<TResult> : IBelongToApp, ICanSetApp, ICanGetState, ICanGetMode, ICanSendEvent, ICanSendCommand, ICanSendQuery
    {
        TResult Execute();
    }

    public abstract class Command : ICommand
    {
        private IApp mApp;

        IApp IBelongToApp.GetApp()
        {
            return mApp;
        }

        void ICanSetApp.SetApp(IApp App)
        {
            mApp = App;
        }

        void ICommand.Execute()
        {
            OnExecute();
        }

        protected abstract void OnExecute();
    }

    public abstract class Command<TResult> : ICommand<TResult>
    {
        private IApp mApp;

        IApp IBelongToApp.GetApp()
        {
            return mApp;
        }

        void ICanSetApp.SetApp(IApp App)
        {
            mApp = App;
        }

        TResult ICommand<TResult>.Execute()
        {
            return OnExecute();
        }

        protected abstract TResult OnExecute();
    }

    #endregion

    #region Query

    public interface IQuery<TResult> : IBelongToApp, ICanSetApp, ICanGetState, ICanGetMode, ICanSendQuery
    {
        TResult Do();
    }

    public abstract class Query<T> : IQuery<T>
    {
        public T Do()
        {
            return OnDo();
        }

        protected abstract T OnDo();


        private IApp mApp;

        public IApp GetApp()
        {
            return mApp;
        }

        public void SetApp(IApp app)
        {
            mApp = app;
        }
    }

    #endregion
}

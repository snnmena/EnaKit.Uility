using System;
using System.Collections.Generic;
using UnityEngine;

namespace Yoziya
{
    /// <summary>
    /// 定义物体对象它能做什么
    /// </summary>
    #region Pawn

    public interface IPawn
    {

    }

    public abstract class AbstractPawn : MonoBehaviour, IPawn
    {

    }

    #endregion

    /// <summary>
    /// 能订阅事件，控制游戏对象在收到什么事件时发送命令让Pawn执行
    /// </summary>
    #region Controller

    public interface IController
    {
        void Initialize();
        void SubscribeEvent();
        void SendCommand<T>(T command) where T : ICommand;
    }

    public abstract class AbstractController : MonoBehaviour, IController
    {
        private IPawn controlledPawn;
        public void Initialize()
        {
            controlledPawn = gameObject.GetComponent<IPawn>();
        }
        public void SendCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            command.Execute();
        }
        public void SubscribeEvent()
        {
            throw new NotImplementedException();
        }
    }

    #endregion

    /// <summary>
    /// 定义游戏规则
    /// 能获取其他Mode、State，能订阅和发送事件
    /// </summary>
    #region Mode

    public interface IMode
    {
        void GetMode<T>() where T : IMode;
        void GetState<T>() where T : IState;
        void SubscribeEvent();
        void SendEvent();
    }

    #endregion

    /// <summary>
    /// 数据层、状态层
    /// 能发送事件
    /// </summary>
    #region State

    public interface IState
    {
        void SendEvent();
    }

    #endregion

    /// <summary>
    /// 事件系统、观察者模式
    /// </summary>
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

    /// <summary>
    /// 命令模式
    /// </summary>
    #region Command

    public interface ICommand
    {
        void Execute();
    }

    #endregion
}

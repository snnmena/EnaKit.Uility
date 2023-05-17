using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yoziya
{
    public class EventManager
    {
        // 单例实例
        private static EventManager _instance;
        public static EventManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new EventManager();
                }
                return _instance;
            }
        }

        // 用于存储事件和对应的订阅者列表
        private Dictionary<string, List<Action<object>>> eventDictionary;

        private EventManager()
        {
            eventDictionary = new Dictionary<string, List<Action<object>>>();
        }

        // 订阅事件
        public void Subscribe(string eventName, Action<object> listener)
        {
            if (!eventDictionary.ContainsKey(eventName))
            {
                eventDictionary.Add(eventName, new List<Action<object>>());
            }
            eventDictionary[eventName].Add(listener);
        }

        // 取消订阅事件
        public void Unsubscribe(string eventName, Action<object> listener)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                eventDictionary[eventName].Remove(listener);
            }
        }

        // 发布事件
        public void Publish(string eventName, object eventParam)
        {
            if (eventDictionary.ContainsKey(eventName))
            {
                foreach (var listener in eventDictionary[eventName])
                {
                    listener.Invoke(eventParam);
                }
            }
        }
    }
}

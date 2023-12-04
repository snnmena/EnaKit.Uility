using System;
using System.Collections;
using System.Collections.Generic;

namespace Yoziya
{
    public class OnEventProperty<T>
    {
        public OnEventProperty(T value = default)
        {
            Value = value;
        }
        private event Action<T> mOnValueChanged;
        public T Value
        {
            get => mValue;
            set
            {
                if (value == null && mValue == null) return;
                if (value != null && !value.Equals(mValue))
                {
                    mValue = value;
                    mOnValueChanged?.Invoke(value);
                }
            }
        }
        private T mValue = default;
        public void SetValueWithoutEvent(T newValue)
        {
            mValue = newValue;
        }
        public void AddListener(Action<T> action)
        {
            mOnValueChanged += action;
        }
        public void RemoveListener(Action<T> action)
        {
            mOnValueChanged -= action;
        }
    }
}
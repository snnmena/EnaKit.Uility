using System;
using System.Collections;
using System.Collections.Generic;

namespace Yoziya
{
    public class OnEventProperty<T> where T : IEquatable<T>
    {
        private event Action<T> OnValueChanged;
        public T Value
        {
            get => mValue;
            set
            {
                if(!mValue.Equals(value))
                {
                    mValue = value;
                    OnValueChanged?.Invoke(mValue);
                }
            }
        }
        private T mValue = default;
        public void AddListener(Action<T> action)
        {
            OnValueChanged += action;
        }
        public void RemoveListener(Action<T> action)
        {
            OnValueChanged -= action;
        }
    }
}
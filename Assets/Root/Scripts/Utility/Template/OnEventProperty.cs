using System;
using System.Collections;
using UnityEngine;

namespace Yoziya
{
    public class OnEventProperty<T> where T : IEquatable<T>
    {
        public Action<T> OnValueChanged;
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
    }
}
using System;
using System.Collections;
using System.Reflection;

namespace Yoziya
{
    public class Singleton<T> where T : class
    {
        private static T mInstance;
        public static T Instance
        {
            get
            {
                if (mInstance == null)
                {
                    Type type = typeof(T);
                    var ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                    var ctor = Array.Find(ctors, c=>c.GetParameters().Length == 0);
                    if (ctor == null)
                    {
                        throw new Exception($"{type.FullName} 缺少一个私有构造函数");
                    }
                    mInstance = ctor.Invoke(null) as T;
                }
                return mInstance;
            }
        }
    }
}
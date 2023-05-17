using System;

namespace Yoziya
{
    //[Serializable]
    public abstract class BaseConfig
    {
        public abstract void SetData(string[] data);
        public abstract int GetID();
    }
}

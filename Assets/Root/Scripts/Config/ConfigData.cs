using System;
using System.Collections.Generic;

namespace Yoziya
{
    //[Serializable]
    public class ConfigData<T> where T : BaseConfig, new()
    {
        public Dictionary<int, T> configDataDictionary = new Dictionary<int, T>();

        public void LoadConfigData(string[] data)
        {
            for (int i = 1; i < data.Length - 1; i++)
            {
                string[] row = data[i].Split(new char[] { ',' });
                T configItem = new T();
                configItem.SetData(row);
                configDataDictionary.Add(configItem.GetID(), configItem);
            }
        }
    }
}

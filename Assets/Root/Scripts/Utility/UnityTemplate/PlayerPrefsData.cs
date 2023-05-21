using System.Collections;
using UnityEngine;

namespace Yoziya
{
    public interface ISerializer<T>
    {
        string Serialize(T value);
        T Deserialize(string serializedData);
    }
    public struct StringSerializer : ISerializer<string>
    {
        public string Serialize(string value)
        {
            return value;
        }

        public string Deserialize(string serializedData)
        {
            return serializedData;
        }
    }

    public class PlayerPrefsData<T>
    {
        private string mData;
        private string mKey;
        private ISerializer<T> mSerializer;

        public PlayerPrefsData(string key, ISerializer<T> serializer)
        {
            mKey = key;
            mSerializer = serializer;
        }

        public T Data
        {
            get
            {
                if (mData == null)
                {
                    mData = PlayerPrefs.GetString(mKey);
                }
                return mSerializer.Deserialize(mData);
            }
            set
            {
                var serializedData = mSerializer.Serialize(value);
                PlayerPrefs.SetString(mKey, serializedData);
                mData = serializedData;
            }
        }
    }
}
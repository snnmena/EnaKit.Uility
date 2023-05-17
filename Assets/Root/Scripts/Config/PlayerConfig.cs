using System;

namespace Yoziya
{
    //[Serializable]
    public class PlayerConfig : BaseConfig
    {
        public int ID;
        public string Name;
        public float Value;

        public override void SetData(string[] data)
        {
            int.TryParse(data[0], out ID);
            Name = data[1];
            float.TryParse(data[2], out Value);
        }

        public override int GetID()
        {
            return ID;
        }
    }
}

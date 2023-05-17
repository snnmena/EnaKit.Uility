using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yoziya
{
    public interface IModel
    {
        void Initialize();
    }

    public abstract class GameModel : IModel
    {
        public void Initialize()
        {
            throw new System.NotImplementedException();
        }
    }
}

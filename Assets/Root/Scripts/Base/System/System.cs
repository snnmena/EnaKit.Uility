using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yoziya
{
    public interface ISystem
    {
        void Initialize();
    }

    public class GameSystem : ISystem
    {
        public virtual void Initialize()
        {
            throw new System.NotImplementedException();
        }

        void Update()
        {

        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yoziya.manjuu
{
    public class Manjuu : App<Manjuu>
    {
        protected override void Initialize()
        {
            RegisterState<Player>(new Player());

            RegisterMode<InputSystem>(new InputSystem());
        }
    }
}

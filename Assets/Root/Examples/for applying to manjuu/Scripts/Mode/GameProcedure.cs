using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yoziya.manjuu
{
    public class GameProcedure : Mode
    {
        enum GameState
        {
            EnterGame,
            CheckUpdate,
            Start,
        }
        OnEventProperty<Enum> Procedure { get; }
        protected override void Init()
        {
            throw new System.NotImplementedException();
        }
    }
}

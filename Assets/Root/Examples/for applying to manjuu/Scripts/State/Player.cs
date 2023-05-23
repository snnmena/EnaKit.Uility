using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yoziya.manjuu
{
    public interface IPlayer : IState
    {
        OnEventProperty<int> Score { get; }
    }

    public class Player : State, IPlayer
    {
        public OnEventProperty<int> Score { get; } = new OnEventProperty<int>();
        protected override void Init()
        {
            Score.Value = 0;
        }
    }
}

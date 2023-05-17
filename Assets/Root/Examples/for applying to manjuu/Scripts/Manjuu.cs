using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yoziya.manjuu
{
    public class Manjuu : App
    {
        protected override void Awake()
        {
            base.Awake();
        }
        protected override void Start()
        {
            base.Start();
            ChangeProcedure(new StartProcedure());
        }
    }
}

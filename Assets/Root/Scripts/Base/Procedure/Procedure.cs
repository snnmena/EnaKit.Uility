using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yoziya
{
    public interface IProcedure
    {
        void Enter();
        void Exit();
        void Update();
    }
    public abstract class Procedure : IProcedure
    {
        public abstract void Enter();
        public abstract void Exit();
        public virtual void Update()
        {

        }
        public void Set()
        {
            //App.UI.Load("Start");
        }
    }
}

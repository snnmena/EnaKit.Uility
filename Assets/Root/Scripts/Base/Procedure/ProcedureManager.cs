using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yoziya
{
    public class ProcedureManager
    {
        private IProcedure mCurrentProcedure;

        public void ChangeProcedure(IProcedure newProcedure)
        {
            mCurrentProcedure?.Exit();
            mCurrentProcedure = newProcedure;
            mCurrentProcedure.Enter();
        }

        public void UpdateProcedure()
        {
            mCurrentProcedure?.Update();
        }
    }
}

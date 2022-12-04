using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public abstract class Simulating
    {
        public virtual void UpdateSimulation(float time)
        {
            if (!isSimulating)
                return;
        }
        public virtual void StartSimulation()
        {
            isSimulating = true;
        }
        public virtual void EndSimulation()
        {
            isSimulating = false;
        }
        public bool IsSimulating => isSimulating;
        protected bool isSimulating = false;
    }
}

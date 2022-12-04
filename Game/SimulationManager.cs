using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    public class SimulationManager : Simulating
    {
        private List<Simulating> simulatings = new List<Simulating>();

 
        public void AddSimulating(Simulating simulating)
        {
            simulatings.Add(simulating);
        }
        public override void StartSimulation()
        {
            base.StartSimulation();
            foreach (var item in simulatings)
            {
                item.StartSimulation();
            }
        }

        public override void EndSimulation()
        {
            base.EndSimulation();
            foreach (var item in simulatings)
            {
                item.EndSimulation();
            }
        }
        public override void UpdateSimulation(float time)
        {
            foreach (var item in simulatings)
            {
                item.UpdateSimulation(time);
            }
        }
    }
}

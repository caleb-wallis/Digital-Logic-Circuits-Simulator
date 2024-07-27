using System.Collections.Generic;
using System.Drawing;

namespace Circuits
{
    /// <summary>
    /// This class implements a compound gate which contains other gates.
    /// </summary>
    public class Compound : Gate
    {
        // List of gates in the compound
        private List<Gate> gates = new List<Gate>();

        /// <summary>
        /// Creates a compound object
        /// </summary>
        public Compound()
        {
        }

        public override void Draw(Graphics paper)
        {            
            // Draw each gate selected
            foreach (Gate g in gates)
            {
                g.Selected = Selected;
                g.Draw(paper);
            }
        }

        public override void MoveTo(int x, int y)
        {
            // Calculate the offset for moving the gates relative to the compound gate
            int xOffset = x - Left;
            int yOffset = y - Top;

            // Move each gate inside the compound gate
            foreach (Gate g in gates)
                g.MoveTo(g.Left + xOffset, g.Top + yOffset);

            // Update the position of the compound gate
            Left = x;
            Top = y;
        }

        public override bool Evaluate()
        {
            foreach (Gate g in gates)
            {
                if (g is OutputLamp)
                {
                    g.Evaluate();
                }
            }
            return true;
        }

        public override Gate Clone()
        {
            Compound clone = new Compound();
            // Clone each gate and add it to the cloned compound
            foreach (Gate g in gates)
            {
                Gate gateClone = g.Clone();
                clone.AddGate(gateClone);
            }
            return clone;
        }
        

        /// <summary>
        /// Adds the gate to the list of gates in the compound
        /// </summary>
        /// <param name="g">The gate to add</param>
        public void AddGate(Gate g)
        {
            gates.Add(g);
        }

        /// <summary>
        /// Gets the list of gates in the compound
        /// </summary>
        public List<Gate> Gates
        {
            get { return gates; }
        }

        /// <summary>
        /// Checks if any of the gates in the compound have been clicked
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override bool IsMouseOn(int x, int y)
        {
            // Draw each gate unselected
            foreach (Gate g in gates)
            {
                if(g.IsMouseOn(x, y))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if any of the pins in the gates of the compound have been clicked
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public override Pin SelectedPin(int x, int y)
        {
            // Draw each gate unselected
            foreach (Gate g in gates)
            {
                // Check if a pin was selected
                Pin pin = g.SelectedPin(x, y);
                if (pin != null)
                {
                    return pin; // Return the pin
                }
            }
            return null; // If no pin was selected return null
        }
    }
}

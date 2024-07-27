using System.Collections.Generic;
using System.Drawing;


namespace Circuits
{
    /// <summary>
    /// This class is the superclass of all gates.
    /// </summary>
    public abstract class Gate
    {
        // Left is the left-hand edge of the main part of the gate.
        // So the input pins are further left than left.
        private int left = 0;

        // Top is the top of the whole gate
        private int top = 0;

        // width and height of the gate
        private int width = 0;
        private int height = 0;

        // This is the list of all the pins of this gate.
        private List<Pin> pins = new List<Pin>();

        //Has the gate been selected
        private bool selected = true;  // True as you are selecting it on creation

        // Offset of clone from original
        private int cloneOffset = 10;


        /// <summary>
        /// Initializes a gate object
        /// </summary>
        /// <param name="w">Width of gate</param>
        /// <param name="h">Height of gate</param>
        public Gate(int w, int h)
        {
           width = w;
           height = h;
        }

        // <summary>
        /// Initializes a gate object with no width or height
        /// </summary>
        public Gate()
        {
        }
        

        /// <summary>
        /// Gets and sets whether the fate is selected or not.
        /// </summary>
        public bool Selected
        {
            get { return selected; }
            set { selected = value; }
        }

        /// <summary>
        /// Gets and sets the left hand edge of the gate.
        /// </summary>
        public int Left
        {
            get { return left; }
            set { left = value; }
        }

        /// <summary>
        /// Gets and sets the top edge of the gate.
        /// </summary>
        public int Top
        {
            get { return top; }
            set { top = value; }
        }

        /// <summary>
        /// Gets the list of pins for the gate.
        /// </summary>
        public List<Pin> Pins
        {
            get { return pins; }
        }

        /// <summary>
        /// Gets the clone offset.
        /// </summary>
        public int CloneOffset
        {
            get { return cloneOffset; }
        }

        /// <summary>
        /// Gets the gates width.
        /// </summary>
        public int Width
        {
            get { return width; }
        }

        /// <summary>
        /// Gets the gates height.
        /// </summary>
        public int Height
        {
            get { return height; }
        }

        /// <summary>
        /// Checks if the gate has been clicked on.
        /// </summary>
        /// <param name="x">The x position of the mouse click</param>
        /// <param name="y">The y position of the mouse click</param>
        /// <returns>True if the mouse click position is inside the gate</returns>
        public virtual bool IsMouseOn(int x, int y)
        {
            if (left <= x && x < left + width
                && top <= y && y < top + height)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Draws the gate
        /// </summary>
        /// <param name="paper">Graphics to draw the gate on</param>
        public abstract void Draw(Graphics paper);

        /// <summary>
        /// Moves the gate to the position specified.
        /// </summary>
        /// <param name="x">The x position to move the gate to</param>
        /// <param name="y">The y position to move the gate to</param>
        public abstract void MoveTo(int x, int y);

        /// <summary>
        /// Computes the correct logical result for that kind of gate.
        /// </summary>
        /// <returns>The evaluation - True or False</returns>
        public abstract bool Evaluate();

        /// <summary>
        /// Returns a fresh copy of the gate
        /// </summary>
        public abstract Gate Clone();

        /// <summary>
        /// Checks whether all input pins are connected or not
        /// </summary>
        /// <returns>If all pins connected or not</returns>
        public bool ConnectedInputPins()
        {
            foreach (Pin pin in pins)
            {
                if (pin.IsInput)
                {
                    if(pin.InputWire == null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// Finds the pin that is close to (x,y), or returns
        /// null if there are no pins close to the position.
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>The pin that has been selected</returns>
        public virtual Pin SelectedPin(int x, int y)
        {
            // For each pin
            foreach (Pin p in Pins)
            {
                // If mouse is on the pin
                if (p.isMouseOn(x, y))
                    return p;
            }
            return null;
        }
    }
}

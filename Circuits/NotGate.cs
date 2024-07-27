using System;
using System.Drawing;

namespace Circuits
{
    /// <summary>
    /// This class implements an NOT gate with two inputs and one output.
    /// </summary>
    class NotGate : Gate
    {
        // Length of Input and Output Pins
        protected const int InputPinLength = 20;
        protected const int OutputPinLength = 30;

        // Image buffer for pins
        private const int BUFFER = 5;

        /// <summary>
        /// Initialises the Gate.
        /// </summary>
        /// <param name="x">The x position of the gate</param>
        /// <param name="y">The y position of the gate</param>
        public NotGate(int x, int y) : base(40, 40)
        {
            //Add the two input pins to the gate
            Pins.Add(new Pin(this, true, InputPinLength));
            //Add the output pin to the gate
            Pins.Add(new Pin(this, false, OutputPinLength));
            //move the gate and the pins to the position passed in
            MoveTo(x, y);
        }

        
        public override void Draw(Graphics paper)
        {
            //Draw each of the pins
            foreach (Pin p in Pins)
                p.Draw(paper);

            //Check if the gate has been selected
            if (Selected)
            {
                // Draw Red AndGate
                paper.DrawImage(Properties.Resources.NotGateAllRed, Left, Top);
            }
            else
            {
                // Draw Normal AndGate
                paper.DrawImage(Properties.Resources.NotGate, Left, Top);
            }
        }

        
        public override void MoveTo(int x, int y)
        {
            //Set the position of the gate to the values passed in
            Left = x;
            Top = y;

            // must move the pins too
            Pins[0].X = x - InputPinLength;
            Pins[0].Y = y + Height / 2+ BUFFER;

            Pins[1].X = x + Width + OutputPinLength;
            Pins[1].Y = y + Height / 2 + BUFFER;
        }

        public override bool Evaluate()
        {
            if (ConnectedInputPins())
            {
                // Gets the gate that the 1st input pin is connected to
                Gate gateA = Pins[0].InputWire.FromPin.Owner;
                // Returns the result of the evaluation of the gate
                return !gateA.Evaluate();
            }

            else
            {
                Console.WriteLine("Not All Input Pins Connected - Returned False");
                return false;
            }
        }

        public override Gate Clone()
        {
            NotGate notGate = new NotGate(Left + CloneOffset, Top + CloneOffset);
            return notGate;
        }
    }
}

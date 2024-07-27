using System.Drawing;

namespace Circuits
{
    /// <summary>
    /// This class implements an Output Lamp with one input.
    /// </summary>
    class OutputLamp : Gate
    {
        // Length of Input and Output Pins
        private const int InputPinLength = 10;

        // Whether the output pin is currently high voltage
        private bool voltage;

        public OutputLamp(int x, int y) : base(20, 20)
        {
            voltage = false;

            //Add the output pin to the gate
            Pins.Add(new Pin(this, true, InputPinLength));
            //move the gate and the pins to the position passed in
            MoveTo(x, y);
        }


        public override void Draw(Graphics paper)
        {
            //Draw each of the pins
            foreach (Pin p in Pins)
                p.Draw(paper);

            Brush brush = new SolidBrush(Color.Gray);
            Pen pen = new Pen(Color.Red);

            if (Selected)
            {
                pen = new Pen(Color.Red);
            }

            else
            {
                pen = new Pen(Color.Black);
            }

            if (voltage)
            {
                // Selected Color
                brush = new SolidBrush(Color.Orange);
            }
            else
            {
                // Unselected Color
                brush = new SolidBrush(Color.Black);
            }

            Rectangle rect1 = new Rectangle(Left, Top, Width, Height);
            paper.FillRectangle(brush, rect1);
            paper.DrawRectangle(pen, rect1);
        }


        public override void MoveTo(int x, int y)
        {
            //Set the position of the gate to the values passed in
            Left = x;
            Top = y;

            Pins[0].X = x - InputPinLength;
            Pins[0].Y = y + Height / 2;
        }

      
        /// <summary>
        /// Switches the voltage
        /// </summary>
        public void SwitchVoltage()
        {
            voltage = !voltage;
        }

        public override bool Evaluate()
        {
            // Get the gate of the input wire
            if(ConnectedInputPins())
            {
                Gate gateA = Pins[0].InputWire.FromPin.Owner;
                // Evaluate that gate
                voltage = gateA.Evaluate();
            }
            else
            {
                voltage = false;
            }

            // Return result
            return voltage;
        }

        public override Gate Clone()
        {
            OutputLamp outputLamp = new OutputLamp(Left + CloneOffset, Top + CloneOffset);
            return outputLamp;
        }
    }
}

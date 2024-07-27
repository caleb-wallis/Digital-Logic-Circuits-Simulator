using System.Drawing;

namespace Circuits
{
    /// <summary>
    /// This class implements an Input Source gate with one output pin.
    /// </summary>
    class InputSource : Gate
    {
        // Length of Input and Output Pins
        protected const int OutputPinLength = 10;

        // Whether the output pin is currently high voltage
        private bool voltage;

        public InputSource(int x, int y) : base(20, 20)
        {
            voltage = false;

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

            Brush brush = new SolidBrush(Color.Gray);

            string voltageText = "0";

            if (voltage)
            {
                // Selected Color
                brush = new SolidBrush(Color.Green);
                voltageText = "1";
            }
            else
            {
                // Unselected Color
                brush = new SolidBrush(Color.Gray);
                voltageText = "0";
            }

            Rectangle rect1 = new Rectangle(Left, Top, Width, Height); 
            Font font1 = new Font("Arial", 12, FontStyle.Bold, GraphicsUnit.Point);

            // Create a StringFormat object so that we can have a number centered on the input source.
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;

            paper.DrawString(voltageText, font1, brush, rect1, stringFormat);
            if (Selected)
            {
                paper.DrawRectangle(Pens.Red, rect1);
            }
            else
            {
                paper.DrawRectangle(Pens.Black, rect1);
            }
        }

        public override void MoveTo(int x, int y)
        {
            //Set the position of the gate to the values passed in
            Left = x;
            Top = y;
           
            Pins[0].X = x + Width + OutputPinLength;
            Pins[0].Y = y + Height/2;
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
            return voltage;
        }

        public override Gate Clone()
        {
            InputSource inputSource = new InputSource(Left + CloneOffset, Top + CloneOffset);
            return inputSource;
        }
    }
}

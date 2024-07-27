using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Circuits
{
    /// <summary>
    /// The main GUI for the COMPX102 digital circuits editor.
    /// This has a toolbar, containing buttons called buttonAnd, buttonOr, etc.
    /// The contents of the circuit are drawn directly onto the form.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// The (x,y) mouse position of the last MouseDown event.
        /// </summary>
        private int startX, startY;

        /// <summary>
        /// If this is non-null, we are inserting a wire by
        /// dragging the mouse from startPin to some output Pin.
        /// </summary>
        private Pin startPin = null;

        /// <summary>
        /// The (x,y) position of the current gate, just before we started dragging it.
        /// </summary>
        private int currentX, currentY;

        /// <summary>
        /// The set of gates in the circuit
        /// </summary>
        private List<Gate> gatesList = new List<Gate>();

        /// <summary>
        /// The set of connector wires in the circuit
        /// </summary>
        private List<Wire> wiresList = new List<Wire>();

        /// <summary>
        /// The currently selected gate, or null if no gate is selected.
        /// </summary>
        private Gate current = null;

        /// <summary>
        /// The new gate that is about to be inserted into the circuit
        /// </summary>
        private Gate newGate = null;

        /// <summary>
        /// The new compound that is being created
        /// </summary>
        private Compound newCompound = null;

        public Form1()
        {
            InitializeComponent();
            DoubleBuffered = true;
        }

        /// <summary>
        /// Finds the pin that is close to (x,y), or returns
        /// null if there are no pins close to the position.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>The pin that has been selected</returns>
        public Pin findPin(int x, int y)
        {
            foreach (Gate g in gatesList)
            {
                Pin pin = g.SelectedPin(x, y);
                if (pin != null)
                    return pin;
            }
            return null;
        }

        /// <summary>
        /// Handles all events when the mouse is moving.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (startPin != null)
            {
                // Console.WriteLine("wire from " + startPin + " to " + e.X + "," + e.Y);
                currentX = e.X;
                currentY = e.Y;
                this.Invalidate();  // this will draw the line
            }
            else if (startX >= 0 && startY >= 0 && current != null)
            {
                // Console.WriteLine("mouse move to " + e.X + "," + e.Y);
                current.MoveTo(currentX + (e.X - startX), currentY + (e.Y - startY));
                this.Invalidate();
            }
            else if (newGate != null)
            {
                currentX = e.X;
                currentY = e.Y;
                this.Invalidate();
            }
        }

        /// <summary>
        /// Handles all events when the mouse button is released.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            if (startPin != null)
            {
                // see if we can insert a wire
                Pin endPin = findPin(e.X, e.Y);
                if (endPin != null)
                {
                    //Console.WriteLine("Trying to connect " + startPin + " to " + endPin);
                    Pin input, output;
                    if (startPin.IsOutput)
                    {
                        input = endPin;
                        output = startPin;
                    }
                    else
                    {
                        input = startPin;
                        output = endPin;
                    }
                    if (input.IsInput && output.IsOutput)
                    {
                        if (input.InputWire == null)
                        {
                            Wire newWire = new Wire(output, input);
                            input.InputWire = newWire;
                            wiresList.Add(newWire);
                        }
                        else
                        {
                            MessageBox.Show("That input is already used.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Error: you must connect an output pin to an input pin.");
                    }
                }
                startPin = null;
                this.Invalidate();
            }
            // We have finished moving/dragging
            startX = -1;
            startY = -1;
            currentX = 0;
            currentY = 0;
        }

        /// <summary>
        /// This will create a new And gate.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonAnd_Click(object sender, EventArgs e)
        {
            newGate = new AndGate(0, 0);
            UnselectCurrent(); // To deselect current gate
        }

        /// <summary>
        /// Redraws all the graphics for the current circuit.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //Draw all of the gates
            foreach (Gate g in gatesList)
            {
                g.Draw(e.Graphics);
            }
            //Draw all of the wires
            foreach (Wire w in wiresList)
            {
                w.Draw(e.Graphics);
            }

            if (startPin != null)
            {
                e.Graphics.DrawLine(Pens.White, startPin.X, startPin.Y, currentX, currentY);
            }

            // Show the gate that we are dragging into the circuit
            if (newGate != null)
            {
                // Move if not a compound
                if (!(newGate is Compound))
                {
                    newGate.MoveTo(currentX, currentY);
                }

                newGate.Draw(e.Graphics);
            }
        }

        /// <summary>
        /// This will create a new Or gate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonOr_Click(object sender, EventArgs e)
        {
            newGate = new OrGate(0, 0);
            UnselectCurrent(); // To deselect current gate
        }

        /// <summary>
        /// This will create a new Not gate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonNot_Click(object sender, EventArgs e)
        {
            newGate = new NotGate(0, 0);
            UnselectCurrent(); // To deselect current gate
        }

        /// <summary>
        /// This will create a new Input Source gate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonInput_Click(object sender, EventArgs e)
        {
            newGate = new InputSource(0, 0);
            UnselectCurrent(); // To deselect current gate
        }

        /// <summary>
        /// This will create a new Output Lamp gate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonOutput_Click(object sender, EventArgs e)
        {
            newGate = new OutputLamp(0, 0);
            UnselectCurrent(); // To deselect current gate
        }

        /// <summary>
        /// Evaluates the circuit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonEvaluate_Click(object sender, EventArgs e)
        {
            // For every gate
            foreach (Gate gate in gatesList)
            {
                // Start the evaluation process if gate is a compound or output lamp
                if (gate is Compound || gate is OutputLamp)
                {
                    gate.Evaluate();
                }
            }
            this.Invalidate();
            MessageBox.Show("Evaluated Gates");
        }


        /// <summary>
        /// Makes a copy of selected gates
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonCopy_Click(object sender, EventArgs e)
        {
            // If a gate has been selected
            if (current != null)
            {
                // Clone the gate
                Gate clone = current.Clone();

                // Try make compound
                Compound c = current as Compound;
                Compound cl = clone as Compound;

                // If cloning a compound
                if(c != null)
                {
                    // Loop through every gate in the compound
                    foreach(Gate g in c.Gates)
                    {
                        // Loop through every pin in each gate
                        foreach (Pin pin in g.Pins)
                        {
                            // If wire exists
                            if (pin.InputWire != null)
                            {
                                // Get the index of the gates (owners of the pins)
                                Gate fromOwner = pin.InputWire.FromPin.Owner;
                                Gate toOwner = pin.InputWire.ToPin.Owner;

                                int fromOwnerIndex = c.Gates.IndexOf(fromOwner);
                                int toOwnerIndex = c.Gates.IndexOf(toOwner);

                                // Get the index of the pins
                                Pin fromPin = pin.InputWire.FromPin;
                                Pin toPin = pin.InputWire.ToPin;

                                int fromPinIndex = fromOwner.Pins.IndexOf(fromPin);
                                int toPinIndex = toOwner.Pins.IndexOf(toPin);

                                // Create a new wire from using the gates and pins indexes
                                Wire wireClone = new Wire(cl.Gates[fromOwnerIndex].Pins[fromPinIndex], cl.Gates[toOwnerIndex].Pins[toPinIndex]);

                                // Connect cloned wire and cloned pin together
                                int gateIndex = c.Gates.IndexOf(g);
                                int pinIndex = g.Pins.IndexOf(pin);
                                cl.Gates[gateIndex].Pins[pinIndex].InputWire = wireClone;

                                // Add cloned wire to wire list
                                wiresList.Add(wireClone);
                            }
                        }
                    }
                }

                // Add clone to gates
                gatesList.Add(clone);
                // Set clone as current
                current.Selected = false;
                current = clone;
                // Redraw
                this.Invalidate();
            }
        }


        /// <summary>
        /// Starts a new compound
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonCompound_Click(object sender, EventArgs e)
        {
            // Safe gaurd
            if (newCompound == null)
            {
                newCompound = new Compound();
                MessageBox.Show("Starting Compound");
            }
        }

        /// <summary>
        /// Finalise a compound gate
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void toolStripButtonEndCompound_Click(object sender, EventArgs e)
        {
            // If there we are making a new Compound
            if (newCompound != null)
            {
                MessageBox.Show("Finished Compound");

                foreach (Gate g in gatesList.ToList())
                {
                    // If the gate is selected
                    if (g.Selected)
                    {
                        // If the gate is a compound gate
                        if (g is Compound)
                        {
                            Compound compound = (Compound)g;
                            // Add every game in the compound
                            foreach (Gate gate in compound.Gates)
                            {
                                newCompound.AddGate(gate);
                            }
                        }
                        // Else add the gate
                        else
                        {
                            newCompound.AddGate(g);
                        }

                        // Remove old gate from gatesList
                        gatesList.Remove(g);
                    }
                }

                // Set new gate to the new compound if it has gates
                if(newCompound.Gates.Count > 0)
                {
                    newGate = newCompound;
                }

                // Reset new compound
                newCompound = null;

                // Trigger mouse click which adds new gates so no movement delay =)
                Form1_MouseClick(sender, null);
            }
        }

        /// <summary>
        /// Handles events while the mouse button is pressed down.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            if (newCompound != null)
            {
                // Stops the movement and adding wires while making a compound
            }

            else if (current == null)
            {
                // try to start adding a wire
                startPin = findPin(e.X, e.Y);
            }

            else if (current.IsMouseOn(e.X, e.Y))
            {
                // start dragging the current object around
                startX = e.X;
                startY = e.Y;
                currentX = current.Left;
                currentY = current.Top;

                // If clicked on compound check whether an input was clicked
                if (current is Compound)
                {
                    // Create the compound
                    Compound compound = (Compound)current;
                    // Loop through the compound
                    foreach (Gate gate in compound.Gates)
                    {
                        // If the input source was clicked
                        if (gate.IsMouseOn(startX, startY))
                        {
                            SwitchInputSourceVoltage(gate);
                        }
                    }
                }

                else
                {
                    // If current gate is an input source switch the voltage
                    SwitchInputSourceVoltage(current);
                }
            }   
        }

        /// <summary>
        /// Checks if a gate is an input source and switches its voltage
        /// </summary>
        /// <param name="g">Gate to check</param>
        private void SwitchInputSourceVoltage(Gate g)
        {
            // If input source is clicked switch the voltage
            if (g is InputSource)
            {
                InputSource input = (InputSource)g;
                input.SwitchVoltage();
            }
        }

        /// <summary>
        /// Handles all events when a mouse is clicked in the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            UnselectCurrent();

            // See if we are inserting a new gate
            if (newGate != null)
            {
                // Add the gate to the list of gates
                gatesList.Add(newGate);
                current = newGate;
                newGate = null;
            }

            else 
            {
                // search for the first gate under the mouse position
                foreach (Gate g in gatesList)
                {
                    if (g.IsMouseOn(e.X, e.Y))
                    {
                        g.Selected = true;
                        current = g;
                        break;
                    }
                }
            }

            // Redraw graphics
            this.Invalidate();
        }

        /// <summary>
        /// Unselect the currently selected gate
        /// </summary>
        public void UnselectCurrent()
        {
            //Check if a gate is currently selected
            if (current != null)
            {
                // Unselect current gate if not making compound
                if (newCompound == null)
                {
                    current.Selected = false;
                }
                current = null;
            }
        }
    }
}
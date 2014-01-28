using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KabelTest
{
    public partial class Form1 : Form
    {
        enum FIELDTYPE { Nothing, Cable, Lamp, Engine };
        enum DIRECTION { Left, Right, Top, Bottom };

        struct FIELD
        {
            public string text;
            public FIELDTYPE type;
            public float currentVoltage;
            public float maxVoltage;


            public float currentProduction;
            public float currentResistance;
            public float currentConsumption;

            public Dictionary<DIRECTION, bool> hasNeighbour;
            public float maxInput;
            public Dictionary<DIRECTION, float> inputFrom;
            public float maxOutput;
            public Dictionary<DIRECTION, float> outputTo;

            public bool isActive;
        }

        FIELD[,] fields = new FIELD[10, 10];

        public Form1()
        {
            for (int row = 0; row <= fields.GetUpperBound(0); row++)
            {
                for (int col = 0; col <= fields.GetUpperBound(0); col++)
                {
                    FIELD field = new FIELD();

                    field.type = FIELDTYPE.Nothing;
                    field.text = field.type.ToString();
                    fields[row, col] = field;

                }
            }
            InitializeComponent();

            RefreshAllFields();

        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void RefreshAllFields()
        {
            for (int row = 0; row <= fields.GetUpperBound(0); row++)
            {
                for (int col = 0; col <= fields.GetUpperBound(0); col++)
                {
                    RefreshField(row, col);

                }
            }
            CheckNeighbours();
        }

        private void RefreshField(int row, int col)
        {
            Control[] controls = this.Controls.Find("field" + row.ToString() + "_" + col.ToString(), false);

            if (controls.Count() > 0)
            {
                FIELD field = fields[row, col];

                Button btn = (Button)controls[0];
                if (field.type == FIELDTYPE.Nothing)
                { btn.Text = ""; }
                else
                { btn.Text = field.text; }

                switch (field.type)
                {
                    case FIELDTYPE.Cable:
                        field.currentVoltage = 0.0f;
                        field.maxVoltage = 5.0f;

                        field.currentProduction = 0.0f;
                        field.currentResistance = 0.5f;
                        field.currentConsumption = 0.0f;

                        field.maxInput = 3.0f;
                        field.inputFrom = GetEmptyInputFrom();
                        field.maxOutput = field.maxInput;
                        field.outputTo = GetEmptyInputFrom();

                        field.isActive = false;
                        break;
                    case FIELDTYPE.Engine:
                        field.currentVoltage = 0.0f;
                        field.maxVoltage = 100.0f;

                        field.currentProduction = 3.0f;
                        field.currentResistance = 0.0f;
                        field.currentConsumption = 0.0f;

                        field.maxInput = 0.0f;
                        field.inputFrom = GetEmptyInputFrom();
                        field.maxOutput = 3.0f;
                        field.outputTo = GetEmptyInputFrom();

                        field.isActive = false;
                        break;
                    case FIELDTYPE.Lamp:
                        field.currentVoltage = 0.0f;
                        field.maxVoltage = 10.0f;

                        field.currentProduction = 0.0f;
                        field.currentResistance = 0.0f;
                        field.currentConsumption = 1.0f;

                        field.maxInput = 3.0f;
                        field.inputFrom = GetEmptyInputFrom();
                        field.maxOutput = 0.0f;
                        field.outputTo = GetEmptyInputFrom();

                        field.isActive = false;
                        break;
                    case FIELDTYPE.Nothing:
                        field.currentVoltage = 0.0f;
                        field.maxVoltage = 0.0f;

                        field.currentProduction = 0.0f;
                        field.currentResistance = 0.0f;
                        field.currentConsumption = 0.0f;

                        field.maxInput = 0.0f;
                        field.inputFrom = GetEmptyInputFrom();
                        field.maxOutput = 0.0f;
                        field.outputTo = GetEmptyInputFrom();

                        field.isActive = false;
                        break;
                }
                fields[row, col] = field;
            }
        }

        private void CheckNeighbours()
        {
            for (int row = 0; row <= fields.GetUpperBound(0); row++)
            {
                for (int col = 0; col <= fields.GetUpperBound(0); col++)
                {
                    FIELD field = fields[row, col];
                    field.hasNeighbour = new Dictionary<DIRECTION, bool>();
                    foreach (DIRECTION dir in Enum.GetValues(typeof(DIRECTION)))
                    {

                        switch (dir)
                        {
                            case DIRECTION.Left:
                                if (GetButton(row - 1, col) != null) field.hasNeighbour[dir] = true;
                                else field.hasNeighbour[dir] = false;
                                break;
                            case DIRECTION.Right:
                                if (GetButton(row + 1, col) != null) field.hasNeighbour[dir] = true;
                                else field.hasNeighbour[dir] = false;
                                break;
                            case DIRECTION.Top:
                                if (GetButton(row, col - 1) != null) field.hasNeighbour[dir] = true;
                                else field.hasNeighbour[dir] = false;
                                break;
                            case DIRECTION.Bottom:
                                if (GetButton(row, col + 1) != null) field.hasNeighbour[dir] = true;
                                else field.hasNeighbour[dir] = false;
                                break;
                        }
                    }
                    fields[row, col] = field;
                }
            }
        }

        private Dictionary<DIRECTION, float> GetEmptyInputFrom()
        {
            Dictionary<DIRECTION, float> dic = new Dictionary<DIRECTION, float>();
            for (int i = 0; i <= 3; i++)
            {
                dic[(DIRECTION)i] = 0.0f;
            }
            return dic;
        }

        private void Field_Click(object sender, MouseEventArgs e)
        {


            Button btn = (Button)sender;
            int row = int.Parse(btn.Name.Substring(btn.Name.IndexOf("_") - 1, 1));
            int col = int.Parse(btn.Name.Substring(btn.Name.IndexOf("_") + 1, 1));
            FIELD field = fields[row, col];

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                FIELDTYPE lastFieldType = Enum.GetValues(typeof(FIELDTYPE)).Cast<FIELDTYPE>().Last();
                FIELDTYPE firstFieldType = Enum.GetValues(typeof(FIELDTYPE)).Cast<FIELDTYPE>().First();

                if (field.type == lastFieldType)
                {
                    field.type = firstFieldType;
                }
                else
                {
                    int fieldtypeInt = Convert.ToInt32(field.type);
                    field.type = (FIELDTYPE)(Convert.ToInt32(field.type) + 1);

                }
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                field.type = FIELDTYPE.Nothing;
            }

            field.text = field.type.ToString();
            fields[row, col] = field;

            RefreshField(row, col);
            CheckNeighbours();
        }

        private void field0_0_MouseClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine("Test");
        }

        private void field0_0_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private Button GetButton(int row, int col)
        {
            Control[] controls = this.Controls.Find("field" + row.ToString() + "_" + col.ToString(), false);

            if (controls.Count() > 0) return (Button)controls[0];
            else return null;
        }

        private void SimulationsTimer_Tick(object sender, EventArgs e)
        {
            if (this.cbSimulationStatus.Checked)
            {
                for (int row = 0; row <= fields.GetUpperBound(0); row++)
                {
                    for (int col = 0; col <= fields.GetUpperBound(0); col++)
                    {
                        Button btn = GetButton(row, col);

                        if (btn != null)
                        {
                            FIELD field = fields[row, col];

                            if (field.type != FIELDTYPE.Nothing)
                            {
                                field.currentVoltage = Clamp(field.currentVoltage + field.currentProduction, 0, field.maxVoltage);

                                int countOutputs = 0;
                                foreach (DIRECTION dir in Enum.GetValues(typeof(DIRECTION))) { if (field.hasNeighbour[dir]) countOutputs++;}

                                foreach (DIRECTION dir in Enum.GetValues(typeof(DIRECTION)))
                                {
                                    //switch (dir)
                                    //{
                                    //    case DIRECTION.Left:
                                    //        field.currentVoltage -= AddVoltage(DIRECTION.Right, row - 1, col, abzugeben / (countOutputs-1));
                                    //        break;
                                    //    case DIRECTION.Right:
                                    //        field.currentVoltage -= AddVoltage(DIRECTION.Left, row + 1, col, abzugeben / 4);
                                    //        break;
                                    //    case DIRECTION.Top:
                                    //        field.currentVoltage -= AddVoltage(DIRECTION.Bottom, row, col - 1, abzugeben / 4);
                                    //        break;
                                    //    case DIRECTION.Bottom:
                                    //        field.currentVoltage -= AddVoltage(DIRECTION.Top, row, col + 1, abzugeben / 4);
                                    //        break;
                                    //}
                                    float kannBekommen = Clamp(field.maxVoltage - field.currentVoltage, 0, field.maxVoltage);
                                    float bekommt = Clamp(field.inputFrom[dir], 0, kannBekommen);

                                    field.currentVoltage += bekommt;
                                    Console.WriteLine(bekommt.ToString());
                                    field.inputFrom[dir] -= bekommt;
                                }

                                float abzugeben = Clamp(field.currentVoltage, 0, field.maxOutput);

                                foreach (DIRECTION dirFrom in Enum.GetValues(typeof(DIRECTION)))
                                {
                                    foreach (DIRECTION dirTo in Enum.GetValues(typeof(DIRECTION)))
                                    {
                                        if (dirFrom != dirTo)
                                        {
                                            field.outputTo[dirTo] = abzugeben / Clamp((countOutputs - 1),1,4);                                          
                                        }
                                    }
                                }

                                foreach (DIRECTION dirTo in Enum.GetValues(typeof(DIRECTION)))
                                {
                                    switch (dirTo)
                                    {
                                        case DIRECTION.Left:
                                            field.currentVoltage -= AddVoltage(DIRECTION.Right, row - 1, col, field.outputTo[dirTo]);
                                            break;
                                        case DIRECTION.Right:
                                            field.currentVoltage -= AddVoltage(DIRECTION.Left, row + 1, col, field.outputTo[dirTo]);
                                            break;
                                        case DIRECTION.Top:
                                            field.currentVoltage -= AddVoltage(DIRECTION.Bottom, row, col - 1, field.outputTo[dirTo]);
                                            break;
                                        case DIRECTION.Bottom:
                                            field.currentVoltage -= AddVoltage(DIRECTION.Top, row, col + 1, field.outputTo[dirTo]);
                                            break;
                                    }
                                }

                                //  field.currentVoltage = Clamp(field.currentVoltage - field.currentResistance, 0, field.currentVoltage);


                                btn.Text = field.currentVoltage.ToString() + " / " + field.maxVoltage.ToString();
                                if (field.isActive) { btn.BackColor = Color.Yellow; }
                                else { btn.BackColor = SystemColors.Control; }

                                fields[row, col] = field;
                            }

                        }
                    }
                }
            }
        }

        private float AddVoltage(DIRECTION fromDirection, int targetRow, int targetCol, float value)
        {
            float abgenommen = 0.0f;

            Control[] controls = this.Controls.Find("field" + targetRow.ToString() + "_" + targetCol.ToString(), false);
            if (controls.Count() > 0)
            {
                FIELD field = fields[targetRow, targetCol];
                Button btn = (Button)controls[0];
                float maxAbnahme = Clamp(field.maxInput - field.inputFrom[fromDirection], 0, field.maxInput);
                abgenommen = Clamp(value, 0, maxAbnahme);
                field.inputFrom[fromDirection] = abgenommen;
            }

            return abgenommen;

        }

        private float Clamp(float value, float min, float max)
        {
            if (value < min) return min;
            else if (value > max) return max;
            else return value;
        }

    }



}

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
        Block[,] fields;

        const int maxRowCol = 10;


        public Form1()
        {
            InitializeComponent();

            fields = new Block[maxRowCol, maxRowCol];

            for (int row = 0; row <= fields.GetUpperBound(0); row++)
            {
                for (int col = 0; col <= fields.GetUpperBound(0); col++)
                {
                    fields[row, col] = new Block(this, row, col, GetButton(row, col), Block.FIELDTYPE.Nothing); ;

                }
            }

            CheckAllNeighbours();

            //RefreshAllFields();
        }




        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void RefreshAllFields()
        {
            //for (int row = 0; row <= fields.GetUpperBound(0); row++)
            //{
            //    for (int col = 0; col <= fields.GetUpperBound(0); col++)
            //    {
            //        RefreshField(row, col);

            //    }
            //}
            //CheckNeighbours();
        }



        public Dictionary<Block.DIRECTION, float> GetEmptyInputFrom()
        {
            Dictionary<Block.DIRECTION, float> dic = new Dictionary<Block.DIRECTION, float>();
            for (int i = 0; i <= 3; i++)
            {
                dic[(Block.DIRECTION)i] = 0.0f;
            }
            return dic;
        }

        private void Field_Click(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            int row = int.Parse(btn.Name.Substring(btn.Name.IndexOf("_") - 1, 1));
            int col = int.Parse(btn.Name.Substring(btn.Name.IndexOf("_") + 1, 1));
            Block field = fields[row, col];


            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                field.ChangeBlockType();
            }
            else if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                field.ChangeBlockType(Block.FIELDTYPE.Nothing);
            }
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

        public void CheckAllNeighbours()
        {
            for (int row = 0; row <= fields.GetUpperBound(0); row++)
            {
                for (int col = 0; col <= fields.GetUpperBound(0); col++)
                {
                    CheckNeighbours(fields[row, col], false);
                }
            }
        }

        public void CheckNeighbours(Block field)
        {
            CheckNeighbours(field, true);
        }

        public void CheckNeighbours(Block field, Boolean shouldCheckNeighboursOfNeighbour)
        {


            foreach (Block.DIRECTION dir in Enum.GetValues(typeof(Block.DIRECTION)))
            {

                switch (dir)
                {
                    case Block.DIRECTION.Left:
                        if (field.col == 0) { field.neighbours[dir] = null; }
                        else { field.neighbours[dir] = fields[field.row - 1, field.col]; }
                        break;
                    case Block.DIRECTION.Right:
                        if (field.col == maxRowCol) { field.neighbours[dir] = null; }
                        else { field.neighbours[dir] = fields[field.row + 1, field.col]; }
                        break;
                    case Block.DIRECTION.Top:
                        if (field.row == 0) { field.neighbours[dir] = null; }
                        else { field.neighbours[dir] = fields[field.row, field.col - 1]; }
                        break;
                    case Block.DIRECTION.Bottom:
                        if (field.row == maxRowCol) { field.neighbours[dir] = null; }
                        else { field.neighbours[dir] = fields[field.row, field.col + 1]; }
                        break;
                }

            }



            if (shouldCheckNeighboursOfNeighbour)
            {
                for (int i = 0; i < Enum.GetNames(typeof(Block.DIRECTION)).Length; i++)
                {
                    if (field.neighbours[(Block.DIRECTION)i] != null)
                    {
                        CheckNeighbours(field.neighbours[(Block.DIRECTION)i], false);
                    }
                }
            }
        }


        private void SimulationsTimer_Tick(object sender, EventArgs e)
        {

            for (int row = 0; row <= fields.GetUpperBound(0); row++)
            {
                for (int col = 0; col <= fields.GetUpperBound(0); col++)
                {
                    fields[row, col].Update();

                }
            }
            //                        Button btn = GetButton(row, col);

            //            if (btn != null)
            //            {
            //                FIELD field = fields[row, col];

            //                if (field.type != FIELDTYPE.Nothing)
            //                {
            //                    field.currentVoltage = Clamp(field.currentVoltage + field.currentProduction, 0, field.maxVoltage);

            //                    int countOutputs = 0;
            //                    foreach (DIRECTION dir in Enum.GetValues(typeof(DIRECTION))) { if (field.hasNeighbour[dir]) countOutputs++; }

            //                    foreach (DIRECTION dir in Enum.GetValues(typeof(DIRECTION)))
            //                    {
            //                        //switch (dir)
            //                        //{
            //                        //    case DIRECTION.Left:
            //                        //        field.currentVoltage -= AddVoltage(DIRECTION.Right, row - 1, col, abzugeben / (countOutputs-1));
            //                        //        break;
            //                        //    case DIRECTION.Right:
            //                        //        field.currentVoltage -= AddVoltage(DIRECTION.Left, row + 1, col, abzugeben / 4);
            //                        //        break;
            //                        //    case DIRECTION.Top:
            //                        //        field.currentVoltage -= AddVoltage(DIRECTION.Bottom, row, col - 1, abzugeben / 4);
            //                        //        break;
            //                        //    case DIRECTION.Bottom:
            //                        //        field.currentVoltage -= AddVoltage(DIRECTION.Top, row, col + 1, abzugeben / 4);
            //                        //        break;
            //                        //}
            //                        float kannBekommen = Clamp(field.maxVoltage - field.currentVoltage, 0, field.maxVoltage);
            //                        float bekommt = Clamp(field.inputFrom[dir], 0, kannBekommen);

            //                        field.currentVoltage += bekommt;
            //                        Console.WriteLine(bekommt.ToString());
            //                        field.inputFrom[dir] -= bekommt;
            //                    }

            //                    float abzugeben = Clamp(field.currentVoltage, 0, field.maxOutput);

            //                    foreach (DIRECTION dirFrom in Enum.GetValues(typeof(DIRECTION)))
            //                    {
            //                        foreach (DIRECTION dirTo in Enum.GetValues(typeof(DIRECTION)))
            //                        {
            //                            if (dirFrom != dirTo)
            //                            {
            //                                field.outputTo[dirTo] = abzugeben / Clamp((countOutputs - 1), 1, 4);
            //                            }
            //                        }
            //                    }

            //                    foreach (DIRECTION dirTo in Enum.GetValues(typeof(DIRECTION)))
            //                    {
            //                        switch (dirTo)
            //                        {
            //                            case DIRECTION.Left:
            //                                field.currentVoltage -= AddVoltage(DIRECTION.Right, row - 1, col, field.outputTo[dirTo]);
            //                                break;
            //                            case DIRECTION.Right:
            //                                field.currentVoltage -= AddVoltage(DIRECTION.Left, row + 1, col, field.outputTo[dirTo]);
            //                                break;
            //                            case DIRECTION.Top:
            //                                field.currentVoltage -= AddVoltage(DIRECTION.Bottom, row, col - 1, field.outputTo[dirTo]);
            //                                break;
            //                            case DIRECTION.Bottom:
            //                                field.currentVoltage -= AddVoltage(DIRECTION.Top, row, col + 1, field.outputTo[dirTo]);
            //                                break;
            //                        }
            //                    }

            //                    //  field.currentVoltage = Clamp(field.currentVoltage - field.currentResistance, 0, field.currentVoltage);


            //                    btn.Text = field.currentVoltage.ToString() + " / " + field.maxVoltage.ToString();
            //                    if (field.isActive) { btn.BackColor = Color.Yellow; }
            //                    else { btn.BackColor = SystemColors.Control; }

            //                    fields[row, col] = field;
            //                }

            //            }
            //        }
            //    }
            //}
        }

        private void cbSimulationStatus_CheckedChanged(object sender, EventArgs e)
        {
            SimulationsTimer.Enabled = cbSimulationStatus.Checked;

            //if (this.cbSimulationStatus.Checked)
            //{
            //    for (int row = 0; row <= fields.GetUpperBound(0); row++)
            //    {
            //        for (int col = 0; col <= fields.GetUpperBound(0); col++)
            //        {
            //            Button btn = GetButton(row, col);

            //            if (btn != null)
            //            {
            //                FIELD field = fields[row, col];

            //                if (field.type != FIELDTYPE.Nothing)
            //                {
            //                    field.currentVoltage = Clamp(field.currentVoltage + field.currentProduction, 0, field.maxVoltage);

            //                    int countOutputs = 0;
            //                    foreach (DIRECTION dir in Enum.GetValues(typeof(DIRECTION))) { if (field.hasNeighbour[dir]) countOutputs++; }

            //                    foreach (DIRECTION dir in Enum.GetValues(typeof(DIRECTION)))
            //                    {
            //                        //switch (dir)
            //                        //{
            //                        //    case DIRECTION.Left:
            //                        //        field.currentVoltage -= AddVoltage(DIRECTION.Right, row - 1, col, abzugeben / (countOutputs-1));
            //                        //        break;
            //                        //    case DIRECTION.Right:
            //                        //        field.currentVoltage -= AddVoltage(DIRECTION.Left, row + 1, col, abzugeben / 4);
            //                        //        break;
            //                        //    case DIRECTION.Top:
            //                        //        field.currentVoltage -= AddVoltage(DIRECTION.Bottom, row, col - 1, abzugeben / 4);
            //                        //        break;
            //                        //    case DIRECTION.Bottom:
            //                        //        field.currentVoltage -= AddVoltage(DIRECTION.Top, row, col + 1, abzugeben / 4);
            //                        //        break;
            //                        //}
            //                        float kannBekommen = Clamp(field.maxVoltage - field.currentVoltage, 0, field.maxVoltage);
            //                        float bekommt = Clamp(field.inputFrom[dir], 0, kannBekommen);

            //                        field.currentVoltage += bekommt;
            //                        Console.WriteLine(bekommt.ToString());
            //                        field.inputFrom[dir] -= bekommt;
            //                    }

            //                    float abzugeben = Clamp(field.currentVoltage, 0, field.maxOutput);

            //                    foreach (DIRECTION dirFrom in Enum.GetValues(typeof(DIRECTION)))
            //                    {
            //                        foreach (DIRECTION dirTo in Enum.GetValues(typeof(DIRECTION)))
            //                        {
            //                            if (dirFrom != dirTo)
            //                            {
            //                                field.outputTo[dirTo] = abzugeben / Clamp((countOutputs - 1), 1, 4);
            //                            }
            //                        }
            //                    }

            //                    foreach (DIRECTION dirTo in Enum.GetValues(typeof(DIRECTION)))
            //                    {
            //                        switch (dirTo)
            //                        {
            //                            case DIRECTION.Left:
            //                                field.currentVoltage -= AddVoltage(DIRECTION.Right, row - 1, col, field.outputTo[dirTo]);
            //                                break;
            //                            case DIRECTION.Right:
            //                                field.currentVoltage -= AddVoltage(DIRECTION.Left, row + 1, col, field.outputTo[dirTo]);
            //                                break;
            //                            case DIRECTION.Top:
            //                                field.currentVoltage -= AddVoltage(DIRECTION.Bottom, row, col - 1, field.outputTo[dirTo]);
            //                                break;
            //                            case DIRECTION.Bottom:
            //                                field.currentVoltage -= AddVoltage(DIRECTION.Top, row, col + 1, field.outputTo[dirTo]);
            //                                break;
            //                        }
            //                    }

            //                    //  field.currentVoltage = Clamp(field.currentVoltage - field.currentResistance, 0, field.currentVoltage);


            //                    btn.Text = field.currentVoltage.ToString() + " / " + field.maxVoltage.ToString();
            //                    if (field.isActive) { btn.BackColor = Color.Yellow; }
            //                    else { btn.BackColor = SystemColors.Control; }

            //                    fields[row, col] = field;
            //                }

            //            }
            //        }
            //    }
            //}
        }

        //private float AddVoltage(DIRECTION fromDirection, int targetRow, int targetCol, float value)
        //{
        //    float abgenommen = 0.0f;

        //    Control[] controls = this.Controls.Find("field" + targetRow.ToString() + "_" + targetCol.ToString(), false);
        //    if (controls.Count() > 0)
        //    {
        //        FIELD field = fields[targetRow, targetCol];
        //        Button btn = (Button)controls[0];
        //        float maxAbnahme = Clamp(field.maxInput - field.inputFrom[fromDirection], 0, field.maxInput);
        //        abgenommen = Clamp(value, 0, maxAbnahme);
        //        field.inputFrom[fromDirection] = abgenommen;
        //    }

        //    return abgenommen;

        //}

    }



}

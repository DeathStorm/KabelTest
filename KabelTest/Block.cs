using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CT_Additions;
using System.Windows.Forms;

namespace KabelTest
{
    public class Block
    {
        private Button button;

        public enum DIRECTION { Left, Right, Top, Bottom };
        public enum FIELDTYPE { Nothing, Cable, Lamp, Engine };

        private string text = "";
        public  FIELDTYPE type = FIELDTYPE.Nothing;
        private float currentVoltage = 0.0f;
        private float maxVoltage = 0.0f;
        private float currentResistance = 0.0f;

        private bool canProduce = false;
        private float currentProduction = 0.0f;

        private bool canConsume = false;
        private float currentConsumption = 0;

        public Dictionary<DIRECTION, Block> neighbours = new Dictionary<DIRECTION, Block>();
        private int validNeighbours = 0;

        private float maxInput = 0.0f;
        private Dictionary<DIRECTION, float> inputFrom = new Dictionary<DIRECTION, float>();

        private float maxOutput = 0.0f;
        private Dictionary<DIRECTION, float> outputTo = new Dictionary<DIRECTION, float>();

        private bool isActive = false;

        private Form1 form1;
        public int row;
        public int col;

        

        public Block(Form1 form1, int row, int col, Button button, FIELDTYPE type)
        {
            this.button = button;
            this.type = type;
            this.text = type.ToString();
            this.form1 = form1;

            SetStandardConfiguration();
            for (int i = 0; i < Enum.GetNames(typeof(DIRECTION)).Length; i++){neighbours[(DIRECTION)i] = null;}
        }

        public void Update()
        {
            //form1.checkNeighbours(this);

            CalcCurrentVoltage();
            CountValidNeighbours();

            ProduceEnergy();
            ConsumeEnergy();


            CalcCurrentVoltage();
            RefreshTitle();
        }

        public void ChangeBlockType()
        {
            FIELDTYPE lastFieldType = Enum.GetValues(typeof(FIELDTYPE)).Cast<FIELDTYPE>().Last();
            FIELDTYPE firstFieldType = Enum.GetValues(typeof(FIELDTYPE)).Cast<FIELDTYPE>().First();

            if (type == lastFieldType)
            {
                ChangeBlockType(firstFieldType);
            }
            else
            {
                int fieldtypeInt = Convert.ToInt32(type);
                ChangeBlockType((FIELDTYPE)(Convert.ToInt32(type) + 1));

            }

            
        }

        public void ChangeBlockType(FIELDTYPE fieldType)
        {

            type = fieldType;
            RefreshTitle();

            form1.CheckNeighbours(this, true);
        }

        private void ProduceEnergy()
        {
            if (canProduce && validNeighbours > 0)
            {
                float energyPerOutput = CT.Clamp(currentProduction, currentProduction, maxVoltage - currentVoltage) / validNeighbours;

                for (int i = 0; i < Enum.GetNames(typeof(DIRECTION)).Length; i++)
                {
                    if (neighbours[(DIRECTION)i] != null && neighbours[(DIRECTION)i].type != FIELDTYPE.Nothing)
                    {
                        outputTo[(DIRECTION)i] = energyPerOutput;
                    }
                }
            }


        }

        private void ConsumeEnergy()
        {
            if (canConsume && currentVoltage >= currentConsumption)
            {
                currentVoltage -= currentConsumption;
                isActive = true;
            }
            else
            {
                isActive = false;
            }
        }

        private void CalcCurrentVoltage()
        {
            currentVoltage = 0.0f;
            for (int i = 0; i < Enum.GetNames(typeof(DIRECTION)).Length; i++)
            {
                currentVoltage += inputFrom[(DIRECTION)i];
                currentVoltage += outputTo[(DIRECTION)i];
            }
        }

        private void CountValidNeighbours()
        {
            validNeighbours = 0;
            for (int i = 0; i < Enum.GetNames(typeof(DIRECTION)).Length; i++)
            {
                if (neighbours[(DIRECTION)i] != null && neighbours[(DIRECTION)i].type != FIELDTYPE.Nothing) { validNeighbours++; }
            }

        }


        private void RefreshTitle()
        {
            this.text = type.ToString().Substring(0, 3) + row.ToString()+"-" + col.ToString() +"[" + validNeighbours.ToString() + "]\n" + currentVoltage.ToString("0.00") + "\n(" + maxVoltage.ToString("0.00") + ")";

            if (button != null) button.Text = text;
        }

        private void SetStandardConfiguration()
        {
            switch (type)
            {
                case FIELDTYPE.Cable:
                    currentVoltage = 0.0f;
                    maxVoltage = 5.0f;

                    currentProduction = 0.0f;
                    currentResistance = 0.5f;
                    currentConsumption = 0.0f;

                    maxInput = 3.0f;
                    maxOutput = maxInput;

                    isActive = false;
                    break;
                case FIELDTYPE.Engine:
                    currentVoltage = 0.0f;
                    maxVoltage = 100.0f;

                    currentProduction = 3.0f;
                    currentResistance = 0.0f;
                    currentConsumption = 0.0f;

                    maxInput = 0.0f;
                    maxOutput = 3.0f;

                    isActive = false;
                    break;
                case FIELDTYPE.Lamp:
                    currentVoltage = 0.0f;
                    maxVoltage = 10.0f;

                    currentProduction = 0.0f;
                    currentResistance = 0.0f;
                    currentConsumption = 1.0f;

                    maxInput = 3.0f;
                    maxOutput = 0.0f;

                    isActive = false;
                    break;
                case FIELDTYPE.Nothing:
                    currentVoltage = 0.0f;
                    maxVoltage = 0.0f;

                    currentProduction = 0.0f;
                    currentResistance = 0.0f;
                    currentConsumption = 0.0f;

                    maxInput = 0.0f;
                    maxOutput = 0.0f;

                    isActive = false;
                    break;
            }

            for (int i = 0; i < Enum.GetNames(typeof(DIRECTION)).Length; i++)
            {
                DIRECTION direction = (DIRECTION)i;
                inputFrom[direction] = 0.0f;
                outputTo[direction] = 0.0f;
                //neighbours[direction] = null;
            }
        }

    }
}

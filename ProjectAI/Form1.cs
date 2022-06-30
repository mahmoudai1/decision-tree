using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProjectAI
{
    public partial class Form1 : Form
    {
        Bitmap off;
        Timer t = new Timer();
        

        class Attributes
        {
            public String name;
            public double iGained;
        }

        class UniqueRows
        {
            public String name;
            public double entropy;
            public double ct;
        }

        class InBetweenLines
        {
            public int X, Y, W, H;
        }

        public class AttributesTree
        {
            public String name;
            public List<Branches> bList = new List<Branches>();
        }

        public class Branches
        {
            public String name;
            public String end;
            public List<AttributesTree> aList = new List<AttributesTree>();
        }

        public static List<AttributesTree> finalList = new List<AttributesTree>();

        public Form1()
        {
            InitializeComponent();
            this.MouseDown += Form1_MouseDown;
            this.MouseMove += Form1_MouseMove;
            this.MouseUp += Form1_MouseUp;
            this.Paint += Form1_Paint;
            t.Tick += T_Tick;
            t.Start();

        }
        List<Attributes> pList = new List<Attributes>();
        List<List<String>> rowsList = new List<List<String>>();
        List<List<UniqueRows>> uniqueList = new List<List<UniqueRows>>();
        List<TextBox> myTextboxList = new List<TextBox>();
        List<Label>  myHeaderLabels = new List<Label>();
        List<InBetweenLines> lineList = new List<InBetweenLines>();

       

        List<Attributes> tempPList;
        List<List<String>> tempRowsList = new List<List<String>>();
        List<List<UniqueRows>> tempUniqueList;

        

        int ctTimer = 0;
        int inputNumber = 1;
        int theme1Width = 0;
        double allEntropy = 0;
        double tempAllEntropy = 0;
        String tempName = "";
        String[] AttributesArray = { "Day", "Outlook", "Temperature", "Humidity", "Wind", "Play Tennis" };
        //String[] AttributesArray = { "L-Sensor", "R-Sensor", "F-Sensor", "B-Sensor", "P-Action", "Action" };
        //String[] AttributesArray = { "E", "Size", "Color", "Shape", "Class"};

        // *********** THIS IS A READY DATASET BECAUSE OF TIME, BUT IT THE DATA CAN MANUALLY INSERTED WITH NO PROBLEMS *********/////////

        string[][] dataArray = new string[][] {    new string[]   { "D1", "Sunny",        "Hot",  "High",     "Weak",     "No" },
                                                   new string[]   { "D2", "Sunny",        "Hot",  "High",     "Strong",   "No" },
                                                   new string[]   { "D3", "Overcast",     "Hot",  "High",     "Weak",     "Yes" },
                                                   new string[]   { "D4", "Rain",         "Mild", "High",     "Weak",     "Yes" },
                                                   new string[]   { "D5", "Rain",         "Cool", "Normal",   "Weak",     "Yes" },
                                                   new string[]   { "D6", "Rain",         "Cool", "Normal",   "Strong",   "No" },
                                                   new string[]   { "D7", "Overcast",     "Cool", "Normal",   "Strong",   "Yes" },
                                                   new string[]   { "D8", "Sunny",        "Mild", "High",     "Weak",     "No" },
                                                   new string[]   { "D9", "Sunny",        "Cool", "Normal",   "Weak",     "Yes" },
                                                   new string[]   { "D10", "Rain",        "Mild", "Normal",   "Weak",     "Yes" },
                                                   new string[]   { "D11", "Sunny",       "Mild", "Normal",   "Strong",   "Yes" },
                                                   new string[]   { "D12", "Overcast",    "Mild", "High",     "Strong",   "Yes" },
                                                   new string[]   { "D13", "Overcast",    "Hot",  "Normal",   "Weak",     "Yes" },
                                                   new string[]   { "D14", "Rain",        "Mild", "High",     "Strong",   "No" } };

        /*string[][] dataArray = new string[][] {  new string[]   { "Obstacle",   "Free",        "Obstacle",  "Free",         "moveForward",      "turnRight"},
                                                   new string[]   { "Free",       "Free",        "Obstacle",  "Free",         "turnLeft",         "turnLeft"},
                                                   new string[]   { "Free",       "Obstacle",    "Free",      "Free",         "moveForward",      "moveForward"},
                                                   new string[]   { "Free",       "Obstacle",    "Free",      "Obstacle",     "turnLeft",         "moveForward"},
                                                   new string[]   { "Obstacle",   "Free",        "Free",      "Free",         "turnRight",        "moveForward"},
                                                   new string[]   { "Free",       "Free",        "Free",      "Obstacle",     "turnRight",        "moveForward"}};*/

        /*string[][] dataArray = new string[][] {  new string[]   { "E1", "Medium",    "Blue",    "Brick",        "Yes" },
                                                 new string[]   { "E2", "Small",     "Red",     "Sphere",       "Yes" },
                                                 new string[]   { "E3", "Large",     "Green",   "Pillar",       "Yes" },
                                                 new string[]   { "E4", "Large",     "Green",   "Sphere",       "Yes" },
                                                 new string[]   { "E5", "Small",     "Red",     "Wedge",        "No" },
                                                 new string[]   { "E6", "Large",     "Red",     "Wedge",        "No" },
                                                 new string[]   { "E7", "Large",     "Red",     "Pillar",       "No" } };*/
        
        int[] descArray;
        int[] tempDescArray;
        TextBox newTextBox = new TextBox();
        int justOnce = 0;
        

        private void Form1_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.Width, this.Height);
            // un-comment all the below functions to use the above ready dataset (except the double commented lines)
            /*assignSavedData();
            changeTheme1();
            changeTheme2();
            setUniqueRows(pList, rowsList, uniqueList);
            //displayStageOneInConsole();
            calculateAllEntropy(rowsList, uniqueList, ref allEntropy);
            calculateEachEntropy(pList, rowsList, uniqueList);
            calculateIGain(pList, rowsList, uniqueList, ref allEntropy);
            descIGain(pList, ref descArray, ref allEntropy);
            createDecisionTree();
            Console.WriteLine();
            //createTheToolsForDrawing(finalList);*/

        }

        void createTheToolsForDrawing(List<AttributesTree> finalList, String[] text)
        {

            for (int i = 0; i < finalList.Count; i++)
            {
                if (finalList[i].bList.Count > 0 && (finalList[i].bList[0].aList.Count > 0 || finalList[i].bList[0].end != null))
                {
                    Console.WriteLine("HEAD: " + finalList[i].name);
                    for (int j = 0; j < finalList[i].bList.Count; j++)
                    {
                        Console.WriteLine(" --> " + finalList[i].bList[j].name);
                        if (finalList[i].bList[j].aList.Count > 0)
                        {
                            createTheToolsForDrawing(finalList[i].bList[j].aList, text);

                        }
                        else
                        {
                            // put the if for calssification here
                            if (justOnce == 0)
                            {
                                for (int k = 0; k < text.Length; k++)
                                {
                                    if (text[k] == finalList[i].bList[j].name)
                                    {
                                        MessageBox.Show(finalList[i].bList[j].end, "Classification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        justOnce = 1;
                                        break;
                                    }
                                }
                            }
                            Console.WriteLine(" END: " + finalList[i].bList[j].end);
                        }

                    }
                    Console.WriteLine();
                }
                Console.WriteLine();
            }
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            drawDouble(e.Graphics);
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void T_Tick(object sender, EventArgs e)
        {
            ctTimer++;
            drawDouble(this.CreateGraphics());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && inputNumber == 1)
            {
                Attributes pnn = new Attributes();
                pnn.name = textBox1.Text;
                pnn.iGained = 0;
                pList.Add(pnn);
                label2.Text = "Predictor " + (pList.Count() + 1) + ":";
                textBox1.Text = "";
            }

            int f = 1;
            if(inputNumber == 2)
            {
                for (int i = 0; i < pList.Count; i++)
                {
                    if(myTextboxList[i].Text == "")
                    {
                        f = 0;
                        break;
                    }
                }

                if(f == 1)
                {
                    List<String> tempList = new List<String>();
                    
                    for (int i = 0; i < pList.Count; i++)
                    {
                        tempList.Add(myTextboxList[i].Text.ToLower());
                    }
                    rowsList.Add(tempList);

                    for(int i = 0; i < pList.Count; i++)
                    {
                        myTextboxList[i].Text = "";
                    }

                    label_ct_rows.Text = "Number of rows: " + rowsList.Count;
                }
                else
                {
                    MessageBox.Show("Some text is empty.");
                   
                }
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pList.Count >= 1 && inputNumber == 1)
            {
                changeTheme1();
                inputNumber = 2;
            }

            if(inputNumber == 2 && rowsList.Count >= 1)
            {
                setUniqueRows(pList, rowsList, uniqueList);
                displayStageOneInConsole();
                calculateAllEntropy(rowsList, uniqueList, ref allEntropy);
                calculateEachEntropy(pList, rowsList, uniqueList);
                calculateIGain(pList, rowsList, uniqueList, ref allEntropy);
                descIGain(pList, ref descArray, ref allEntropy);
                changeTheme2();
                createDecisionTree();

            }
        }

        private void newButton_click(object sender, EventArgs e)
        {
            Tree form = new Tree();
            form.ShowDialog();
        }

        private void newButton2_click(object sender, EventArgs e)
        {
            string[] classifyText = newTextBox.Text.Split(' ');
            justOnce = 0;
            createTheToolsForDrawing(finalList, classifyText);
        }

        public void changeTheme1()
        {
            int vX = 20;
            textBox1.Visible = false;
            label2.Visible = false;
            for (int i = 0; i < pList.Count; i++)
            {
                Label newLabel = new Label();
                newLabel.Font = new System.Drawing.Font("Calibri", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                newLabel.BackColor = System.Drawing.Color.Transparent;
                newLabel.ForeColor = System.Drawing.Color.Black;
                newLabel.AutoSize = true;
                newLabel.Location = new System.Drawing.Point(vX, 125);
                newLabel.Name = "label" + (i + 3);
                newLabel.Size = new System.Drawing.Size(123, 36);
                newLabel.TabIndex = i + 4;
                newLabel.Text = pList[i].name;
                myHeaderLabels.Add(newLabel);
                this.Controls.Add(newLabel);

                TextBox newTextBox = new TextBox();
                newTextBox.Font = new System.Drawing.Font("Calibri", 16.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                newTextBox.Location = new System.Drawing.Point(vX, 180);
                newTextBox.Multiline = true;
                newTextBox.Name = "textBox"+ (i + 2);
                newTextBox.Size = new System.Drawing.Size(260, 63);
                newTextBox.TabIndex = 2;
                this.Controls.Add(newTextBox);
                myTextboxList.Add(newTextBox);
                vX += 300;
            }

            label_ct_rows.Font = new System.Drawing.Font("Calibri", 10.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label_ct_rows.Location = new System.Drawing.Point(20, 270);
            label_ct_rows.Text = "Number of rows: " + rowsList.Count;
            label_ct_rows.Visible = true;

            theme1Width = vX;
            this.Size = new Size(vX, 410);
            off = new Bitmap(this.Width, this.Height);
            button1.Location = new Point(vX - 300, 250);
            button2.Location = new Point(vX - 210, 250);
        }

        public void changeTheme2()
        {
            label_ct_rows.Visible = false;
            for (int i = 0; i < myTextboxList.Count; i++)
            {
                myTextboxList[i].Visible = false;
            }

            for(int i = 0;i < myHeaderLabels.Count; i++)
            {
                myHeaderLabels[i].Font = new Font("Calibri", 16.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            }

            button1.Visible = false;
            button2.Visible = false;

            int vX = 20;
            int vY = 200;
            InBetweenLines pnn = null;
            for (int i = 0; i < rowsList.Count(); i++)
            {
                pnn = new InBetweenLines();
                pnn.X = 0;
                pnn.Y = vY - 15;
                pnn.W = theme1Width; //
                pnn.H = 2;
                lineList.Add(pnn);

                pnn = new InBetweenLines();
                pnn.X = 0;
                pnn.Y = 120;
                pnn.W = theme1Width; //
                pnn.H = 2;
                lineList.Add(pnn);

                for (int ii = 0; ii < rowsList[i].Count(); ii++)
                {
                    Label newLabel = new Label();
                    newLabel.Font = new System.Drawing.Font("Calibri", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    newLabel.BackColor = System.Drawing.Color.Transparent;
                    newLabel.ForeColor = System.Drawing.Color.Black;
                    newLabel.AutoSize = true;
                    newLabel.Location = new System.Drawing.Point(vX, vY);
                    newLabel.Name = "label" + (i + 10);
                    newLabel.Size = new System.Drawing.Size(123, 36);
                    newLabel.TabIndex = i + 4;
                    newLabel.Text = rowsList[i][ii];
                    this.Controls.Add(newLabel);
                    vX += 310;
                }
                vX = 20;
                vY += 70;
            }

            pnn = new InBetweenLines();
            pnn.X = 0;
            pnn.Y = vY - 15;
            pnn.W = theme1Width; //
            pnn.H = 2;
            lineList.Add(pnn);

            Button newButton = new Button();
            newButton.Font = new System.Drawing.Font("Calibri", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            newButton.Location = new System.Drawing.Point(20, vY);
            newButton.Name = "buttonTree";
            newButton.Size = new System.Drawing.Size(300, 80);
            newButton.Text = "Generate Tree";
            newButton.Cursor = System.Windows.Forms.Cursors.Hand;
            newButton.Click += new System.EventHandler(this.newButton_click);
            this.Controls.Add(newButton);

            newTextBox.Font = new System.Drawing.Font("Calibri", 16.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            newTextBox.Location = new System.Drawing.Point(400, vY + 10);
            newTextBox.Name = "textBoxClassify";
            newTextBox.Size = new System.Drawing.Size(500, 400);
            this.Controls.Add(newTextBox);

            newButton = new Button();
            newButton.Font = new System.Drawing.Font("Calibri", 12.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            newButton.Location = new System.Drawing.Point(915, vY + 8);
            newButton.Name = "buttonTree";
            newButton.Size = new System.Drawing.Size(200, 65);
            newButton.Text = "Classify";
            newButton.Cursor = System.Windows.Forms.Cursors.Hand;
            newButton.Click += new System.EventHandler(this.newButton2_click);
            this.Controls.Add(newButton);

            this.Size = new Size(theme1Width + 30, (vY + 100) + 80);
            off = new Bitmap(this.Width, this.Height);
        }

        public void assignSavedData()
        {
            for(int i = 0; i < AttributesArray.Length; i++)
            {
                Attributes pnn = new Attributes();
                pnn.name = AttributesArray[i];
                pnn.iGained = 0;
                pList.Add(pnn);
            }


            List<String> tempList = null;
            for (int i = 0; i < dataArray.Length; i++)
            {
                tempList = new List<String>();
                for (int ii = 0; ii < dataArray[i].Length; ii++)
                {
                    tempList.Add(dataArray[i][ii]);
                }
                rowsList.Add(tempList);
            }
            
        }

        void setUniqueRows(List<Attributes> pList, List<List<String>> rowsList, List<List<UniqueRows>> uniqueList)
        {
            for (int i = 0; i < pList.Count; i++)
            {
                List<UniqueRows> tempList = new List<UniqueRows>();
                for (int ii = 0; ii < rowsList.Count; ii++)
                {
                    int f = 1;
                    for (int iii = 0; iii < tempList.Count; iii++)
                    {
                        if(rowsList[ii][i] == tempList[iii].name)
                        {
                            f = 0;
                            break;
                        }
                    }

                    if (f == 1)
                    {
                        UniqueRows pnn = new UniqueRows();
                        pnn.name = rowsList[ii][i];
                        pnn.entropy = 0;
                        tempList.Add(pnn);
                    }

                }
                uniqueList.Add(tempList);
            }
        }

        void calculateAllEntropy(List<List<String>> rowsList, List<List<UniqueRows>> uniqueList, ref double allEntropy)
        {
            int[] classAttributes = new int[uniqueList[uniqueList.Count - 1].Count]; //uniqueList[uniqueList.Count - 1].Count
            double[] classAttributesAverage = new double[uniqueList[uniqueList.Count - 1].Count]; //uniqueList[uniqueList.Count - 1].Count

            for (int i = 0; i < rowsList.Count; i++)
            {
                for(int ii = rowsList[i].Count - 1; ii < rowsList[i].Count; ii++)
                {
                    for (int iii = 0; iii < uniqueList[uniqueList.Count - 1].Count; iii++)
                    {
                        if (rowsList[i][ii] == uniqueList[uniqueList.Count - 1][iii].name)
                        {
                            classAttributes[iii]++;
                        }
                    }
                }
            }

            double totalAttributes = 0.0;
            for (int i = 0; i < classAttributes.Length && classAttributes[i] != '\0'; i++)
            {
                totalAttributes += classAttributes[i];
            }

            for(int i = 0; i < classAttributes.Length && classAttributes[i] != '\0'; i++)
            {
                classAttributesAverage[i] = classAttributes[i] / totalAttributes;
            }

            for(int i = 0; i < classAttributesAverage.Length && classAttributesAverage[i]!= '\0'; i++)
            {
                allEntropy += -classAttributesAverage[i] * Math.Log(classAttributesAverage[i], 2);
            }

            allEntropy = Math.Round(allEntropy, 4);

            //allEntropy = -avgFirst * Math.Log(avgFirst, 2) - avgSecond * Math.Log(avgSecond, 2);
            Console.WriteLine();
            //Console.WriteLine("Entropy for all = " + allEntropy);
            Console.WriteLine();
        }

        void calculateEachEntropy(List<Attributes> pList, List<List<String>> rowsList, List<List<UniqueRows>> uniqueList)
        {
            int[] entropyEach = new int[uniqueList[uniqueList.Count - 1].Count];     //---

            for(int i = 0; i < uniqueList.Count && i < pList.Count; i++)
            {
                for(int ii = 0; ii < uniqueList[i].Count; ii++)
                {
                    for (int iv = 0; iv < uniqueList[uniqueList.Count - 1].Count; iv++)    // 1- No
                    {
                        for (int iii = 0; iii < rowsList.Count; iii++)
                        {
                            if (rowsList[iii][i] == uniqueList[i][ii].name)       // 1- sunny
                            {
                                //Console.WriteLine(rowsList[iii][i]);
                                if (rowsList[iii][rowsList[iii].Count - 1] == uniqueList[uniqueList.Count - 1][iv].name)
                                {
                                    entropyEach[iv]++;
                                }
                            }
                        }
                    }
                    //Console.WriteLine(uniqueList[i][ii].name + "  " + entropyEach[0] + "   " + entropyEach[1]);

                    double totalEntropies = 0;
                    for (int v = 0; v < entropyEach.Length; v++)
                    {
                        totalEntropies += entropyEach[v];
                    }
                    uniqueList[i][ii].ct = totalEntropies;
                    
                    for (int v = 0; v < entropyEach.Length ; v++)
                    {
                        if (entropyEach[v] != 0)
                        {
                            double tempAvg = entropyEach[v] / totalEntropies;
                            uniqueList[i][ii].entropy += Math.Round(-tempAvg * Math.Log(tempAvg, 2), 3);
                        }
                    }
                    
                    for(int v = 0; v < entropyEach.Length; v++)
                    {
                        entropyEach[v] = 0;
                    }

                    //Console.WriteLine(uniqueList[i][ii].name + " Entropy = " + uniqueList[i][ii].entropy + "  Repeated = " + uniqueList[i][ii].ct );
                }
            }
        }

        void calculateIGain(List<Attributes> pList, List<List<String>> rowsList, List<List<UniqueRows>> uniqueList, ref double allEntropy)
        {
            Console.WriteLine();
            double I = 0;
            for(int i = 0; i < uniqueList.Count; i++)
            {
                for(int ii = 0; ii < uniqueList[i].Count; ii++)
                {
                    I += (uniqueList[i][ii].ct / rowsList.Count) * uniqueList[i][ii].entropy;
                }

                pList[i].iGained = Math.Round(allEntropy - I, 4);
                if (pList[i].iGained == allEntropy)
                {
                    //pList.Remove(pList[i]);
                    //j--;
                }
                else
                {
                    
                }
                //Console.WriteLine("Information Gained for " + pList[i].name + " = " + pList[i].iGained);
                I = 0;
            }
            Console.WriteLine();
        }

        void descIGain(List<Attributes> pList, ref int[] descArray, ref double allEntropy)
        {
            int tempCt = 0;
            for(int i = 0; i < pList.Count; i++)
            {
                if(pList[i].iGained != allEntropy)
                {
                    tempCt++;
                }
            }

            descArray = new int[tempCt];
            
            for (int i = 0; i < descArray.Length; i++)
            {
                double max = -99999.0;
                int iWhich = -1;
                for(int ii = 0; ii < pList.Count; ii++)
                {
                    if (checkIfItsAlreadyCompared(ii) && pList[ii].iGained != allEntropy)
                    {
                        if (pList[ii].iGained > max)
                        {
                            //MessageBox.Show(pList[ii].iGained + "");
                            max = pList[ii].iGained;
                            iWhich = ii;
                        }
                        //MessageBox.Show(i + "  " + ii);
                    }
                    
                }
                descArray[i] = iWhich;
            }

            Console.WriteLine();
           // Console.WriteLine("Descending Order for Information Gained:");

            for (int i = 0; i < descArray.Length; i++)
            {
                //Console.WriteLine(descArray[i]);
            }

        }

        public bool checkIfItsAlreadyCompared(int ii)
        {
            for(int i = 0; i < descArray.Length; i++)
            {
                if (descArray[i] == ii)            // <======== && != '\0'
                    return false;
            }
            return true;
        }

        /*
        void tempSetUniqueRows()
        {
            for (int i = 0; i < tempPList.Count; i++)
            {
                List<UniqueRows> tempList = new List<UniqueRows>();
                for (int ii = 0; ii < tempRowsList.Count; ii++)
                {
                    int f = 1;
                    for (int iii = 0; iii < tempList.Count; iii++)
                    {
                        if (tempRowsList[ii][i] == tempList[iii].name)
                        {
                            f = 0;
                            break;
                        }
                    }

                    if (f == 1)
                    {
                        UniqueRows pnn = new UniqueRows();
                        pnn.name = tempRowsList[ii][i];
                        pnn.entropy = 0;
                        tempList.Add(pnn);
                    }

                }
                tempUniqueList.Add(tempList);
            }

            /*for(int i = 0; i < uniqueList.Count; i++)
            {
                for(int ii = 0; ii < uniqueList[i].Count; ii++)
                {
                    Console.WriteLine(uniqueList[i][ii]);
                }
            }
    }



    void tempCalculateAllEntropy()
        {
            int[] classAttributes = new int[tempUniqueList[tempUniqueList.Count - 1].Count]; //uniqueList[uniqueList.Count - 1].Count
            double[] classAttributesAverage = new double[tempUniqueList[tempUniqueList.Count - 1].Count]; //uniqueList[uniqueList.Count - 1].Count

            for (int i = 0; i < tempRowsList.Count; i++)
            {
                for (int ii = tempRowsList[i].Count - 1; ii < tempRowsList[i].Count; ii++)
                {
                    for (int iii = 0; iii < tempUniqueList[tempUniqueList.Count - 1].Count; iii++)
                    {
                        if (tempRowsList[i][ii] == tempUniqueList[tempUniqueList.Count - 1][iii].name)
                        {
                            classAttributes[iii]++;
                        }
                    }
                }
            }

            double totalAttributes = 0.0;
            for (int i = 0; i < classAttributes.Length && classAttributes[i] != '\0'; i++)
            {
                totalAttributes += classAttributes[i];
            }

            for (int i = 0; i < classAttributes.Length && classAttributes[i] != '\0'; i++)
            {
                classAttributesAverage[i] = classAttributes[i] / totalAttributes;
            }

            for (int i = 0; i < classAttributesAverage.Length && classAttributesAverage[i] != '\0'; i++)
            {
                tempAllEntropy += -classAttributesAverage[i] * Math.Log(classAttributesAverage[i], 2);
            }

            tempAllEntropy = Math.Round(tempAllEntropy, 4);

            //allEntropy = -avgFirst * Math.Log(avgFirst, 2) - avgSecond * Math.Log(avgSecond, 2);
            Console.WriteLine();
            Console.WriteLine("Entropy for all = " + tempAllEntropy);
            Console.WriteLine();
        }

        void tempCalculateEachEntropy()
        {
            int[] entropyEach = new int[tempUniqueList[tempUniqueList.Count - 1].Count];     //---

            for (int i = 0; i < tempUniqueList.Count && i < tempPList.Count; i++)
            {
                for (int ii = 0; ii < tempUniqueList[i].Count; ii++)
                {
                    for (int iv = 0; iv < tempUniqueList[tempUniqueList.Count - 1].Count; iv++)    // 1- No
                    {
                        for (int iii = 0; iii < tempRowsList.Count; iii++)
                        {
                            if (tempRowsList[iii][i] == tempUniqueList[i][ii].name)       // 1- sunny
                            {
                                //Console.WriteLine(rowsList[iii][i]);
                                if (tempRowsList[iii][tempRowsList[iii].Count - 1] == tempUniqueList[tempUniqueList.Count - 1][iv].name)
                                {
                                    entropyEach[iv]++;
                                }
                            }
                        }
                    }
                    //Console.WriteLine(uniqueList[i][ii].name + "  " + entropyEach[0] + "   " + entropyEach[1]);

                    double totalEntropies = 0;
                    for (int v = 0; v < entropyEach.Length; v++)
                    {
                        totalEntropies += entropyEach[v];
                    }
                    tempUniqueList[i][ii].ct = totalEntropies;

                    for (int v = 0; v < entropyEach.Length; v++)
                    {
                        if (entropyEach[v] != 0)
                        {
                            double tempAvg = entropyEach[v] / totalEntropies;
                            tempUniqueList[i][ii].entropy += Math.Round(-tempAvg * Math.Log(tempAvg, 2), 3);
                        }
                    }

                    for (int v = 0; v < entropyEach.Length; v++)
                    {
                        entropyEach[v] = 0;
                    }

                    Console.WriteLine(tempUniqueList[i][ii].name + " Entropy = " + tempUniqueList[i][ii].entropy + "  Repeated = " + tempUniqueList[i][ii].ct);
                }
            }
        }

        void tempCalculateIGain()
        {
            Console.WriteLine();
            double I = 0;
            for (int i = 0; i < tempUniqueList.Count; i++)
            {
                for (int ii = 0; ii < tempUniqueList[i].Count; ii++)
                {
                    I += (tempUniqueList[i][ii].ct / tempRowsList.Count) * tempUniqueList[i][ii].entropy;
                }
                tempPList[i].iGained = Math.Round(tempAllEntropy - I, 4);
                Console.WriteLine("Information Gained for " + tempPList[i].name + " = " + tempPList[i].iGained);
                I = 0;
            }
            Console.WriteLine();
        }

        void tempDescIGain()
        {
            int tempCt = 0;
            for (int i = 0; i < tempPList.Count; i++)
            {
                if (tempPList[i].iGained != tempAllEntropy)
                {
                    tempCt++;
                }
            }

            tempDescArray = new int[tempCt];

            for (int i = 0; i < tempDescArray.Length; i++)
            {
                double max = -999999.0;
                int iWhich = -1;
                for (int ii = 0; ii < tempPList.Count; ii++)
                {
                    if (tempCheckIfItsAlreadyCompared(ii) && tempPList[ii].iGained != tempAllEntropy)
                    {
                        if (tempPList[ii].iGained > max)
                        {
                            max = tempPList[ii].iGained;
                            iWhich = ii;
                        }
                    }
                }
                tempDescArray[i] = iWhich;
            }

            Console.WriteLine();
            Console.WriteLine("Descending Order for Information Gained:");

            for (int i = 0; i < tempDescArray.Length; i++)
            {
                Console.WriteLine(tempDescArray[i]);
            }

        }

        public bool tempCheckIfItsAlreadyCompared(int ii)
        {
            for (int i = 0; i < tempDescArray.Length; i++)
            {
                if (tempDescArray[i] == ii)
                    return false;
            }
            return true;
        }

        */

        public void createDecisionTree()
        {
            int finalBreak = 0;
            for(int i = 0; i < descArray.Length; i++)
            {
                for(int ii = 0; ii < pList.Count; ii++)
                {
                    if(ii == descArray[i])
                    {
                        AttributesTree pnn1 = new AttributesTree();
                        pnn1.name = pList[ii].name;
                        //Console.WriteLine("HEAD: " + pnn1.name);//
                        pnn1.bList = new List<Branches>();
                        int numberOfPureSets = 0;
                        for (int iii = 0; iii < uniqueList[ii].Count; iii++)
                        {
                            Branches pnn2 = new Branches();
                            if (uniqueList[ii][iii].entropy == 0)
                            {
                                numberOfPureSets++;
                                //MessageBox.Show("" + numberOfPureSets);
                                // pureset
                                pnn2.name = uniqueList[ii][iii].name;
                                //Console.WriteLine(pnn2.name);//
                                int b = 0;
                                for(int iv = 0; iv < rowsList.Count; iv++)
                                {
                                    for(int v = 0; v < rowsList[iv].Count; v++)
                                    {
                                        if (rowsList[iv][v] == pnn2.name && v == ii)            // <====== && v == ii Fixed a bug :D  Because if the name exist in another column before it arrives to the right column
                                        {
                                            pnn2.end = rowsList[iv][rowsList[iv].Count - 1];
                                            //Console.WriteLine(pnn2.end);//
                                            Console.WriteLine();
                                            b = 1;
                                            break;
                                        }
                                    }
                                    if (b == 1) break;
                                }
                                if (numberOfPureSets == uniqueList[ii].Count)
                                {
                                    //MessageBox.Show("" + numberOfPureSets);
                                    finalBreak = 1;
                                }

                            }
                            else
                            {
                                // create new dataset <------------ The hardest
                                pnn2.name = uniqueList[ii][iii].name;
                                tempRowsList = new List<List<string>>(rowsList);
                                tempUniqueList = new List<List<UniqueRows>>();
                                tempPList = new List<Attributes>(pList);
                                tempAllEntropy = 0;

                                if(tempDescArray != null)
                                    Array.Clear(tempDescArray, 0, tempDescArray.Length);

                                //pnn2.end = "";
                                int j = i + 1;
                                if (j < descArray.Length)
                                {
                                    
                                    for (; ; )
                                    {
                                        //Console.WriteLine(pnn2.name);//
                                        createNewDataset(pnn2.name, tempPList);

                                        setUniqueRows(tempPList, tempRowsList, tempUniqueList);
                                        calculateAllEntropy(tempRowsList, tempUniqueList, ref tempAllEntropy);
                                        if (tempAllEntropy < 1)
                                        {
                                            calculateEachEntropy(tempPList, tempRowsList, tempUniqueList);
                                            calculateIGain(tempPList, tempRowsList, tempUniqueList, ref tempAllEntropy);
                                            descIGain(tempPList, ref tempDescArray, ref tempAllEntropy);
                                        }


                                        if (checkIfEnded(ref numberOfPureSets, j, ref pnn2.aList, tempPList, tempRowsList, tempUniqueList))     // <----- send the new dataset
                                        {
                                            i = j;
                                            break;
                                        }

                                        if(numberOfPureSets == uniqueList[ii].Count - 1)
                                        {
                                            finalBreak = 1;
                                            break;
                                        }


                                        pnn2.name = tempName;
                                        tempUniqueList = new List<List<UniqueRows>>();
                                        tempAllEntropy = 0;
                                        if (tempDescArray != null)
                                            Array.Clear(tempDescArray, 0, tempDescArray.Length);

                                        j++;

                                       if(j > descArray.Length - 1) break;             // Finally. It was a bug :D

                                    }
                                    tempRowsList.Clear();
                                    tempUniqueList.Clear();
                                    tempPList.Clear();
                                }


                            }
                            pnn1.bList.Add(pnn2);
                            if (finalBreak == 1) break;
                            
                        }
                        finalList.Add(pnn1);

                        break;
                    }
                    if (finalBreak == 1) break;
                }
                if (finalBreak == 1) break;
            }

        }

        bool checkIfEnded(ref int numberOfPureSets, int j, ref List<AttributesTree> selfAtt, List<Attributes> pList, List<List<String>> rowsList, List<List<UniqueRows>> uniqueList)
        {
            for (int ii = 0; ii < pList.Count; ii++)
            {
                if (ii == descArray[j])             // <======== descArray here might bug in a large use, so it may need to be replaced with tempDescArray
                {
                    AttributesTree pnn1 = new AttributesTree();
                    pnn1.name = pList[ii].name;
                    //Console.WriteLine("HEAD: " + pnn1.name);//
                    pnn1.bList = new List<Branches>();
                    for (int iii = 0; iii < 2; iii++)
                    {
                        Branches pnn2 = new Branches();
                        if (uniqueList[ii][iii].entropy == 0)
                        {
                            // pureset
                            pnn2.name = uniqueList[ii][iii].name;
                            //Console.WriteLine(pnn2.name);//
                            int b = 0;
                            for (int iv = 0; iv < rowsList.Count; iv++)
                            {
                                for (int v = 0; v < rowsList[iv].Count; v++)
                                {
                                    if (rowsList[iv][v] == pnn2.name && v == ii)            // <====== && v == ii Fixed a bug :D
                                    {
                                        pnn2.end = rowsList[iv][rowsList[iv].Count - 1];
                                        b = 1;
                                        //Console.WriteLine(pnn2.end);//
                                        Console.WriteLine();
                                        break;
                                    }
                                }
                                if (b == 1) break;
                            }

                            /////
                        }
                        else
                        {
                            ///////
                            tempName = pnn2.name;
                            return false;
                        }
                        pnn1.bList.Add(pnn2);
                    }
                    selfAtt.Add(pnn1);
                    break;
                }
            }

            numberOfPureSets++;
            return true;
        }

        void createNewDataset(String name, List<Attributes> pList)
        {
            for(int i = 0; i < tempRowsList.Count; i++)
            {
                int f = 0;
                for(int ii = 0; ii < tempRowsList[i].Count; ii++)
                {
                    if(tempRowsList[i][ii] == name)
                    {
                        f = 1;
                    }
                }
                if (f == 0)
                {
                    tempRowsList.RemoveAt(i);
                    i--;            // Finally. It was a bug :D
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            //Console.WriteLine("New Dataset: ");

            for (int i = 0; i < tempRowsList.Count; i++)
            {
                for(int ii = 0; ii < tempRowsList[i].Count; ii++)
                {
                    //Console.WriteLine(tempRowsList[i][ii]);
                }
                Console.WriteLine();
            }
        }

        public void displayStageOneInConsole()
        {
                for(int ii = 0; ii < uniqueList.Count; ii++)
                {
                    Console.WriteLine();
                    Console.WriteLine(pList[ii].name + " has " + uniqueList[ii].Count + " Arrows: ");
                    for(int iii = 0; iii < uniqueList[ii].Count; iii++)
                    {
                        Console.WriteLine(uniqueList[ii][iii].name);
                    }
                }
        }

        void drawScene(Graphics g)
        {
            g.Clear(Color.White);

            for(int i = 0; i < lineList.Count; i++)
            {
                g.FillRectangle(Brushes.Black, lineList[i].X, lineList[i].Y, lineList[i].W, lineList[i].H);
            }
        }

        void drawDouble(Graphics g)
        {
            Graphics g2 = Graphics.FromImage(off);
            drawScene(g2);
            g.DrawImage(off, 0, 0);
        }
    }
}

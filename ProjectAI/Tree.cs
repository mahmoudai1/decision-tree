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
    public partial class Tree : Form
    {
        Bitmap off;


        class DrawTree
        {
            public int X, Y;
            public int X1, Y1, X2, Y2;
            public String name;
            public String type;
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

        List<DrawTree> treeList = new List<DrawTree>();

        int vX = 0;
        int vY = 50;
        int ct = 0;
        int vX2 = 300;

        public Tree()
        {
            InitializeComponent();
            this.Paint += Form1_Paint;
        }

        private void Tree_Load(object sender, EventArgs e)
        {
            off = new Bitmap(this.Width, this.Height);
            vX = this.ClientSize.Width / 2;
            createTheToolsForDrawing(Form1.finalList, 0, vX2);
            
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            drawDouble(e.Graphics);
        }

        void createTheToolsForDrawing(List<Form1.AttributesTree> finalList, int prev, int vX2)
        {
            
            for (int i = 0; i < finalList.Count; i++)
            {
                if (finalList[i].bList.Count > 0 && (finalList[i].bList[0].aList.Count > 0 || finalList[i].bList[0].end != null))
                {
                    DrawTree pnn1 = new DrawTree();
                    if (treeList.Count == 0)
                    {
                        pnn1.name = finalList[i].name;
                        pnn1.type = "head";
                        pnn1.X = vX;
                        pnn1.Y = vY;
                        treeList.Add(pnn1);
                    }
                    else
                    {
                        Font font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        Bitmap b = new Bitmap(2200, 2200);
                        Graphics g = Graphics.FromImage(b);
                        SizeF sizeOfString = new SizeF();

                        pnn1.name = finalList[i].name;
                        sizeOfString = g.MeasureString(pnn1.name, font);
                        pnn1.type = "head";
                        pnn1.X = treeList[prev].X2 - (Convert.ToInt32(sizeOfString.Width) / 2);
                        pnn1.Y = treeList[prev].Y2 + 15;
                        treeList.Add(pnn1);
                    }
                    //vX2 = 500;
                    for(int j = 0; j < finalList[i].bList.Count; j++)
                    {
                        DrawTree pnn2 = new DrawTree();
                        pnn2.name = finalList[i].bList[j].name;
                        pnn2.type = "arrow";
                        pnn2.X1 = pnn1.X + 65;
                        pnn2.Y1 = pnn1.Y + 50;
                        pnn2.Y2 = pnn1.Y + 300;
                        pnn2.X2 = pnn1.X - (finalList[i].bList.Count - (j * 4) * 120) - 300;        // EDITED MUCH TIMES

                        Font font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        Bitmap b = new Bitmap(2200, 2200);
                        Graphics g = Graphics.FromImage(b);
                        SizeF sizeOfString = new SizeF();
                        sizeOfString = g.MeasureString(pnn2.name, font);

                        pnn2.X = ((pnn2.X1 + pnn2.X2) / 2) - (Convert.ToInt32(sizeOfString.Width) / 2);
                        pnn2.Y = ((pnn2.Y1 + pnn2.Y2) / 2);
                        vX2 -= 300;
                        treeList.Add(pnn2);

                        if (finalList[i].bList[j].aList.Count > 0)
                        {
                           vX2 = 300;
                            createTheToolsForDrawing(finalList[i].bList[j].aList, treeList.Count - 1, vX2);
                        }
                        else
                        {
                            // put the if for calssification here
                            

                            DrawTree pnn3 = new DrawTree();
                            pnn3.name = finalList[i].bList[j].end;
                            sizeOfString = g.MeasureString(pnn3.name, font);
                            pnn3.type = "end";
                            pnn3.X = pnn2.X2 - (Convert.ToInt32(sizeOfString.Width) / 2);
                            pnn3.Y = pnn2.Y2 + 15;
                            treeList.Add(pnn3);
                        }
                    }
                }
            }
        }

        void drawScene(Graphics g)
        {
            g.Clear(Color.White);

            for(int i = 0; i < treeList.Count; i++)
            {
                if(treeList[i].type == "head")
                {
                    g.DrawString(treeList[i].name + "", new Font("Arial", 16, FontStyle.Bold), Brushes.Black, treeList[i].X, treeList[i].Y);
                }
                else if(treeList[i].type == "arrow")
                {
                    Pen p = new Pen(Color.Black, 3);
                    
                    g.DrawLine(p, treeList[i].X1, treeList[i].Y1, treeList[i].X2, treeList[i].Y2);
                    g.FillRectangle(Brushes.White, treeList[i].X, treeList[i].Y, 150, 50);
                    g.DrawString(treeList[i].name + "", new Font("Arial", 14, FontStyle.Bold), Brushes.DodgerBlue, treeList[i].X, treeList[i].Y);
                    
                }
                else if(treeList[i].type == "end")
                {
                    g.DrawString(treeList[i].name + "", new Font("Arial", 16, FontStyle.Bold), Brushes.LimeGreen, treeList[i].X, treeList[i].Y);
                }
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

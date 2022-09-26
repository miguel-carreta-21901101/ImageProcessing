using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CG_OpenCV
{
    public partial class HistGray : Form
    {
        public HistGray(int[] histArray)
        {
            InitializeComponent();
            ZedGraph.GraphPane myPane = zedGraphControl1.GraphPane;
            myPane.Title.Text = "Histograma de Cinzentos";
            ZedGraph.PointPairList list1 = new ZedGraph.PointPairList();

            for (int x = 0; x < 256; x++)
            {
                list1.Add(x, histArray[x]);
            }

            myPane.AddCurve("Cinzentos", list1, Color.Black, ZedGraph.SymbolType.Diamond);
            zedGraphControl1.AxisChange();

        }

        private void HistGray_Load(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSO2AddAbility
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Weapon w = new Weapon();
            w.abilities = new IAbility[] { パワー.GetLv(3), ヴォル・ソール.Get(), アビリティ.GetLv(3), アーム.GetLv(3)};
            Synthesis.Synthesize(w);
        }

        class aaa
        {
            public aaa()
            {

            }
        }
    }
}

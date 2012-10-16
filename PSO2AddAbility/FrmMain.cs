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
            w.abilities = new IAbility[] { new パワー(3), new ヴォル・ソール(), new アビリティ(3)};
            Synthesis.Synthesize(w);
        }
    }
}

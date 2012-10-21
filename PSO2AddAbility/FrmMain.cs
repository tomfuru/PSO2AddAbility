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
            //Weapon w = new Weapon();
            //w.abilities = new IAbility[] { パワー.GetLv(3), ヴォル・ソール.Get(), アビリティ.GetLv(3), アーム.GetLv(3) };
            //Synthesis.Synthesize(w);

            Weapon w1 = new Weapon();
            w1.abilities = new IAbility[] { パワー.GetLv(1), アーム.GetLv(1) };
            var result1 = Synthesis.Synthesize(w1, false);

            displayWeaponSynthesis(w1, result1);
        }

        private void displayWeaponSynthesis(Weapon weapon, SynthesisWeapons[] synthesisweapons)
        {
            TreeNode node = new TreeNode(weapon.ToString());
            var nodes = synthesisweapons.Select((sw, i) => {
                var tn = new TreeNode(string.Format("case {0}", i + 1));
                tn.Nodes.AddRange(new TreeNode[] {
                    displayWeaponSynthesisRecursive(sw.info0),
                    displayWeaponSynthesisRecursive(sw.info1),
                    displayWeaponSynthesisRecursive(sw.info2)
                });
                return tn;
            });
            node.Nodes.AddRange(nodes.ToArray());
            treeViewResult.Nodes.Add(node);
        }

        private TreeNode displayWeaponSynthesisRecursive(WeaponSynthesisInfo info)
        {
            TreeNode node = new TreeNode(info.Weapon.ToString());
            if (info.SynthesisInfo != null) {
                var nodes = info.SynthesisInfo.Select((sw, i) =>
                {
                    var tn = new TreeNode(string.Format("case {0}", i + 1));
                    tn.Nodes.AddRange(new TreeNode[] {
                    displayWeaponSynthesisRecursive(sw.info0),
                    displayWeaponSynthesisRecursive(sw.info1),
                    displayWeaponSynthesisRecursive(sw.info2)
                });
                    return tn;
                });
                node.Nodes.AddRange(nodes.ToArray());
            }
            return node;
        }

    }
}

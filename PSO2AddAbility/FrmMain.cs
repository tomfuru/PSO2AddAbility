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
            //Weapon w = new Weapon(パワー.GetLv(3), ヴォル・ソール.Get(), アビリティ.GetLv(3), アーム.GetLv(3));
            //Weapon w = new Weapon(パワー.GetLv(3), アーム.GetLv(3));
            Weapon w = new Weapon(パワー.GetLv(3), ファング・ソール.Get());
            var result = Synthesis.Synthesize(w, false);
            displayWeaponSynthesisDynamically_first(w, result);
        }

        //-------------------------------------------------------------------------------
        #region -(Class)NodeInfo
        //-------------------------------------------------------------------------------
        private class NodeInfo
        {
            public bool isAddedNode = false;
            public SynthesisWeapons[] synthesisWeapons;
        }
        //-------------------------------------------------------------------------------
        #endregion (-(Class)NodeInfo)

        //-------------------------------------------------------------------------------
        #region displayWeaponSynthesisDynamically_first 最上階部分結果データ表示
        //-------------------------------------------------------------------------------
        //
        private void displayWeaponSynthesisDynamically_first(Weapon weapon, SynthesisWeapons[] synthesisweapons)
        {
            TreeNode node = new TreeNode(weapon.ToString());
            displayWeaponSynthesisDynamically(node, synthesisweapons);
            treeViewResult.Nodes.Add(node);
        }
        #endregion (displayWeaponSynthesisDynamically_first)
        //-------------------------------------------------------------------------------
        #region displayWeaponSynthesisDynamically Expand時にデータを追加するようなノードを追加
        //-------------------------------------------------------------------------------
        //
        private void displayWeaponSynthesisDynamically(TreeNode parent_node, SynthesisWeapons[] synthesisweapons)
        {
            var nodes = synthesisweapons.Select((sw, i) =>
            {
                var tn = new TreeNode(string.Format("case {0}", i + 1));
                TreeNode node0 = new TreeNode(sw.info0.Weapon.ToString());
                node0.Tag = new NodeInfo() { synthesisWeapons = sw.info0.SynthesisInfo };
                if (sw.info0.SynthesisInfo != null) { node0.Nodes.Add(""); } // Expandできるようにダミーノード追加
                TreeNode node1 = new TreeNode(sw.info1.Weapon.ToString());
                node1.Tag = new NodeInfo() { synthesisWeapons = sw.info1.SynthesisInfo };
                if (sw.info1.SynthesisInfo != null) { node1.Nodes.Add(""); } // Expandできるようにダミーノード追加
                TreeNode node2 = new TreeNode(sw.info2.Weapon.ToString());
                node2.Tag = new NodeInfo() { synthesisWeapons = sw.info2.SynthesisInfo };
                if (sw.info2.SynthesisInfo != null) { node2.Nodes.Add(""); } // Expandできるようにダミーノード追加

                tn.Nodes.AddRange(new TreeNode[] { node0, node1, node2 });
                return tn;
            });
            parent_node.Nodes.Clear();
            parent_node.Nodes.AddRange(nodes.ToArray());
        }
        #endregion (displayWeaponSynthesisDynamically)

        //-------------------------------------------------------------------------------
        #region treeViewResult_BeforeExpand Expand時に発生，項目を動的に追加
        //-------------------------------------------------------------------------------
        //
        private void treeViewResult_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            NodeInfo info = e.Node.Tag as NodeInfo;
            if (info == null) { return; }

            if (!info.isAddedNode) {
                displayWeaponSynthesisDynamically(e.Node, info.synthesisWeapons);

                info.isAddedNode = true;
            }
        }
        #endregion (treeViewResult_BeforeExpand)


        //-------------------------------------------------------------------------------
        #region displayWeaponSynthesis 全ノードを追加
        //-------------------------------------------------------------------------------
        //
        private void displayWeaponSynthesis(Weapon weapon, SynthesisWeapons[] synthesisweapons)
        {
            TreeNode node = new TreeNode(weapon.ToString());
            var nodes = synthesisweapons.Select((sw, i) =>
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
            treeViewResult.Nodes.Add(node);
        }
        #endregion (displayWeaponSynthesis)
        //-------------------------------------------------------------------------------
        #region displayWeaponSynthesisRecursive 全ノードを追加(再帰部分)
        //-------------------------------------------------------------------------------
        //
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
        #endregion (displayWeaponSynthesisRecursive)

    }
}

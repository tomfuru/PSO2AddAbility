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
        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        //
        public FrmMain()
        {
            InitializeComponent();
        }
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region btnSynthesis_Click 合成ボタン
        //-------------------------------------------------------------------------------
        //
        private void btnSynthesis_Click(object sender, EventArgs e)
        {
            Weapon w = new Weapon(waInputObjective.GetAbilities().ToArray());

            var result = Synthesis.Synthesize(w, false);
            displayWeaponSynthesisDynamically_first(w, result);
        }
        #endregion (btnSynthesis_Click)

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
            node.Expand();
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
                var tn = new TreeNode(string.Format("case {0} {1} {2}", i + 1, Util.ProbabilityToString(sw.probabilities.Aggregate(1.0f, (f1, f2) => f1 *  f2)) ,sw.probabilities.Select(Util.ProbabilityToString).ToArray().AllToString('[', ']')));
                List<TreeNode> nodeList = new List<TreeNode>();
                
                TreeNode node0 = new TreeNode(sw.info0.Weapon.ToString());
                node0.Tag = new NodeInfo() { synthesisWeapons = sw.info0.SynthesisInfo };
                if (sw.info0.SynthesisInfo != null) { node0.Nodes.Add(""); } // Expandできるようにダミーノード追加
                nodeList.Add(node0);

                TreeNode node1 = new TreeNode(sw.info1.Weapon.ToString());
                node1.Tag = new NodeInfo() { synthesisWeapons = sw.info1.SynthesisInfo };
                if (sw.info1.SynthesisInfo != null) { node1.Nodes.Add(""); } // Expandできるようにダミーノード追加
                nodeList.Add(node1);
                
                if (sw.info2 != null) {
                    TreeNode node2 = new TreeNode(sw.info2.Weapon.ToString());
                    node2.Tag = new NodeInfo() { synthesisWeapons = sw.info2.SynthesisInfo };
                    if (sw.info2.SynthesisInfo != null) { node2.Nodes.Add(""); } // Expandできるようにダミーノード追加
                    nodeList.Add(node2);
                }
                tn.Nodes.AddRange(nodeList.ToArray());
                tn.Expand();
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

        private void tsmi設定_Click(object sender, EventArgs e)
        {

        }


        //-------------------------------------------------------------------------------
        #region (commented out)
        /*
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
        */
        //-------------------------------------------------------------------------------
        #endregion ((commented out))

    }
}

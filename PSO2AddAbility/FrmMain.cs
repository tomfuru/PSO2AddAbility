using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSO2AddAbility
{
    public partial class FrmMain : Form
    {
        private SettingsData _settings;
        private const string SETTINGS_FILENAME = "PSO2AddAbilitySettings.dat";

        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        //
        public FrmMain()
        {
            InitializeComponent();

            tsslText.Text = tsslSynthesisNumber.Text = "";
            _settings = SettingsData.Restore(SETTINGS_FILENAME) ?? new SettingsData();

            numSynFee2.Value = _settings.Synthesis_2weapons;
            numSynFee3.Value = _settings.Synthesis_3weapons;
        }
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region FrmMain_FormClosed
        //-------------------------------------------------------------------------------
        //
        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            _settings.Synthesis_2weapons = (int)numSynFee2.Value;
            _settings.Synthesis_3weapons = (int)numSynFee3.Value;

            _settings.Save(SETTINGS_FILENAME);
        }
        #endregion (FrmMain_FormClosed)

        //-------------------------------------------------------------------------------
        #region btnSynthesis_Click 合成ボタン
        //-------------------------------------------------------------------------------
        //
        private async void btnSynthesis_Click(object sender, EventArgs e)
        {
            Weapon w = new Weapon(waInputObjective.GetAbilities().ToArray());

            CancellationTokenSource cts = new CancellationTokenSource();
            var sresult = Task.Factory.StartNew((() =>
            {
                Stopwatch sw = Stopwatch.StartNew();
                while (!cts.Token.IsCancellationRequested) {
                    this.Invoke((Action)(() => tsslText.Text = string.Format("{0}秒経過．．．", sw.ElapsedMilliseconds / 1000)));
                    Thread.Sleep(100);
                }
                return sw.ElapsedMilliseconds / 1000;
            }), cts.Token);

            int max_num = 0;
            int current_num = 0;
            Action<int> max_report = (max) =>
            {
                Interlocked.Exchange(ref max_num, max);
                this.Invoke((Action)(() => tsslSynthesisNumber.Text = string.Format("{0}/{1}", 0, max_num)));
            };
            Action finOne_report = () =>
            {
                Interlocked.Increment(ref current_num);
                this.Invoke((Action)(() => tsslSynthesisNumber.Text = string.Format("{0}/{1}", current_num, max_num)));
            };

            var synInfo = new Synthesis.SynthesisGeneralInfo(_settings, (int)numSynFee2.Value, (int)numSynFee3.Value);

            var result = await Task.Run(() => Synthesis.Synthesize(w, false, synInfo, max_report, finOne_report));

            cts.Cancel();
            tsslText.Text = string.Format("候補の列挙が完了しました！({0}秒かかりました)", sresult.Result);

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
            treeViewResult.Nodes.Clear();

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
            var nodes = synthesisweapons.OrderBy(wep => wep.cost)
                                        .Select((sw, i) =>
            {
                var tn = new TreeNode(string.Format("case{0} 【{1}】 {2} {3}", i + 1, Util.CostToString(sw.cost, "N0"), Util.ProbabilityToString(sw.probabilities.Aggregate(1.0f, (f1, f2) => f1 * f2)), sw.probabilities.Select(Util.ProbabilityToString).ToArray().AllToString('[', ']')));
                List<TreeNode> nodeList = new List<TreeNode>();

                TreeNode node0 = new TreeNode(string.Format("{0} 【{1}】", sw.info0.Weapon.ToString(), Util.CostToString(sw.info0.Cost, "N0")));
                node0.Tag = new NodeInfo() { synthesisWeapons = sw.info0.SynthesisInfo };
                if (sw.info0.SynthesisInfo != null) { node0.Nodes.Add(""); } // Expandできるようにダミーノード追加
                nodeList.Add(node0);

                TreeNode node1 = new TreeNode(string.Format("{0} 【{1}】", sw.info1.Weapon.ToString(), Util.CostToString(sw.info1.Cost, "N0")));
                node1.Tag = new NodeInfo() { synthesisWeapons = sw.info1.SynthesisInfo };
                if (sw.info1.SynthesisInfo != null) { node1.Nodes.Add(""); } // Expandできるようにダミーノード追加
                nodeList.Add(node1);

                if (sw.info2 != null) {
                    TreeNode node2 = new TreeNode(string.Format("{0} 【{1}】", sw.info2.Weapon.ToString(), Util.CostToString(sw.info2.Cost, "N0")));
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

        //-------------------------------------------------------------------------------
        #region tsmi設定_Click
        //-------------------------------------------------------------------------------
        //
        private void tsmi設定_Click(object sender, EventArgs e)
        {
            using (FrmConfig frm = new FrmConfig()) {
                frm.ShowDialog(this);
            }
        }
        #endregion (tsmi設定_Click)
        //-------------------------------------------------------------------------------
        #region tsmiConfigValue_Click
        //-------------------------------------------------------------------------------
        //
        private void tsmiConfigValue_Click(object sender, EventArgs e)
        {
            using (FrmConfigValue frm = new FrmConfigValue()) {
                frm.ValueData = _settings.ValueData;
                if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK) {
                    _settings.Save(SETTINGS_FILENAME);
                }
            }
        }
        #endregion (tsmiConfigValue_Click)

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

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSO2AddAbility
{
    public partial class FrmConfigValue : Form
    {
        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        public FrmConfigValue()
        {
            InitializeComponent();
        }
        //-------------------------------------------------------------------------------
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region FrmConfigValue_Load
        //-------------------------------------------------------------------------------
        //
        private void FrmConfigValue_Load(object sender, EventArgs e)
        {
            Debug.Assert(ValueData != null);
            DisplayAllItems();
        }
        #endregion (FrmConfigValue_Load)

        //-------------------------------------------------------------------------------
        #region ValueData プロパティ：
        //-------------------------------------------------------------------------------
        private ValueData _valuedata;
        /// <summary>
        /// 
        /// </summary>
        public ValueData ValueData
        {
            get { return _valuedata; }
            set { _valuedata = value; }
        }
        #endregion (ValueData)

        //-------------------------------------------------------------------------------
        #region btnOK_Click
        //-------------------------------------------------------------------------------
        //
        private void btnOK_Click(object sender, EventArgs e)
        {
            ExtractAndSetAllItems();
            bool b = ValueData.ValueDataDic.ContainsKey(SerializableTuple.Create(AbilityType.アーム, 1));

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        #endregion (btnOK_Click)
        //-------------------------------------------------------------------------------
        #region btnCansel_Click
        //-------------------------------------------------------------------------------
        //
        private void btnCansel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        #endregion (btnCansel_Click)

        //-------------------------------------------------------------------------------
        #region -DisplayAllItems
        //-------------------------------------------------------------------------------
        //
        private void DisplayAllItems()
        {
            Action<IAbility, string, int[]> append_values = (ab, text, vals) =>
            {
                DataGridViewRow row = new DataGridViewRow();
                row.HeaderCell.Value = text;
                object[] vals__ = vals.Skip(1).Select(i => (object)i).ToArray();
                //row.SetValues(vals__);
                int index = dgwValues.Rows.Add(row);
                dgwValues.Rows[index].SetValues(vals__);
                dgwValues.Rows[index].Tag = ab;
            };
            Action<IAbility> append_default = ab => append_values(ab, ab.ToString(), new int[] { 0, 1050, 1050, 1050, 1050, 1050, 1050, 1050, 1050 });

            // ゴミだけ
            append_values(ゴミ.Get(), "", _valuedata.GarbageValues);

            // 特定アビリティ1つ付き
            var dic = _valuedata.ValueDataDic;
            foreach (var ab in Data.ALL_ABILITIES) {
                AbilityType type = Data.DIC_IABILITY_TO_ABILITYTYPE[ab];
                if (ab is ILevel) {
                    ILevel ab_lv = ab as ILevel;
                    int max_level = ab_lv.AllLevels().Count();
                    for (int i = 1; i <= max_level; i++) {
                        if (dic.ContainsKey(SerializableTuple.Create(type, i))) {
                            append_values(ab_lv.GetInstanceOfLv(i), ab_lv.GetInstanceOfLv(i).ToString(), dic[SerializableTuple.Create(type, i)]);
                        }
                        else { append_default(ab_lv.GetInstanceOfLv(i)); }
                    }
                }
                else {
                    if (dic.ContainsKey(SerializableTuple.Create(type, 0))) {
                        append_values(ab, ab.ToString(), dic[SerializableTuple.Create(type, 0)]);
                    }
                    else { append_default(ab); }
                }
            }

        }
        #endregion (DisplayAllItems)
        //-------------------------------------------------------------------------------
        #region -ExtractAndSetAllItems
        //-------------------------------------------------------------------------------
        //
        private void ExtractAndSetAllItems()
        {
            foreach (DataGridViewRow row in dgwValues.Rows) {
                int[] values = new int[9];
                int[] input_values = row.Cells.Cast<DataGridViewCell>()
                                              .Select(dgc => (dgc.Value is int) ? (int)dgc.Value : (dgc.Value is string) ? int.Parse((string)dgc.Value) : 0)
                                              .ToArray();
                Array.Copy(input_values, 0, values, 1, 8);

                if (row.Tag is ゴミ) {
                    _valuedata.GarbageValues = values;
                }
                else {
                    IAbility ab = row.Tag as IAbility;
                    Debug.Assert(ab != null);

                    Action<AbilityType, int, int[]> updateDictionary = (a_ab, a_level, a_values) =>
                    {
                        var tuple = SerializableTuple.Create(a_ab, a_level);
                        if (_valuedata.ValueDataDic.ContainsKey(tuple)) {
                            _valuedata.ValueDataDic[tuple] = a_values;
                        }
                        else {
                            _valuedata.ValueDataDic.Add(tuple, a_values);
                        }
                    };

                    if (ab is ILevel) {
                        ILevel ab_lv = ab as ILevel;
                        updateDictionary(Data.DIC_IABILITY_TO_ABILITYTYPE[ab_lv.GetInstanceOfLv(1)], ab_lv.Level, values);
                    }
                    else {
                        updateDictionary(Data.DIC_IABILITY_TO_ABILITYTYPE[ab], 0, values);
                    }
                }
            }
        }
        #endregion (ExtractAndSetAllItems)

        //-------------------------------------------------------------------------------
        #region dgwValues_EditingControlShowing
        //-------------------------------------------------------------------------------
        //
        private void dgwValues_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tb = e.Control as TextBox;
            tb.KeyPress += DGVTextbox_Keypress;
        }
        #endregion (dgwValues_EditingControlShowing)
        //-------------------------------------------------------------------------------
        #region DGVTextbox_Keypress
        //-------------------------------------------------------------------------------
        //
        private void DGVTextbox_Keypress(object sender, KeyPressEventArgs e)
        {
            if (!(Char.IsDigit(e.KeyChar) || Char.IsControl(e.KeyChar))) {
                e.Handled = true;
            }
        }
        #endregion (DGVTextbox_Keypress)
    }

    //-------------------------------------------------------------------------------
    #region (Class)ValueData
    //-------------------------------------------------------------------------------
    [Serializable]
    public class ValueData
    {
        /// <summary>
        /// <para>一つだけアビリティを持つ武器の価値</para>
        /// <para>[アビリティクラス, レベル] -> 金額(dummy, 1スロ, 2スロ, ... , 8スロ)</para>
        /// <para>レベルがない場合(ILevel継承していない)はレベル=0</para>
        /// </summary>
        public SerializableDictionary<SerializableTuple<AbilityType, int>, int[]> ValueDataDic = new SerializableDictionary<SerializableTuple<AbilityType, int>, int[]>();
        /// <summary>
        /// <para>ゴミだけのアビリティを持つ武器の価値</para>
        /// </summary>
        public int[] GarbageValues = new int[] { 0, 1050, 1050, 1050, 1050, 1050, 1050, 1050, 1050 };
    }
    //-------------------------------------------------------------------------------
    #endregion (ValueData)
}

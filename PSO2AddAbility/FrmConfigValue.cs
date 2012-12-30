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
        #region DisplayAllItems
        //-------------------------------------------------------------------------------
        //
        private void DisplayAllItems()
        {
            Action<IAbility, int[]> append_values = (ab, vals) =>
            {
                DataGridViewRow row = new DataGridViewRow();
                row.HeaderCell.Value = ab.ToString();
                object[] vals__ = vals.Skip(1).Select(i => (object)i).ToArray();
                //row.SetValues(vals__);
                int index = dataGridView1.Rows.Add(row);
                dataGridView1.Rows[index].SetValues(vals__);
                dataGridView1.Rows[index].Tag = ab;
            };
            Action<IAbility> append_default = ab => append_values(ab, new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 });

            var dic = _valuedata.ValueDataDic;
            foreach (var ab in Data.ALL_ABILITIES) {
                AbilityType type = Data.DIC_IABILITY_TO_ABILITYTYPE[ab];
                if (ab is ILevel) {
                    ILevel ab_lv = ab as ILevel;
                    int max_level = ab_lv.AllLevels().Count();
                    for (int i = 1; i <= max_level; i++) {
                        if (dic.ContainsKey(SerializableTuple.Create(type, i))) {
                            append_values(ab_lv.GetInstanceOfLv(i), dic[SerializableTuple.Create(type, i)]);
                        }
                        else { append_default(ab_lv.GetInstanceOfLv(i)); }
                    }
                }
                else {
                    if (dic.ContainsKey(SerializableTuple.Create(type, 0))) {
                        append_values(ab, dic[SerializableTuple.Create(type, 0)]);
                    }
                    else { append_default(ab); }
                }
            }

        }
        #endregion (DisplayAllItems)

        //-------------------------------------------------------------------------------
        #region ExtractAndSetAllItems
        //-------------------------------------------------------------------------------
        //
        private void ExtractAndSetAllItems()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows) {
                int[] values = new int[9];
                int[] input_values = row.Cells.Cast<DataGridViewCell>()
                                              .Select(dgc => (dgc.Value is int) ? (int)dgc.Value : (dgc.Value is string) ? int.Parse((string)dgc.Value) : 0)
                                              .ToArray();
                Array.Copy(input_values, 0, values, 1, 8);

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
        #endregion (ExtractAndSetAllItems)
    }

    //-------------------------------------------------------------------------------
    #region (Class)ValueData
    //-------------------------------------------------------------------------------
    [Serializable]
    public class ValueData
    {
        /// <summary>
        /// <para>[アビリティクラス, レベル] -> 金額(dummy, 1スロ, 2スロ, ... , 8スロ)</para>
        /// <para>レベルがない場合(ILevel継承していない)はレベル=0</para>
        /// </summary>
        public SerializableDictionary<SerializableTuple<AbilityType, int>, int[]> ValueDataDic = new SerializableDictionary<SerializableTuple<AbilityType, int>, int[]>();
    }
    //-------------------------------------------------------------------------------
    #endregion (ValueData)
}

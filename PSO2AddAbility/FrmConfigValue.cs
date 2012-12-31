using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        #region Constatnts
        //-------------------------------------------------------------------------------
        private const string CSV_Filter = "CSVファイル(*.csv)|*.csv";
        private const char COMMA_C = ',';
        private const string COMMA = ",";
        //-------------------------------------------------------------------------------
        #endregion (Constatnts)

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
        #region btnReadFile_Click
        //-------------------------------------------------------------------------------
        //
        private void btnReadFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog()) {
                ofd.Filter = CSV_Filter;
                ofd.InitialDirectory = Environment.CurrentDirectory;
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    try {
                        var values = ReadFromFile(ofd.FileName);
                        // データ更新
                        if (values != null) {
                            dgvValues.Rows
                                     .Cast<DataGridViewRow>()
                                     .ToList()
                                     .ForEach(row =>
                                     {
                                         IAbility ability = row.Tag as IAbility;
                                         if (values.ContainsKey(ability)) {
                                             row.SetValues(values[ability].Select(i => (object)i).ToArray());
                                         }
                                     });
                        }

                        MessageBox.Show("読込完了しました。", Application.ProductName);
                    }
                    catch (Exception) {
                        MessageBox.Show("読込に失敗しました。。", Application.ProductName);
                    }
                }
            }
        }
        #endregion (btnReadFile_Click)
        //-------------------------------------------------------------------------------
        #region btnWriteFile_Click
        //-------------------------------------------------------------------------------
        //
        private void btnWriteFile_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog()) {
                sfd.Filter = CSV_Filter;
                sfd.InitialDirectory = Environment.CurrentDirectory;
                if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                    try {
                        Dictionary<IAbility, int[]> valueDic = new Dictionary<IAbility, int[]>();

                        dgvValues.Rows.OfType<DataGridViewRow>()
                                      .Select(row => new KeyValuePair<IAbility, int[]>(row.Tag as IAbility, GetRowIntValues(row)))
                                      .ToList()
                                      .ForEach(kvp => valueDic.Add(kvp.Key, kvp.Value));

                        WriteToFile(sfd.FileName, valueDic);

                        MessageBox.Show("保存完了しました。", Application.ProductName);
                    }
                    catch (Exception) {
                        MessageBox.Show("保存に失敗しました。", Application.ProductName);
                    }
                }
            }
        }
        #endregion (btnWriteFile_Click)

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
                int index = dgvValues.Rows.Add(row);
                dgvValues.Rows[index].SetValues(vals__);
                dgvValues.Rows[index].Tag = ab;
            };
            Action<IAbility> append_default = ab => append_values(ab, ab.ToString(), new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0});

            // ゴミだけ
            append_values(ゴミ.Get(), "", _valuedata.GarbageValues);

            // 特定アビリティ1つ付き
            var dic = _valuedata.ValueDataDic;
            foreach (var ab in Data.GetAllAbilitiesIncludedLevel()) {
                AbilityType type = Data.DIC_IABILITY_TO_ABILITYTYPE[ab];
                int level = (ab is ILevel) ? (ab as ILevel).Level : 0;

                if (dic.ContainsKey(SerializableTuple.Create(type, level))) {
                    append_values(ab, ab.ToString(), dic[SerializableTuple.Create(type, level)]);
                }
                else { append_default(ab); }
            }
        }
        #endregion (DisplayAllItems)
        //-------------------------------------------------------------------------------
        #region -ExtractAndSetAllItems
        //-------------------------------------------------------------------------------
        //
        private void ExtractAndSetAllItems()
        {
            foreach (DataGridViewRow row in dgvValues.Rows) {
                int[] values = new int[9];
                int[] input_values = GetRowIntValues(row);
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

        //-------------------------------------------------------------------------------
        #region -ReadFromFile ファイルからデータを読み込んで設定
        //-------------------------------------------------------------------------------
        //
        private Dictionary<IAbility, int[]> ReadFromFile(string filePath)
        {
            using (StreamReader sr = new StreamReader(filePath)) {
                sr.ReadLine();

                var dic = new Dictionary<IAbility, int[]>();
                while (!sr.EndOfStream) {
                    string[] values_str = sr.ReadLine().Split(COMMA_C);
                    IAbility ability = Util.StrToIAbility(values_str[0]);
                    int[] values = values_str.Skip(1).Select(val => int.Parse(val)).ToArray();

                    dic.Add(ability, values);
                }

                return dic;
            }
        }
        #endregion (ReadFromFile)
        //-------------------------------------------------------------------------------
        #region -WriteToFile ファイルにデータを書き込む
        //-------------------------------------------------------------------------------
        //
        private void WriteToFile(string filePath, Dictionary<IAbility, int[]> valueDic)
        {
            using (StreamWriter sw = new StreamWriter(filePath)) {
                sw.WriteLine(Enumerable.Range(1, 8).Select(i => string.Format(",Slot{0}", i)).Aggregate("", (s1, s2) => s1 + s2));

                Data.GetAllAbilitiesIncludedLevel()
                    .ToList()
                    .ForEach(iab =>
                    {
                        sw.Write(iab.ToString());
                        sw.Write(COMMA_C);
                        sw.Write(string.Join(COMMA, valueDic[iab].Select(val => val.ToString())));
                        sw.WriteLine();
                    });
            }
        }
        #endregion (WriteToFile)

        //-------------------------------------------------------------------------------
        #region -GetRowIntValues 整数値としてデータを取得
        //-------------------------------------------------------------------------------
        //
        private int[] GetRowIntValues(DataGridViewRow row)
        {
            return row.Cells.Cast<DataGridViewCell>()
                            .Select(dgc => (dgc.Value is int) ? (int)dgc.Value : (dgc.Value is string) ? int.Parse((string)dgc.Value) : 0)
                            .ToArray();
        }
        #endregion (GetRowIntValues)
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
        public int[] GarbageValues = new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0};
    }
    //-------------------------------------------------------------------------------
    #endregion (ValueData)
}

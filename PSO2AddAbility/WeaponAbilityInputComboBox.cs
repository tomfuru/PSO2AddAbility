using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PSO2AddAbility
{
    public partial class WeaponAbilityInputComboBox : UserControl
    {
        private readonly ComboBox[] ABILITY_COMBOBOXES;

        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        //
        public WeaponAbilityInputComboBox()
        {
            InitializeComponent();

            ABILITY_COMBOBOXES = new ComboBox[] {
                cboAbbility1, cboAbbility2, cboAbbility3, cboAbbility4,
                cboAbbility5, cboAbbility6, cboAbbility7, cboAbbility8
            };
            // アビリティ一覧リスト作成
            List<IAbility> ablist = new List<IAbility>();
            foreach (var ability in Data.ALL_ABILITIES) {
                if (ability is ILevel) {
                    ILevel ab_lv = ability as ILevel;
                    foreach (var level in ab_lv.AllLevels()) {
                        ablist.Add(ab_lv.GetInstanceOfLv(level));
                    }
                }
                else { ablist.Add(ability); }
            }

            // 全コンボボックスに追加
            foreach (var combobox in ABILITY_COMBOBOXES) {
                combobox.Items.Add("無し");
                combobox.Items.AddRange(ablist.ToArray());
                combobox.SelectedIndex = 0;
            }
        }
        #endregion (Constructor)

        //-------------------------------------------------------------------------------
        #region IncludeGarbage プロパティ：
        //-------------------------------------------------------------------------------
        private bool _includeGarbage = false;
        /// <summary>
        /// 
        /// </summary>
        public bool IncludeGarbage
        {
            get { return _includeGarbage; }
            set {
                if (_includeGarbage && !value) {
                    foreach (var cbo in ABILITY_COMBOBOXES) { cbo.Items.RemoveAt(1); }
                }
                else if (!_includeGarbage && value) {
                    foreach (var cbo in ABILITY_COMBOBOXES) { cbo.Items.Insert(1, ゴミ.Get()); }
                }
                _includeGarbage = value;
            }
        }
        #endregion (IncludeGarbage)

        //-------------------------------------------------------------------------------
        #region Label プロパティ
        //-------------------------------------------------------------------------------
        /// <summary>
        /// 
        /// </summary>
        public string Label
        {
            get { return lblText.Text; }
            set { lblText.Text = value; }
        }
        #endregion (Label)

        //-------------------------------------------------------------------------------
        #region +GetAbilities
        //-------------------------------------------------------------------------------
        //
        public IEnumerable<IAbility> GetAbilities()
        {
            return ABILITY_COMBOBOXES.Where(cmb => cmb.SelectedIndex > 0).Select(cmb => cmb.SelectedItem as IAbility);
        }
        #endregion (GetAbilities)
        //-------------------------------------------------------------------------------
        #region +SetAbilities
        //-------------------------------------------------------------------------------
        //
        public void SetAbilities(IAbility[] abilities)
        {
            int index = 0;
            foreach (var cmbAbility in ABILITY_COMBOBOXES) {
                cmbAbility.SelectedItem = abilities[index];
                ++index;
            }
        }
        #endregion (SetAbilities)
    }
}

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
    public partial class WeaponAbilityInputCheckBox : UserControl
    {
        //-------------------------------------------------------------------------------
        #region Constructor
        //-------------------------------------------------------------------------------
        public WeaponAbilityInputCheckBox()
        {
            InitializeComponent();

            int count = 0;

            int defHeight = pnlDisp.Height;

            Action<IAbility> addCombobox = ab =>
            {
                int x = (count % 2 == 0) ? POINT_X_LEFT : POINT_X_RIGHT;
                int y = POINT_Y_FIRST + MARGIN_Y * (count / 2);

                var chb = new CheckBox() {
                    Location = new Point(x, y),
                    Text = ab.ToString(),
                    Tag = ab,
                    AutoSize = true
                };

                pnlDisp.Controls.Add(chb);
                _checkboxes.Add(chb);
            };

            foreach (var ability in Data.ALL_ABILITIES) {
                if (ability is ILevel) {
                    ILevel ab_lv = ability as ILevel;
                    foreach (var level in ab_lv.AllLevels()) {
                        addCombobox(ab_lv.GetInstanceOfLv(level));
                        count++;
                    }
                    if (count % 2 != 0) { count++; }
                }
                else {
                    addCombobox(ability);
                    count++;
                }
            }

            vscrPanel.Maximum = pnlDisp.Height - defHeight;
            vscrPanel.LargeChange = defHeight - 11;
            vscrPanel.SmallChange = 11;
        }
        //-------------------------------------------------------------------------------
        #endregion (Constructor)

        private List<CheckBox> _checkboxes = new List<CheckBox>();

        //-------------------------------------------------------------------------------
        #region Constant_Control
        //-------------------------------------------------------------------------------
        private const int POINT_Y_FIRST = 3;
        private const int POINT_X_LEFT = 12;
        private const int POINT_X_RIGHT = 129;
        private const int MARGIN_Y = 16;
        //-------------------------------------------------------------------------------
        #endregion (Constant_Control)

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
            return _checkboxes.Where(chb => chb.Checked)
                              .Select(chb => chb.Tag as IAbility);
        }
        #endregion (GetAbilities)
        //-------------------------------------------------------------------------------
        #region +SetAbilities
        //-------------------------------------------------------------------------------
        //
        public void SetAbilities(IAbility[] abilities)
        {
            foreach (var chb in _checkboxes) {
                var ab = chb.Tag as IAbility; 
                chb.Checked = abilities.Contains(ab);
            }
        }
        #endregion (SetAbilities)

        //-------------------------------------------------------------------------------
        #region WeaponAbilityInputCheckBox_Load
        //-------------------------------------------------------------------------------
        //
        private void WeaponAbilityInputCheckBox_Load(object sender, EventArgs e)
        {
            if (this.TopLevelControl != null) {
                this.TopLevelControl.MouseWheel += (obj, ee) =>
                {
                    int change = -ee.Delta / 120 * MARGIN_Y * 2;
                    vscrPanel.ScrollDelta(change);
                };
            }
        }
        #endregion (WeaponAbilityInputCheckBox_Load)

        //-------------------------------------------------------------------------------
        #region vscrPanel_Scroll
        //-------------------------------------------------------------------------------
        //
        private void vscrPanel_Scroll(object sender, ScrollEventArgs e)
        {
            vscrPanel.Value = e.NewValue;
        }
        #endregion (vscrPanel_Scroll)
        //-------------------------------------------------------------------------------
        #region vscrPanel_ValueChanged
        //-------------------------------------------------------------------------------
        //
        private void vscrPanel_ValueChanged(object sender, EventArgs e)
        {
            pnlDisp.Top = -vscrPanel.Value;
        }
        #endregion (vscrPanel_ValueChanged)

    }
}

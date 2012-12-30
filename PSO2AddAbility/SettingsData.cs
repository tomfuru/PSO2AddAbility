using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSO2AddAbility
{
    [Serializable]
    public class SettingsData : SaveDataClassBase<SettingsData>
    {
        public int Material_Synthesis_2weapons = 300;
        public int Material_Synthesis_3weapons = 400;
        public ValueData ValueData = new ValueData();
    }
}

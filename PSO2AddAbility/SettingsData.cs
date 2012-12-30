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
        public ValueData ValueData = new ValueData();
    }
}

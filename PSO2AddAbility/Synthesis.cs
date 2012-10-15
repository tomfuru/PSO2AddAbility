using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSO2AddAbility
{
    public static class Synthesis
    {
        //-------------------------------------------------------------------------------
        #region +Synthesize
        //-------------------------------------------------------------------------------
        //
        public static void Synthesize(Weapon objective)
        {
            int num_slot = objective.abilities.Length;
            var need_materials = objective.abilities.Select((IAbility ab) =>
            {
                List<IEnumerable<IAbility>> list = new List<IEnumerable<IAbility>>();
                if (ab is Inheritable) {
                    List<IAbility> l = new List<IAbility>();
                    l.Add(ab);
                    list.Add(l);
                }
                list.Add(Data.GetMaterialAbilities(ab));

                return list;
            }).ToArray();

            List<IEnumerable<IEnumerable<IAbility>>> combinations = new List<IEnumerable<IEnumerable<IAbility>>>();
            Action<int, IEnumerable<IEnumerable<IAbility>>> act = null;
            act = (i, values) => {
                foreach (var vals in need_materials[i])
                {
                    var concatted = values.Concat(new IEnumerable<IAbility>[] { vals });
                    if (need_materials.Length == i - 1) {
                        combinations.Add(concatted);
                    }
                    else {
                        act(i + 1, concatted);
                    }
                }
            };

            foreach (var combination in combinations) {
                var elements = combination.SelectMany(a => a); // 必要な特殊能力のリスト

                // (目的能力数)の合成で行う場合
                // TODO

                // (目的能力数)-1の合成で行う場合
                // TODO
            }
            
        }
        #endregion (Synthesize)

        //-------------------------------------------------------------------------------
        #region +GetProbability
        //-------------------------------------------------------------------------------
        //
        public static float GetProbability(IAbility objective, Weapon weapon1, Weapon weapon2, Weapon weapon3 = null)
        {

            throw new NotImplementedException();
        }
        #endregion (GetProbability)

        //-------------------------------------------------------------------------------
        #region +GetProbabilities
        //-------------------------------------------------------------------------------
        //
        public static float[] GetProbabilities(Weapon objective, Weapon weapon1, Weapon weapon2, Weapon weapon3 = null)
        {

            throw new NotImplementedException();
        }
        #endregion (GetProbabilities)
    }
}

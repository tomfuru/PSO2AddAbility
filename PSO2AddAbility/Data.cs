using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSO2AddAbility
{
    public static class Data
    {
        //-------------------------------------------------------------------------------
        #region +[static]GetMaterialAbilities
        //-------------------------------------------------------------------------------
        public static IEnumerable<IEnumerable<IAbility>> GetMaterialAbilities(IAbility ability)
        {
            if (ability is Basic_up || ability is Additional) {
                // (元の能力*1,2,3) or (一つ下の能力*2,3)
                yield return new[] { ability };
                yield return new[] { ability, ability };
                yield return new[] { ability, ability, ability };

                ILevel ability_l = ability as ILevel;
                int level = ability_l.Level;

                if (level > 1) {
                    //IAbility ab = Activator.CreateInstance(ability.GetType(), level - 1) as IAbility; 下に代替
                    IAbility ab = ability_l.GetInstanceOfLv(level - 1);
                    yield return new[] { ab, ab };
                    yield return new[] { ab, ab, ab };
                }
            }
            else if (ability is Ability) {
                // (元の能力*1,2,3) or (パワー,シュート,テクニック各1)
                yield return new[] { ability };
                yield return new[] { ability, ability };
                yield return new[] { ability, ability, ability };

                int level = ((ILevel)ability).Level;
                yield return new IAbility[] { パワー.GetLv(level), シュート.GetLv(level), テクニック.GetLv(level) };
            }
            else if (ability is Soul || ability is Special_up) {
                // (元の能力*2,3)
                yield return new[] { ability, ability };
                yield return new[] { ability, ability, ability };
            }

            yield break;
        }
        //-------------------------------------------------------------------------------
        #endregion (+[static]GetMaterialAbilities)

        public static readonly IAbility[] ALL_ABILITIES = {
            パワー.GetLv(1), シュート.GetLv(1), テクニック.GetLv(1), 
            ボディ.GetLv(1),  リアクト.GetLv(1), マインド.GetLv(1), 
            アーム.GetLv(1), スタミナ.GetLv(1), スピリタ.GetLv(1),

            バーン.GetLv(1), フリーズ.GetLv(1), ショック.GetLv(1), 
            ミラージュ.GetLv(1), パニック.GetLv(1), ポイズン.GetLv(1),
            ブロウレジスト.GetLv(1), ショットレジスト.GetLv(1), マインドレジスト.GetLv(1),
            フレイムレジスト.GetLv(1), アイスレジスト.GetLv(1), ショックレジスト.GetLv(1), 
            ウィンドレジスト.GetLv(1), ライトレジスト.GetLv(1), グルームレジスト.GetLv(1),
            
            アビリティ.GetLv(1),

            ミューテーションⅠ.Get(),
            ヴォル・ソール.Get(), グワナ・ソール.Get(), クォーツ・ソール.Get(), 
            ランサ・ソール.Get(), ファング・ソール.Get(), マイザー・ソール.Get(),
            ラグネ・ソール.Get(), 
            シグノ・ソール.Get(), ラッピー・ソール.Get(), 
            スノウ・ソール.Get(), ロックベア・ソール.Get(), 
            マルモ・ソール.Get(), ヴァーダー・ソール.Get(), 
            キャタ・ソール.Get(),

            スタミナ・ブースト.Get(), スピリタ・ブースト.Get()
        };

        public const int GEN_SP = 0;
        public static readonly int[] GEN = new int[] { -1, -1, 0, 1 };
        public static readonly int[] INH = new int[] { -1, 2, 3, 4 };
        private const float NaN = float.NaN;
        private const float UK = float.NaN;

        /// <summary>[(LV),(GEN[2-3] or INH[1-3])]</summary>
        public static readonly float[,] PROB_NORMAL_BASIC = {
            {   NaN,   NaN,   NaN,   NaN,   NaN }, // dummy
            {   NaN,   NaN, 1.00f, 1.00f, 1.00f },
            { 0.60f, 0.80f, 0.60f, 0.80f, 1.00f },
            { 0.30f, 0.50f, 0.60f, 0.80f, 1.00f },
            { 0.20f, 0.40f, 0.40f,    UK,    UK },
            { 0.10f, 0.30f, 0.20f,    UK,    UK }
        };
        /// <summary>[(LV),(GEN[2-3] or INH[1-3])]</summary>
        public static readonly float[,] PROB_NORMAL_ADDITIONAL = {
            {   NaN,   NaN,   NaN,   NaN,   NaN }, // dummy
            {   NaN,   NaN, 0.60f, 0.80f, 1.00f },
            { 0.60f, 0.80f, 0.40f, 0.60f, 0.80f },
            { 0.20f, 0.40f, 0.20f, 0.40f, 0.60f },
            { 0.20f, 0.40f, 0.20f, 0.30f, 0.50f },
            { 0.10f, 0.30f, 0.10f, 0.20f,    UK }
        };
        /// <summary>[(LV),(GEN_SP] or INH[1-3])]</summary>
        public static readonly float[,] PROB_NORMAL_ABILITY = {
            {   NaN,   NaN,   NaN,   NaN,   NaN }, // dummy
            { 0.80f,   NaN, 1.00f, 1.00f, 1.00f },
            { 0.69f,   NaN, 0.20f, 0.40f, 0.60f },
            { 0.60f,   NaN, 0.10f,    UK,    UK },                  
        };

        /// <summary>[(LV),(GEN[2-3] or INH[1-3])]</summary>
        public static readonly float[,] PROB_MUTATION1_BASIC = {
            {   NaN,   NaN,   NaN,   NaN,   NaN }, // dummy
            {   NaN,   NaN, 1.00f, 1.00f, 1.00f },
            { 0.60f, 0.80f, 0.60f, 0.80f, 1.00f },
            { 0.60f, 0.80f, 0.60f, 0.80f, 1.00f },
            { 0.20f, 0.40f, 0.40f,    UK,    UK },
            { 0.10f, 0.30f, 0.20f,    UK,    UK }                                          
        };
        /// <summary>[(LV),(GEN[2-3] or INH[1-3])]</summary>
        public static readonly float[,] PROB_MUTATION1_ADDITIONAL = {
            {   NaN,   NaN,   NaN,   NaN,   NaN }, // dummy
            {   NaN,   NaN, 0.60f, 0.80f, 1.00f },
            { 0.60f, 0.80f, 0.40f, 0.60f, 0.80f },
            { 0.60f, 0.80f, 0.20f, 0.40f, 0.60f },
            { 0.20f, 0.40f, 0.20f, 0.30f, 0.50f },
            { 0.10f, 0.30f, 0.10f, 0.20f,    UK }
        };

        /// <summary>[(LV),(GEN[2-3] or INH[1-3])]</summary>
        public static readonly float[,] PROB_SOUL_BASIC = {
            {   NaN,   NaN,   NaN,   NaN,   NaN }, // dummy
            {   NaN,   NaN, 1.00f, 1.00f, 1.00f },
            { 0.60f, 0.80f, 0.60f, 0.80f, 1.00f },
            { 0.50f, 0.69f, 0.80f, 1.00f, 1.00f },
            { 0.40f, 0.60f, 0.40f,    UK,    UK },
            { 0.10f, 0.30f, 0.20f,    UK,    UK }                                          
        };
        /// <summary>[(LV),(GEN[2-3] or INH[1-3])]</summary>
        public static readonly float[,] PROB_SOUL_ADDITIONAL = {
            {   NaN,   NaN,   NaN,   NaN,   NaN }, // dummy
            {   NaN,   NaN, 0.60f, 0.80f, 1.00f },
            { 0.60f, 0.80f, 0.40f, 0.60f, 0.80f },
            { 0.50f, 0.69f, 0.80f, 1.00f, 1.00f },
            { 0.40f, 0.60f, 0.20f, 0.30f, 0.50f },
            { 0.10f, 0.30f, 0.10f, 0.20f,    UK }
        };

        /// <summary>[INH[2-3]]</summary>
        public static readonly float[] PROB_SOUL = { NaN, NaN, NaN, 0.50f, 0.80f};

        /// <summary>[(現在スロット),(素材数)]</summary>
        public static readonly float[,] PROB_CORRECTION_EXTRA = {
            { NaN, 0.80f, 0.80f },
            { NaN, 0.70f, 0.75f },
            { NaN, 0.60f, 0.70f },
            { NaN, 0.50f, 0.60f },
            { NaN, 0.45f, 0.55f },
            { NaN, 0.40f, 0.50f },
            { NaN, 0.35f, 0.40f },
            { NaN, 0.30f, 0.30f }
        };
    }
}

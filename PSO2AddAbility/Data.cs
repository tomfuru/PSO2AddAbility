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
        public static IEnumerable<IAbility> GetMaterialAbilities(IAbility ability)
        {
            if (ability is Basic_up || ability is Additional || ability is Regist) {
                int level = ((ILevel)ability).Level;

                IAbility ab = Activator.CreateInstance(ability.GetType(), level - 1) as IAbility;
                yield return ab;
            }
            else if (ability is Ability) {
                int level = ((ILevel)ability).Level;

                yield return new パワー(level);
                yield return new シュート(level);
                yield return new テクニック(level);
            }
            else if (ability is Soul || ability is Special_up) {
                IAbility ab = Activator.CreateInstance(ability.GetType()) as IAbility;
                yield return ab;
                yield return ab;
            }

            yield break;
        }
        //-------------------------------------------------------------------------------
        #endregion (+[static]GetMaterialAbilities)

        public static readonly IAbility[] ALL_ABILITIES = {
            new パワー(0), new シュート(0), new テクニック(0), 
            new ボディ(0), new リアクト(0), new マインド(0), 
            new アーム(0), new スタミナ(0), new スピリタ(0),

            new ミューテーションⅠ(),
            new ヴォル・ソール(), new グワナ・ソール(), new クォーツ・ソール(), 
            new ランサ・ソール(), new ファング・ソール(), new マイザー・ソール(),
            new ラグネ・ソール(), 
            new シグノ・ソール(), new ラッピー・ソール(), 
            new スノウ・ソール(), new ロックベア・ソール(), 
            new マルモ・ソール(), new ヴァーダー・ソール(), 
            new キャタ・ソール(),

            new スタミナ・ブースト(), new スピリタ・ブースト(),

            new アビリティ(0),

            new バーン(0), new フリーズ(0), new ショック(0), 
            new ミラージュ(0), new パニック(0), new ポイズン(0),
            new ブロウレジスト(0), new ショットレジスト(0), new マインドレジスト(0),
            new フレイムレジスト(0), new アイスレジスト(0), new ショックレジスト(0), 
            new ウィンドレジスト(0), new ライトレジスト(0), new グルームレジスト(0)
        };

        const int GEN = 0;
        const int GEN_2 = 0;
        const int GEN_3 = 1;
        const int INH_1 = 2;
        const int INH_2 = 3;
        const int INH_3 = 4;
        private const float NaN = float.NaN;
        private const float UK = float.NaN;

        public static readonly float[,] PROB_NORMAL_BASIC = {
            {   NaN,   NaN,   NaN,   NaN,   NaN }, // dummy
            {   NaN,   NaN, 1.00f, 1.00f, 1.00f },
            { 0.60f, 0.80f, 0.60f, 0.80f, 1.00f },
            { 0.30f, 0.50f, 0.60f, 0.80f, 1.00f },
            { 0.20f, 0.40f, 0.40f,    UK,    UK },
            { 0.10f, 0.30f, 0.20f,    UK,    UK }
        };
        public static readonly float[,] PROB_NORMAL_ADDITIONAL = {
            {   NaN,   NaN,   NaN,   NaN,   NaN }, // dummy
            {   NaN,   NaN, 0.60f, 0.80f, 1.00f },
            { 0.60f, 0.80f, 0.40f, 0.60f, 0.80f },
            { 0.20f, 0.40f, 0.20f, 0.40f, 0.60f },
            { 0.20f, 0.40f, 0.20f, 0.30f, 0.50f },
            { 0.10f, 0.30f, 0.10f, 0.20f,    UK }
        };
        public static readonly float[,] PROB_NORMAL_ABILITY = {
            {   NaN,   NaN,   NaN,   NaN,   NaN }, // dummy
            { 0.80f,   NaN, 1.00f, 1.00f, 1.00f },
            { 0.69f,   NaN, 0.20f, 0.40f, 0.60f },
            { 0.60f,   NaN, 0.10f,    UK,    UK },                  
        };

        public static readonly float[,] PROB_MUTATION1_BASIC = {
            {   NaN,   NaN,   NaN,   NaN,   NaN }, // dummy
            {   NaN,   NaN, 1.00f, 1.00f, 1.00f },
            { 0.60f, 0.80f, 0.60f, 0.80f, 1.00f },
            { 0.60f, 0.80f, 0.60f, 0.80f, 1.00f },
            { 0.20f, 0.40f, 0.40f,    UK,    UK },
            { 0.10f, 0.30f, 0.20f,    UK,    UK }                                          
        };
        public static readonly float[,] PROB_MUTATION1_ADDITIONAL = {
            {   NaN,   NaN,   NaN,   NaN,   NaN }, // dummy
            {   NaN,   NaN, 0.60f, 0.80f, 1.00f },
            { 0.60f, 0.80f, 0.40f, 0.60f, 0.80f },
            { 0.60f, 0.80f, 0.20f, 0.40f, 0.60f },
            { 0.20f, 0.40f, 0.20f, 0.30f, 0.50f },
            { 0.10f, 0.30f, 0.10f, 0.20f,    UK }
        };

        public static readonly float[,] PROB_SOUL_BASIC = {
            {   NaN,   NaN,   NaN,   NaN,   NaN }, // dummy
            {   NaN,   NaN, 1.00f, 1.00f, 1.00f },
            { 0.60f, 0.80f, 0.60f, 0.80f, 1.00f },
            { 0.50f, 0.69f, 0.80f, 1.00f, 1.00f },
            { 0.40f, 0.60f, 0.40f,    UK,    UK },
            { 0.10f, 0.30f, 0.20f,    UK,    UK }                                          
        };
        public static readonly float[,] PROB_SOUL_ADDITIONAL = {
            {   NaN,   NaN,   NaN,   NaN,   NaN }, // dummy
            {   NaN,   NaN, 0.60f, 0.80f, 1.00f },
            { 0.60f, 0.80f, 0.40f, 0.60f, 0.80f },
            { 0.50f, 0.69f, 0.80f, 1.00f, 1.00f },
            { 0.40f, 0.60f, 0.20f, 0.30f, 0.50f },
            { 0.10f, 0.30f, 0.10f, 0.20f,    UK }
        };

        /// <summary>[現在スロット,素材数]</summary>
        public static readonly float[,] PROB_CORRECTION_EXTRA = {
            { NaN, 0.80f, 0.80f },
            { NaN, 0.70f, 0.75f },
            { NaN, 0.60f, 0.70f },
            { NaN, 0.50f, 0.60f },
            { NaN, 0.45f, 0.55f },
            { NaN, 0.40f, 0.50f },
            { NaN, 0.35f, 0.40f },
            { NaN,    UK,    UK }
        };
    }
}

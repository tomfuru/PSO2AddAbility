using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

[assembly: CLSCompliant(true)]

namespace PSO2AddAbility
{
    //-------------------------------------------------------------------------------
    #region AbilityType 列挙体：
    //-------------------------------------------------------------------------------
    /// <summary>
    /// 
    /// </summary>
    public enum AbilityType
    {
        パワー, シュート, テクニック,
        ボディ, リアクト, マインド,
        アーム, スタミナ, スピリタ,

        バーン, フリーズ, ショック,
        ミラージュ, パニック, ポイズン,
        ブロウレジスト, ショットレジスト, マインドレジスト,
        フレイムレジスト, アイスレジスト, ショックレジスト,
        ウィンドレジスト, ライトレジスト, グルームレジスト,

        アビリティ,

        ミューテーション,
        ヴォルソール, グワナソール, クォーツソール,
        ランサソール, ファングソール, マイザーソール,
        ラグネソール,
        シグノソール, ラッピーソール,
        スノウソール, ロックベアソール,
        マルモソール, ヴァーダーソール,
        キャタソール, ウォルガソール,
        シュレイダソール, エルダーソール,

        スタミナブースト, スピリタブースト
    }
    //-------------------------------------------------------------------------------
    #endregion (AbilityType)

    public static class Data
    {
        //-------------------------------------------------------------------------------
        #region +[static]GetMaterialAbilities
        //-------------------------------------------------------------------------------
        public static IEnumerable<IEnumerable<IAbility>> GetMaterialAbilities(IAbility ability)
        {
            if (ability is Basic_up || ability is Additional) {
                // (元の能力*1,2,3) or (一つ下の能力*2,3)
                yield return new[] { ability, ability, ability };
                yield return new[] { ability, ability };
                yield return new[] { ability };

                ILevel ability_l = ability as ILevel;
                int level = ability_l.Level;

                if (level > 1) {
                    //IAbility ab = Activator.CreateInstance(ability.GetType(), level - 1) as IAbility; 下に代替
                    IAbility ab = ability_l.GetInstanceOfLv(level - 1);
                    yield return new[] { ab, ab, ab };
                    yield return new[] { ab, ab };
                }
            }
            else if (ability is Ability) {
                // (パワー,シュート,テクニック各1) or (元の能力*1,2,3)
                int level = ((ILevel)ability).Level;
                yield return new IAbility[] { パワー.GetLv(level), シュート.GetLv(level), テクニック.GetLv(level) };

                yield return new[] { ability, ability, ability };
                yield return new[] { ability, ability };
                yield return new[] { ability };
            }
            else if (ability is Soul || ability is Special_up) {
                // (元の能力*2,3)
                yield return new[] { ability, ability, ability };
                yield return new[] { ability, ability };
            }

            yield break;
        }
        //-------------------------------------------------------------------------------
        #endregion (+[static]GetMaterialAbilities)

        public static readonly IAbility[] ALL_ABILITIES;
        public static readonly Dictionary<IAbility, AbilityType> DIC_IABILITY_TO_ABILITYTYPE = new Dictionary<IAbility, AbilityType>() {
                #region IAbility <-> AbilityType関係宣言
                { パワー.GetLv(1), AbilityType.パワー }, { シュート.GetLv(1), AbilityType.シュート }, { テクニック.GetLv(1), AbilityType.テクニック }, 
                { ボディ.GetLv(1), AbilityType.ボディ }, { リアクト.GetLv(1), AbilityType.リアクト }, { マインド.GetLv(1), AbilityType.マインド }, 
                { アーム.GetLv(1), AbilityType.アーム }, { スタミナ.GetLv(1), AbilityType.スタミナ }, { スピリタ.GetLv(1), AbilityType.スピリタ },
            
                { バーン.GetLv(1), AbilityType.バーン }, { フリーズ.GetLv(1), AbilityType.フリーズ }, { ショック.GetLv(1), AbilityType.ショック }, 
                { ミラージュ.GetLv(1), AbilityType.ミラージュ }, { パニック.GetLv(1), AbilityType.パニック }, { ポイズン.GetLv(1), AbilityType.ポイズン }, 
                { ブロウレジスト.GetLv(1), AbilityType.ブロウレジスト }, { ショットレジスト.GetLv(1), AbilityType.ショットレジスト },　{ マインドレジスト.GetLv(1), AbilityType.マインドレジスト },
                { フレイムレジスト.GetLv(1), AbilityType.フレイムレジスト }, { アイスレジスト.GetLv(1), AbilityType.アイスレジスト }, { ショックレジスト.GetLv(1), AbilityType.ショックレジスト },
                { ウィンドレジスト.GetLv(1), AbilityType.ウィンドレジスト }, { ライトレジスト.GetLv(1), AbilityType.ライトレジスト }, { グルームレジスト.GetLv(1), AbilityType.グルームレジスト },
            
                { アビリティ.GetLv(1), AbilityType.アビリティ },

                { ミューテーション.GetLv(1), AbilityType.ミューテーション },
                { ヴォル・ソール.Get(), AbilityType.ヴォルソール }, { グワナ・ソール.Get(), AbilityType.グワナソール }, { クォーツ・ソール.Get(), AbilityType.クォーツソール }, 
                { ランサ・ソール.Get(), AbilityType.ランサソール }, { ファング・ソール.Get(), AbilityType.ファングソール }, { マイザー・ソール.Get(), AbilityType.マイザーソール },
                { ラグネ・ソール.Get(), AbilityType.ラグネソール }, 
                { シグノ・ソール.Get(), AbilityType.シグノソール }, { ラッピー・ソール.Get(), AbilityType.ラッピーソール }, 
                { スノウ・ソール.Get(), AbilityType.スノウソール }, { ロックベア・ソール.Get(), AbilityType.ロックベアソール }, 
                { マルモ・ソール.Get(), AbilityType.マルモソール }, { ヴァーダー・ソール.Get(), AbilityType.ヴァーダーソール }, 
                { キャタ・ソール.Get(), AbilityType.キャタソール }, { ウォルガ・ソール.Get(), AbilityType.ウォルガソール },
                { シュレイダ・ソール.Get(), AbilityType.シュレイダソール }, { エルダー・ソール.Get(), AbilityType.エルダーソール },

                { スタミナ・ブースト.Get(), AbilityType.スタミナブースト }, { スピリタ・ブースト.Get(), AbilityType.スピリタブースト }
                #endregion 
            };
        public static readonly Dictionary<AbilityType, IAbility> DIC_ABILITYTYPE_TO_IABILITY;

        //-------------------------------------------------------------------------------
        #region static Constructor
        //-------------------------------------------------------------------------------
        //
        static Data()
        {
            DIC_ABILITYTYPE_TO_IABILITY = new Dictionary<AbilityType, IAbility>();
            DIC_IABILITY_TO_ABILITYTYPE.ToList().ForEach(kvp => DIC_ABILITYTYPE_TO_IABILITY.Add(kvp.Value, kvp.Key));

            // LV2以上のものを追加
            DIC_IABILITY_TO_ABILITYTYPE
                .Where(kvp => kvp.Key is ILevel)
                .ToList()
                .ForEach(kvp =>
                {
                    ILevel ab_lv = kvp.Key as ILevel;
                    ab_lv.AllLevels()
                            .ToList()
                            .ForEach(level =>
                            {
                                IAbility ab = ab_lv.GetInstanceOfLv(level);
                                if (!DIC_IABILITY_TO_ABILITYTYPE.ContainsKey(ab)) {
                                    DIC_IABILITY_TO_ABILITYTYPE.Add(ab, kvp.Value);
                                }
                            });
                });

            ALL_ABILITIES = Enum.GetValues(typeof(AbilityType)).Cast<AbilityType>().Select(type => DIC_ABILITYTYPE_TO_IABILITY[type]).ToArray();
        }
        #endregion (static Constructor)

        //-------------------------------------------------------------------------------
        #region +[static]GetAllAbilitiesIncludedLevel レベルも考慮した全アビリティの列挙
        //-------------------------------------------------------------------------------
        //
        public static IEnumerable<IAbility> GetAllAbilitiesIncludedLevel()
        {
            return Data.ALL_ABILITIES.SelectMany(ab =>
                (ab is ILevel) ? (ab as ILevel).AllLevels().Select(i => (ab as ILevel).GetInstanceOfLv(i))
                               : new IAbility[] { ab }
            );
        }
        #endregion (GetAllAbilitiesIncludedLevel)


        public const int GEN_SP = 0;
        public static readonly int[] GEN = new int[] { -1, -1, 0, 1 };
        public static readonly int[] INH = new int[] { -1, 2, 3, 4 };
        private const float NaN = float.NaN;

        //-------------------------------------------------------------------------------
        #region 確率リスト
        //-------------------------------------------------------------------------------
        /// <summary>[(LV),(GEN[2-3] or INH[1-3])]</summary>
        public static readonly float[,] PROB_NORMAL_BASIC = {
            {   NaN,   NaN,   NaN,   NaN,   NaN }, // dummy
            {   NaN,   NaN, 1.00f, 1.00f, 1.00f },
            { 0.60f, 0.80f, 0.60f, 0.80f, 1.00f },
            { 0.30f, 0.50f, 0.60f, 0.80f, 1.00f },
            { 0.20f, 0.40f, 0.40f, 0.60f, 0.80f },
            { 0.10f, 0.30f, 0.20f, 0.40f, 0.60f }
        };
        /// <summary>[(LV),(GEN[2-3] or INH[1-3])]</summary>
        public static readonly float[,] PROB_NORMAL_ADDITIONAL = {
            {   NaN,   NaN,   NaN,   NaN,   NaN }, // dummy
            {   NaN,   NaN, 0.60f, 0.80f, 1.00f },
            { 0.60f, 0.80f, 0.40f, 0.60f, 0.80f },
            { 0.20f, 0.40f, 0.20f, 0.40f, 0.60f },
            { 0.20f, 0.40f, 0.20f, 0.30f, 0.50f },
            { 0.10f, 0.30f, 0.10f, 0.20f, 0.40f }
        };
        /// <summary>[(LV),(GEN_SP] or INH[1-3])]</summary>
        public static readonly float[,] PROB_NORMAL_ABILITY = {
            {   NaN,   NaN,   NaN,   NaN,   NaN }, // dummy
            { 0.80f,   NaN, 1.00f, 1.00f, 1.00f },
            { 0.69f,   NaN, 0.20f, 0.40f, 0.60f },
            { 0.60f,   NaN, 0.10f, 0.40f, 0.60f }                  
        };

        /// <summary>[(LV),(GEN[2-3] or INH[1-3])]</summary>
        public static readonly float[,] PROB_MUTATION1_BASIC = {
            {   NaN,   NaN,   NaN,   NaN,   NaN }, // dummy
            {   NaN,   NaN, 1.00f, 1.00f, 1.00f },
            { 0.60f, 0.80f, 0.60f, 0.80f, 1.00f },
            { 0.60f, 0.80f, 0.60f, 0.80f, 1.00f },
            { 0.20f, 0.40f, 0.40f, 0.60f, 0.80f },
            { 0.10f, 0.30f, 0.20f, 0.40f, 0.60f }                                    
        };
        /// <summary>[(LV),(GEN[2-3] or INH[1-3])]</summary>
        public static readonly float[,] PROB_MUTATION1_ADDITIONAL = {
            {   NaN,   NaN,   NaN,   NaN,   NaN }, // dummy
            {   NaN,   NaN, 0.60f, 0.80f, 1.00f },
            { 0.60f, 0.80f, 0.40f, 0.60f, 0.80f },
            { 0.60f, 0.80f, 0.20f, 0.40f, 0.60f },
            { 0.20f, 0.40f, 0.20f, 0.30f, 0.50f },
            { 0.10f, 0.30f, 0.10f, 0.20f, 0.40f }
        };

        /// <summary>[(LV),(GEN[2-3] or INH[1-3])]</summary>
        public static readonly float[,] PROB_SOUL_BASIC = {
            {   NaN,   NaN,   NaN,   NaN,   NaN }, // dummy
            {   NaN,   NaN, 1.00f, 1.00f, 1.00f },
            { 0.60f, 0.80f, 0.60f, 0.80f, 1.00f },
            { 0.50f, 0.69f, 0.80f, 1.00f, 1.00f },
            { 0.40f, 0.60f, 0.40f, 0.60f, 0.80f },
            { 0.10f, 0.30f, 0.20f, 0.40f, 0.60f }
        };
        /// <summary>[(LV),(GEN[2-3] or INH[1-3])]</summary>
        public static readonly float[,] PROB_SOUL_ADDITIONAL = {
            {   NaN,   NaN,   NaN,   NaN,   NaN }, // dummy
            {   NaN,   NaN, 0.60f, 0.80f, 1.00f },
            { 0.60f, 0.80f, 0.40f, 0.60f, 0.80f },
            { 0.50f, 0.69f, 0.80f, 1.00f, 1.00f },
            { 0.40f, 0.60f, 0.20f, 0.30f, 0.50f },
            { 0.10f, 0.30f, 0.10f, 0.20f, 0.40f }
        };

        /// <summary>[INH[2-3]]</summary>
        public static readonly float[] PROB_SOUL = { NaN, NaN, NaN, 0.50f, 0.80f };

        /// <summary>[(現在スロット),(素材数)]</summary>
        public static readonly float[,] PROB_CORRECTION_EXTRA = {
            { NaN, NaN, 0.80f, 0.80f },
            { NaN, NaN, 0.70f, 0.75f },
            { NaN, NaN, 0.60f, 0.70f },
            { NaN, NaN, 0.50f, 0.60f },
            { NaN, NaN, 0.45f, 0.55f },
            { NaN, NaN, 0.40f, 0.50f },
            { NaN, NaN, 0.35f, 0.40f },
            { NaN, NaN, 0.30f, 0.30f }
        };
        //-------------------------------------------------------------------------------
        #endregion 確率リスト
        //-------------------------------------------------------------------------------
    }
}

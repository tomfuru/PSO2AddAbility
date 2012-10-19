using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSO2AddAbility
{
    public class Weapon : IEquatable<Weapon>
    {
        public IAbility[] abilities;

        public bool Equals(Weapon other)
        {
            return (abilities.Length == other.abilities.Length)
                && (abilities.All(ab => other.abilities.Contains(ab)));
        }

        public override string ToString()
        {
            if (abilities == null) { return "Invalid Weapon"; }
            return abilities.AllToString();
        }
    }

    public class ゴミ : ToStringC, IAbility
    {
        private ゴミ() { }
        private static ゴミ value = new ゴミ();
        public static ゴミ Get() { return value; }
    }

    public class パワー : Basic_up, IMutationAmplifiable, ISoulAmplifiable<ファング・ソール>
    {
        private パワー(int lv) { Level = lv; }
        private static パワー[] values = new パワー[] { null, new パワー(1), new パワー(2), new パワー(3), new パワー(4), new パワー(5) };
        public static IAbility GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }
    public class シュート : Basic_up, IMutationAmplifiable, ISoulAmplifiable<マイザー・ソール>
    {
        public シュート(int lv) { Level = lv; }
        private static シュート[] values = new シュート[] { null, new シュート(1), new シュート(2), new シュート(3), new シュート(4), new シュート(5) };
        public static シュート GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }
    public class テクニック : Basic_up, IMutationAmplifiable, ISoulAmplifiable<クォーツ・ソール>
    {
        public テクニック(int lv) { Level = lv; }
        private static テクニック[] values = new テクニック[] { null, new テクニック(1), new テクニック(2), new テクニック(3), new テクニック(4), new テクニック(5) };
        public static テクニック GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }
    public class ボディ : Basic_up, IMutationAmplifiable, ISoulAmplifiable<スノウ・ソール>
    {
        public ボディ(int lv) { Level = lv; }
        private static ボディ[] values = new ボディ[] { null, new ボディ(1), new ボディ(2), new ボディ(3), new ボディ(4), new ボディ(5) };
        public static ボディ GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }
    public class リアクト : Basic_up, IMutationAmplifiable, ISoulAmplifiable<ラグネ・ソール>
    {
        public リアクト(int lv) { Level = lv; }
        private static リアクト[] values = new リアクト[] { null, new リアクト(1), new リアクト(2), new リアクト(3), new リアクト(4), new リアクト(5) };
        public static リアクト GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }
    public class マインド : Basic_up, IMutationAmplifiable, ISoulAmplifiable<ロックベア・ソール>
    {
        public マインド(int lv) { Level = lv; }
        private static マインド[] values = new マインド[] { null, new マインド(1), new マインド(2), new マインド(3), new マインド(4), new マインド(5) };
        public static マインド GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }
    public class アーム : Basic_up, IMutationAmplifiable, ISoulAmplifiable<ヴォル・ソール>
    {
        public アーム(int lv) { Level = lv; }
        private static アーム[] values = new アーム[] { null, new アーム(1), new アーム(2), new アーム(3), new アーム(4), new アーム(5) };
        public static アーム GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }
    public class スタミナ : Basic_up, IMutationAmplifiable, ISoulAmplifiable<マルモ・ソール>
    {
        public スタミナ(int lv) { Level = lv; }
        private static スタミナ[] values = new スタミナ[] { null, new スタミナ(1), new スタミナ(2), new スタミナ(3), new スタミナ(4), new スタミナ(5) };
        public static スタミナ GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }
    public class スピリタ : Basic_up, IMutationAmplifiable, ISoulAmplifiable<ラッピー・ソール>
    {
        public スピリタ(int lv) { Level = lv; }
        private static スピリタ[] values = new スピリタ[] { null, new スピリタ(1), new スピリタ(2), new スピリタ(3), new スピリタ(4), new スピリタ(5) };
        public static スピリタ GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }

    public class バーン : Additional, IMutationAmplifiable, ISoulAmplifiable<ヴォル・ソール>
    {
        public バーン(int lv) { Level = lv; }
        private static バーン[] values = new バーン[] { null, new バーン(1), new バーン(2), new バーン(3), new バーン(4), new バーン(5) };
        public static バーン GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }
    public class フリーズ : Additional, IMutationAmplifiable, ISoulAmplifiable<マルモ・ソール>
    {
        public フリーズ(int lv) { Level = lv; }
        private static フリーズ[] values = new フリーズ[] { null, new フリーズ(1), new フリーズ(2), new フリーズ(3), new フリーズ(4), new フリーズ(5) };
        public static フリーズ GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }
    public class ショック : Additional, IMutationAmplifiable, ISoulAmplifiable<ファング・ソール>
    {
        public ショック(int lv) { Level = lv; }
        private static ショック[] values = new ショック[] { null, new ショック(1), new ショック(2), new ショック(3), new ショック(4), new ショック(5) };
        public static ショック GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }
    public class ミラージュ : Additional, IMutationAmplifiable, ISoulAmplifiable<シグノ・ソール>
    {
        public ミラージュ(int lv) { Level = lv; }
        private static ミラージュ[] values = new ミラージュ[] { null, new ミラージュ(1), new ミラージュ(2), new ミラージュ(3), new ミラージュ(4), new ミラージュ(5) };
        public static ミラージュ GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }
    public class パニック : Additional, IMutationAmplifiable, ISoulAmplifiable<クォーツ・ソール>
    {
        public パニック(int lv) { Level = lv; }
        private static パニック[] values = new パニック[] { null, new パニック(1), new パニック(2), new パニック(3), new パニック(4), new パニック(5) };
        public static パニック GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }
    public class ポイズン : Additional, IMutationAmplifiable, ISoulAmplifiable<グワナ・ソール>
    {
        public ポイズン(int lv) { Level = lv; }
        private static ポイズン[] values = new ポイズン[] { null, new ポイズン(1), new ポイズン(2), new ポイズン(3), new ポイズン(4), new ポイズン(5) };
        public static ポイズン GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }

    public class ブロウレジスト : Regist, IMutationAmplifiable
    {
        public ブロウレジスト(int lv) { Level = lv; }
        private static ブロウレジスト[] values = new ブロウレジスト[] { null, new ブロウレジスト(1), new ブロウレジスト(2), new ブロウレジスト(3), new ブロウレジスト(4), new ブロウレジスト(5) };
        public static ブロウレジスト GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }
    public class ショットレジスト : Regist, IMutationAmplifiable, ISoulAmplifiable<ヴァーダー・ソール>
    {
        public ショットレジスト(int lv) { Level = lv; }
        private static ショットレジスト[] values = new ショットレジスト[] { null, new ショットレジスト(1), new ショットレジスト(2), new ショットレジスト(3), new ショットレジスト(4), new ショットレジスト(5) };
        public static ショットレジスト GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }
    public class マインドレジスト : Regist, IMutationAmplifiable
    {
        public マインドレジスト(int lv) { Level = lv; }
        private static マインドレジスト[] values = new マインドレジスト[] { null, new マインドレジスト(1), new マインドレジスト(2), new マインドレジスト(3), new マインドレジスト(4), new マインドレジスト(5) };
        public static マインドレジスト GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }
    public class フレイムレジスト : Regist, IMutationAmplifiable, ISoulAmplifiable<キャタ・ソール>
    {
        public
            フレイムレジスト(int lv) { Level = lv; }
        private static フレイムレジスト[] values = new フレイムレジスト[] { null, new フレイムレジスト(1), new フレイムレジスト(2), new フレイムレジスト(3), new フレイムレジスト(4), new フレイムレジスト(5) };
        public static フレイムレジスト GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }
    public class アイスレジスト : Regist, IMutationAmplifiable, ISoulAmplifiable<スノウ・ソール>
    {
        public アイスレジスト(int lv) { Level = lv; }
        private static アイスレジスト[] values = new アイスレジスト[] { null, new アイスレジスト(1), new アイスレジスト(2), new アイスレジスト(3), new アイスレジスト(4), new アイスレジスト(5) };
        public static アイスレジスト GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }
    public class ショックレジスト : Regist, IMutationAmplifiable
    {
        public ショックレジスト(int lv) { Level = lv; }
        private static ショックレジスト[] values = new ショックレジスト[] { null, new ショックレジスト(1), new ショックレジスト(2), new ショックレジスト(3), new ショックレジスト(4), new ショックレジスト(5) };
        public static ショックレジスト GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }
    public class ウィンドレジスト : Regist, IMutationAmplifiable, ISoulAmplifiable<ヴァーダー・ソール>
    {
        public ウィンドレジスト(int lv) { Level = lv; }
        private static ウィンドレジスト[] values = new ウィンドレジスト[] { null, new ウィンドレジスト(1), new ウィンドレジスト(2), new ウィンドレジスト(3), new ウィンドレジスト(4), new ウィンドレジスト(5) };
        public static ウィンドレジスト GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }
    public class ライトレジスト : Regist, IMutationAmplifiable, ISoulAmplifiable<ラッピー・ソール>
    {
        public ライトレジスト(int lv) { Level = lv; }
        private static ライトレジスト[] values = new ライトレジスト[] { null, new ライトレジスト(1), new ライトレジスト(2), new ライトレジスト(3), new ライトレジスト(4), new ライトレジスト(5) };
        public static ライトレジスト GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }
    public class グルームレジスト : Regist, IMutationAmplifiable, ISoulAmplifiable<ラグネ・ソール>
    {
        public グルームレジスト(int lv) { Level = lv; }
        private static グルームレジスト[] values = new グルームレジスト[] { null, new グルームレジスト(1), new グルームレジスト(2), new グルームレジスト(3), new グルームレジスト(4), new グルームレジスト(5) };
        public static グルームレジスト GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }

    public class アビリティ : Ability
    {
        public アビリティ(int lv) { Level = lv; }
        private static アビリティ[] values = new アビリティ[] { null, new アビリティ(1), new アビリティ(2), new アビリティ(3) };
        public static アビリティ GetLv(int lv) { return values[lv]; }
        public override IAbility GetInstanceOfLv(int lv) { return values[lv]; }
    }

    public class スタミナ・ブースト : Boost
    {
        private スタミナ・ブースト() { }
        private static スタミナ・ブースト value = new スタミナ・ブースト();
        public static スタミナ・ブースト Get() { return value; }
    }
    public class スピリタ・ブースト : Boost
    {
        private スピリタ・ブースト() { }
        private static スピリタ・ブースト value = new スピリタ・ブースト();
        public static スピリタ・ブースト Get() { return value; }
    }

    public class ミューテーションⅠ : Special_up
    {
        private ミューテーションⅠ() { }
        private static ミューテーションⅠ value = new ミューテーションⅠ();
        public static ミューテーションⅠ Get() { return value; }
    }
    public class スティグマ : Special_up
    {
        private スティグマ() { }
        private static スティグマ value = new スティグマ();
        public static スティグマ Get() { return value; }
    }
    public class ヴォル・ソール : Soul
    {
        private ヴォル・ソール() { }
        private static ヴォル・ソール value = new ヴォル・ソール();
        public static ヴォル・ソール Get() { return value; }
    }
    public class グワナ・ソール : Soul
    {
        private グワナ・ソール() { }
        private static グワナ・ソール value = new グワナ・ソール();
        public static グワナ・ソール Get() { return value; }
    }
    public class クォーツ・ソール : Soul
    {
        private クォーツ・ソール() { }
        private static クォーツ・ソール value = new クォーツ・ソール();
        public static クォーツ・ソール Get() { return value; }
    }
    public class ランサ・ソール : Soul
    {
        private ランサ・ソール() { }
        private static ランサ・ソール value = new ランサ・ソール();
        public static ランサ・ソール Get() { return value; }
    }
    public class ファング・ソール : Soul
    {
        private ファング・ソール() { }
        private static ファング・ソール value = new ファング・ソール();
        public static ファング・ソール Get() { return value; }
    }
    public class マイザー・ソール : Soul
    {
        private マイザー・ソール() { }
        private static マイザー・ソール value = new マイザー・ソール();
        public static マイザー・ソール Get() { return value; }
    }
    public class ラグネ・ソール : Soul
    {
        private ラグネ・ソール() { }
        private static ラグネ・ソール value = new ラグネ・ソール();
        public static ラグネ・ソール Get() { return value; }
    }
    public class シグノ・ソール : Soul
    {
        private シグノ・ソール() { }
        private static シグノ・ソール value = new シグノ・ソール();
        public static シグノ・ソール Get() { return value; }
    }
    public class ラッピー・ソール : Soul
    {
        private ラッピー・ソール() { }
        private static ラッピー・ソール value = new ラッピー・ソール();
        public static ラッピー・ソール Get() { return value; }
    }
    public class スノウ・ソール : Soul
    {
        private スノウ・ソール() { }
        private static スノウ・ソール value = new スノウ・ソール();
        public static スノウ・ソール Get() { return value; }
    }
    public class ロックベア・ソール : Soul
    {
        private ロックベア・ソール() { }
        private static ロックベア・ソール value = new ロックベア・ソール();
        public static ロックベア・ソール Get() { return value; }
    }
    public class マルモ・ソール : Soul
    {
        private マルモ・ソール() { }
        private static マルモ・ソール value = new マルモ・ソール();
        public static マルモ・ソール Get() { return value; }
    }
    public class ヴァーダー・ソール : Soul
    {
        private ヴァーダー・ソール() { }
        private static ヴァーダー・ソール value = new ヴァーダー・ソール();
        public static ヴァーダー・ソール Get() { return value; }
    }
    public class キャタ・ソール : Soul
    {
        private キャタ・ソール() { }
        private static キャタ・ソール value = new キャタ・ソール();
        public static キャタ・ソール Get() { return value; }
    }
}

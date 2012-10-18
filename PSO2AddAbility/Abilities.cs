using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSO2AddAbility
{
    public class Weapon
    {
        public IAbility[] abilities;
    }

    public class ゴミ : ToStringC, IAbility { }

    public class パワー : Basic_up, IMutationAmplifiable, ISoulAmplifiable<ファング・ソール> { public パワー(int lv) { Level = lv; } }
    public class シュート : Basic_up, IMutationAmplifiable, ISoulAmplifiable<マイザー・ソール> { public シュート(int lv) { Level = lv; } }
    public class テクニック : Basic_up, IMutationAmplifiable, ISoulAmplifiable<クォーツ・ソール> { public テクニック(int lv) { Level = lv; } }
    public class ボディ : Basic_up, IMutationAmplifiable, ISoulAmplifiable<スノウ・ソール> { public ボディ(int lv) { Level = lv; } }
    public class リアクト : Basic_up, IMutationAmplifiable, ISoulAmplifiable<ラグネ・ソール> { public リアクト(int lv) { Level = lv; } }
    public class マインド : Basic_up, IMutationAmplifiable, ISoulAmplifiable<ロックベア・ソール> { public マインド(int lv) { Level = lv; } }
    public class アーム : Basic_up, IMutationAmplifiable, ISoulAmplifiable<ヴォル・ソール> { public アーム(int lv) { Level = lv; } }
    public class スタミナ : Basic_up, IMutationAmplifiable, ISoulAmplifiable<マルモ・ソール> { public スタミナ(int lv) { Level = lv; } }
    public class スピリタ : Basic_up, IMutationAmplifiable, ISoulAmplifiable<ラッピー・ソール> { public スピリタ(int lv) { Level = lv; } }

    public class バーン : Additional, IMutationAmplifiable, ISoulAmplifiable<ヴォル・ソール> { public バーン(int lv) { Level = lv; } }
    public class フリーズ : Additional, IMutationAmplifiable, ISoulAmplifiable<マルモ・ソール> { public フリーズ(int lv) { Level = lv; } }
    public class ショック : Additional, IMutationAmplifiable, ISoulAmplifiable<ファング・ソール> { public ショック(int lv) { Level = lv; } }
    public class ミラージュ : Additional, IMutationAmplifiable, ISoulAmplifiable<シグノ・ソール> { public ミラージュ(int lv) { Level = lv; } }
    public class パニック : Additional, IMutationAmplifiable, ISoulAmplifiable<クォーツ・ソール> { public パニック(int lv) { Level = lv; } }
    public class ポイズン : Additional, IMutationAmplifiable, ISoulAmplifiable<グワナ・ソール> { public ポイズン(int lv) { Level = lv; } }

    public class ブロウレジスト : Regist, IMutationAmplifiable { public ブロウレジスト(int lv) { Level = lv; } }
    public class ショットレジスト : Regist, IMutationAmplifiable, ISoulAmplifiable<ヴァーダー・ソール> { public ショットレジスト(int lv) { Level = lv; } }
    public class マインドレジスト : Regist, IMutationAmplifiable { public マインドレジスト(int lv) { Level = lv; } }
    public class フレイムレジスト : Regist, IMutationAmplifiable, ISoulAmplifiable<キャタ・ソール> { public フレイムレジスト(int lv) { Level = lv; } }
    public class アイスレジスト : Regist, IMutationAmplifiable, ISoulAmplifiable<スノウ・ソール> { public アイスレジスト(int lv) { Level = lv; } }
    public class ショックレジスト : Regist, IMutationAmplifiable  { public ショックレジスト(int lv) { Level = lv; } }
    public class ウィンドレジスト : Regist, IMutationAmplifiable, ISoulAmplifiable<ヴァーダー・ソール> { public ウィンドレジスト(int lv) { Level = lv; } }
    public class ライトレジスト : Regist, IMutationAmplifiable, ISoulAmplifiable<ラッピー・ソール> { public ライトレジスト(int lv) { Level = lv; } }
    public class グルームレジスト : Regist, IMutationAmplifiable, ISoulAmplifiable<ラグネ・ソール> { public グルームレジスト(int lv) { Level = lv; } }

    public class アビリティ : Ability { public アビリティ(int lv) { Level = lv; } }

    public class スタミナ・ブースト : Boost { }
    public class スピリタ・ブースト : Boost { }

    public class ミューテーションⅠ : Special_up { }
    //public class スティグマ : Secial_up { }
    public class ヴォル・ソール : Soul { }
    public class グワナ・ソール : Soul { }
    public class クォーツ・ソール : Soul { }
    public class ランサ・ソール : Soul { }
    public class ファング・ソール : Soul { }
    public class マイザー・ソール : Soul { }
    public class ラグネ・ソール : Soul { }
    public class シグノ・ソール : Soul { }
    public class ラッピー・ソール : Soul { }
    public class スノウ・ソール : Soul { }
    public class ロックベア・ソール : Soul { }
    public class マルモ・ソール : Soul { }
    public class ヴァーダー・ソール : Soul { }
    public class キャタ・ソール : Soul { }
}

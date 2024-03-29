﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSO2AddAbility
{
    /// <summary>能力UP</summary>
    public abstract class Basic_up : ToStringC, IWeapon, IUnit, ILevel, Inheritable { public abstract IEnumerable<int> AllLevels(); public int Level { get; protected set; } public abstract IAbility GetInstanceOfLv(int lv); }
    /// <summary>追加効果</summary>
    public abstract class Additional : ToStringC, IWeapon, ILevel, Inheritable { public abstract IEnumerable<int> AllLevels(); public int Level { get; protected set; } public abstract IAbility GetInstanceOfLv(int lv); }
    /// <summary>○○・ブースト</summary>
    public abstract class Boost : ToStringC, IWeapon, IUnit { }
    /// <summary>アビリティ</summary>
    public abstract class Ability : ToStringC, IWeapon, IUnit, ILevel { public abstract IEnumerable<int> AllLevels(); public int Level { get; protected set; } public abstract IAbility GetInstanceOfLv(int lv); }
    /// <summary>ミューテーションⅠ，スティグマ</summary>
    public abstract class Special_up : ToStringC, IWeapon, IUnit, Inheritable { }
    /// <summary>○○・ソール</summary>
    public abstract class Soul : ToStringC, IWeapon, IUnit, Inheritable { public abstract bool IsAmplifiableAbility(IAbility ab); }

    //-------------------------------------------------------------------------------
    #region abstract class ToStringC
    //-------------------------------------------------------------------------------
    public abstract class ToStringC
    {
        //-------------------------------------------------------------------------------
        #region +[override]ToString
        //-------------------------------------------------------------------------------
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(this.GetType().Name);

            if (this is ILevel) {
                ILevel lev = this as ILevel;
                switch (lev.Level) {
                    case 1: sb.Append('Ⅰ'); break;
                    case 2: sb.Append('Ⅱ'); break;
                    case 3: sb.Append('Ⅲ'); break;
                    case 4: sb.Append('Ⅳ'); break;
                    case 5: sb.Append('Ⅴ'); break;
                    default:
                        break;
                }
            }

            return sb.ToString();
        }
        //-------------------------------------------------------------------------------
        #endregion (+[override]ToString)
    }
    //-------------------------------------------------------------------------------
    #endregion (abstract class ToStringC<T>)

    /// <summary>武器専用特殊能力</summary>
    public interface IWeapon : IAbility { }
    /// <summary>ユニット専用特殊能力</summary>
    public interface IUnit : IAbility { }

    public interface Inheritable { }

    /// <summary>全ての特殊能力のベースクラス</summary>
    public interface IAbility { }

    /// <summary>LVがある</summary>
    public interface ILevel { IEnumerable<int> AllLevels(); int Level { get; } IAbility GetInstanceOfLv(int lv); }

    /// <summary>ミューテーションⅠで能力が上がる</summary>
    public interface IMutationAmplifiable { }
    /// <summary>ソールで能力が上がる</summary>
    public interface ISoulAmplifiable<S> where S : Soul { }
}

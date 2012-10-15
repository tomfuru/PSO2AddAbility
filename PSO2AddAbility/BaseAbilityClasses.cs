using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSO2AddAbility
{

    /// <summary>能力UP</summary>
    public abstract class Basic_up : IWeapon, IUnit, ILevel, Inheritable { public int Level { get; protected set; } }
    /// <summary>追加効果</summary>
    public abstract class Additional : IWeapon, ILevel, Inheritable { public int Level { get; protected set; } }
    /// <summary>○○・ブースト</summary>
    public abstract class Boost : IWeapon, IUnit { }
    /// <summary>アビリティ</summary>
    public abstract class Ability : IWeapon, IUnit, ILevel { public int Level { get; protected set; } }
    /// <summary>○○レジスト</summary>
    public abstract class Regist : IUnit, ILevel, Inheritable { public int Level { get; protected set; } }
    /// <summary>ミューテーションⅠ，スティグマ</summary>
    public abstract class Special_up : IWeapon, IUnit, Inheritable { }
    /// <summary>○○・ソール</summary>
    public abstract class Soul : IWeapon, IUnit, Inheritable { }

    /// <summary>武器専用特殊能力</summary>
    public interface IWeapon : IAbility { }
    /// <summary>ユニット専用特殊能力</summary>
    public interface IUnit : IAbility { }

    public interface Inheritable { }

    /// <summary>全ての特殊能力のベースクラス</summary>
    public interface IAbility { }

    /// <summary>LVがある</summary>
    public interface ILevel { int Level { get; } }

    /// <summary>ミューテーションⅠで能力が上がる</summary>
    public interface IMutationAmplifiable { }
    /// <summary>ソールで能力が上がる</summary>
    public interface ISoulAmplifiable<S> where S : Soul  { }
}

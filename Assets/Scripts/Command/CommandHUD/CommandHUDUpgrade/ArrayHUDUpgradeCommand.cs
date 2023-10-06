using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayHUDUpgradeCommand
{
    private static Command[] arrCmd = new Command[(int)EHUDUpgradeCommand.LENGTH];

    public static void Add(EHUDUpgradeCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EHUDUpgradeCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}

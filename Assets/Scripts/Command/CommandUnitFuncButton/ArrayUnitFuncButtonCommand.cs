using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayUnitFuncButtonCommand
{
    private static Command[] arrCmd = new Command[(int)EUnitFuncButtonCommand.LENGTH];

    public static void Add(EUnitFuncButtonCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EUnitFuncButtonCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}

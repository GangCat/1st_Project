using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayUnitActionCommand
{
    private static Command[] arrCmd = new Command[(int)EUnitActionCommand.LENGTH];

    public static void Add(EUnitActionCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EUnitActionCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayUnitButtonCommand
{
    private static Command[] arrCmd = new Command[(int)EUnitButtonCommand.LENGTH];

    public static void Add(EUnitButtonCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EUnitButtonCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}

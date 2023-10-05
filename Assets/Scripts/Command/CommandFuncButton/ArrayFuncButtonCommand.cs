using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayFuncButtonCommand
{
    private static Command[] arrCmd = new Command[(int)EFuncButtonCommand.LENGTH];

    public static void Add(EFuncButtonCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EFuncButtonCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}

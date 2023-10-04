using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayHUDCommand
{
    private static Command[] arrCmd = new Command[(int)EHUDCommand.LENGTH];

    public static void Add(EHUDCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EHUDCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}

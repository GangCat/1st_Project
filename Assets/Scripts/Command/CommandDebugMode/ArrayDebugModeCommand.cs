using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayDebugModeCommand
{
    private static Command[] arrCmd = new Command[(int)EDebugModeCommand.LENGTH];

    public static void Add(EDebugModeCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EDebugModeCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}

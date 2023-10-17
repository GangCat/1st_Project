using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayPauseCommand
{
    public static void Add(EPauseCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EPauseCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }

    private static Command[] arrCmd = new Command[(int)EPauseCommand.LENGTH];
}

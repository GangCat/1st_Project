using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayPauseCommand
{
    public static void Add(EPauseCOmmand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EPauseCOmmand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }

    private static Command[] arrCmd = new Command[(int)EPauseCOmmand.LENGTH];
}

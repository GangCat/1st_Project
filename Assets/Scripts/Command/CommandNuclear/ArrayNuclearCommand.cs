using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayNuclearCommand
{
    private static Command[] arrCmd = new Command[(int)ENuclearCommand.LENGTH];

    public static void Add(ENuclearCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(ENuclearCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}

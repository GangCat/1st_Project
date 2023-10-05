using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayCameraMoveCommand
{
    private static Command[] arrCmd = new Command[(int)ECameraCommand.LENGTH];

    public static void Add(ECameraCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(ECameraCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}

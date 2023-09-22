using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayBunkerCommand
{
    private static Command[] arrCmd = new Command[(int)EBunkerCommand.LENGTH];

    public static void Add(EBunkerCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EBunkerCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}

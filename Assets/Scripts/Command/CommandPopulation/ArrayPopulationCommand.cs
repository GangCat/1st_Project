using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayPopulationCommand
{
    private static Command[] arrCmd = new Command[(int)EPopulationCommand.LENGTH];

    public static void Add(EPopulationCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EPopulationCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}

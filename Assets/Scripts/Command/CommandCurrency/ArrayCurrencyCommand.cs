using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayCurrencyCommand
{
    private static Command[] arrCmd = new Command[(int)ECurrencyCommand.LENGTH];

    public static void Add(ECurrencyCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(ECurrencyCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}

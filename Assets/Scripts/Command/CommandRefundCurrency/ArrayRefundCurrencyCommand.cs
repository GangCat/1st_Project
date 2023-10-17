using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayRefundCurrencyCommand
{
    private static Command[] arrCmd = new Command[(int)ERefuncCurrencyCommand.LENGTH];

    public static void Add(ERefuncCurrencyCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(ERefuncCurrencyCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}

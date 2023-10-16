using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrayChangeHotkeyCommand
{
    private static Command[] arrCmd = new Command[(int)EChangeHotkeyCommand.LENGTH];

    public static void Add(EChangeHotkeyCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EChangeHotkeyCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}

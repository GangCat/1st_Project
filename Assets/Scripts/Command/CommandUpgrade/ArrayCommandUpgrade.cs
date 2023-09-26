using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ArrayCommandUpgrade
{
    private static Command[] arrCmd = new Command[(int)EUpgradeCommand.LENGTH];

    public static void Add(EUpgradeCommand _eCmd, Command _cmd)
    {
        arrCmd[(int)_eCmd] = _cmd;
    }

    public static void Use(EUpgradeCommand _eCmd, params object[] _objects)
    {
        arrCmd[(int)_eCmd].Execute(_objects);
    }
}

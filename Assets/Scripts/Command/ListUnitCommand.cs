using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListUnitCommand
{
    public static List<Command> listUnitCmd = new List<Command>();

    public static void Add(Command _cmd)
    {
        listUnitCmd.Add(_cmd);
    }

    public static void Remove(Command _cmd)
    {
        listUnitCmd.Remove(_cmd);
    }

    public static void Use(int _idx)
    {
        listUnitCmd[_idx].Execute();
    }
}

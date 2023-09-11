using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListUnitButtonCommand
{
    public static List<Command> listUnitBtnCmd = new List<Command>();

    public static void Add(Command _cmd)
    {
        listUnitBtnCmd.Add(_cmd);
    }

    public static void Remove(Command _cmd)
    {
        listUnitBtnCmd.Remove(_cmd);
    }

    public static void Use(int _idx)
    {
        listUnitBtnCmd[_idx].Execute();
    }
}

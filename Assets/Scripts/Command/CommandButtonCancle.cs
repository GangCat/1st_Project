using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandButtonCancle : Command
{
    public CommandButtonCancle() { }

    public override void Execute(params object[] _objects)
    {
        Debug.Log("Cancle");
    }
}

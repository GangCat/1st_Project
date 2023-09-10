using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandCancle : Command
{
    public CommandCancle() { }

    public override void Execute()
    {
        Debug.Log("Cancle");
    }
}

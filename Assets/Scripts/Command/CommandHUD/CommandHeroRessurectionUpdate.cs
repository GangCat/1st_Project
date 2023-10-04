using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHeroRessurectionUpdate : Command
{
    public CommandHeroRessurectionUpdate(CanvasHeroRessurection _canvas)
    {
        canvasHero = _canvas;
    }
    public override void Execute(params object[] _objects)
    {
        canvasHero.UpdateTimer((float)_objects[0]);
    }

    private CanvasHeroRessurection canvasHero = null;
}

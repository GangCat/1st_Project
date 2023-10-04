using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandHeroRessurectionFinish : Command
{
    public CommandHeroRessurectionFinish(CanvasHeroRessurection _canvas)
    {
        canvasHero = _canvas;
    }

    public override void Execute(params object[] _objects)
    {
        canvasHero.SetActive(false);
    }

    private CanvasHeroRessurection canvasHero = null;
}

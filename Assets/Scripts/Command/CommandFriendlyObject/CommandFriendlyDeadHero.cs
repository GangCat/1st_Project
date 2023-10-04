using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandFriendlyDeadHero : Command
{
    public CommandFriendlyDeadHero(HeroUnitManager _heroMng, UIManager _uiMng, SelectableObjectManager _selMng)
    {
        selMng = _selMng;
        heroMng = _heroMng;
        uiMng = _uiMng;
    }

    public override void Execute(params object[] _objects)
    {
        selMng.RemoveUnitAtList((FriendlyObject)_objects[0]);
        heroMng.Dead();
        uiMng.HeroDead();
    }

    private SelectableObjectManager selMng = null;
    private HeroUnitManager heroMng = null;
    private UIManager uiMng = null;
}

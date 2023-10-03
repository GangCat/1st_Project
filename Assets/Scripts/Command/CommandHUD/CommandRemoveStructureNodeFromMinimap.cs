using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandRemoveStructureNodeFromMinimap : Command
{
    public CommandRemoveStructureNodeFromMinimap(ImageMinimap _imageMinimap)
    {
        imageMinimap = _imageMinimap;
    }

    public override void Execute(params object[] _objects)
    {
        for(int i = 0; i < _objects.Length; ++i)
            imageMinimap.RemoveStructureNodeFromMinimap((PF_Node)_objects[i]);
    }

    private ImageMinimap imageMinimap = null;
}

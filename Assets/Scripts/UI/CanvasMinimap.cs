using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMinimap : MonoBehaviour
{
    public void Init()
    {
        imageMinimap.Init();
        ArrayHUDCommand.Add(EHUDCommand.ADD_STRUCTURE_NODE_TO_MINIMAP, new CommandAddStructureNodeToMinimap(imageMinimap));
        ArrayHUDCommand.Add(EHUDCommand.REMOVE_STRUCTURE_NODE_FROM_MINIMAP, new CommandRemoveStructureNodeFromMinimap(imageMinimap));
    }


    [SerializeField]
    private ImageMinimap imageMinimap = null;
}

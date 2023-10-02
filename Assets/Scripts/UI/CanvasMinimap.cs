using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasMinimap : MonoBehaviour
{
    public void Init()
    {
        imageMinimap.Init();
    }

    public void AddStructureNodeToMinimap(PF_Node _node)
    {
        imageMinimap.AddStructureNodeToMinimap(_node);
    }

    public void RemoveStructureNodeFromMinimap(PF_Node _node)
    {
        imageMinimap.RemoveStructureNodeFromMinimap(_node);
    }



    [SerializeField]
    private ImageMinimap imageMinimap = null;
}

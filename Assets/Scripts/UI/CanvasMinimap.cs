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

    public void UpdateMinimap(EObjectType _type, PF_Node _prevNode, PF_Node _curNode)
    {
        imageMinimap.UpdateNode();
        Debug.Log(_type);
    }



    [SerializeField]
    private ImageMinimap imageMinimap = null;
}

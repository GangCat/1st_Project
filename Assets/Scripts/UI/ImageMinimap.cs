using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ImageMinimap : MonoBehaviour
{
    public void Init()
    {
        imageMinimap = GetComponent<Image>();
        tex2d = new Texture2D(256, 256);
        //tex2d.name = "Set Pixel";
        texW = tex2d.width;
        texH = tex2d.height;

        texRect = new Rect(0, 0, texW, texH);
        pivotVec = new Vector2(0.5f, 0.5f);

        listStructureNode = new List<PF_Node>();

        StartCoroutine("UpdateMinimap");
    }

    public void AddStructureNodeToMinimap(PF_Node _node)
    {
        listStructureNode.Add(_node);
    }

    public void RemoveStructureNodeFromMinimap(PF_Node _node)
    {
        listStructureNode.Remove(_node);
    }

    private IEnumerator UpdateMinimap()
    {
        while (true)
        {
            UpdateTexture(ref tex2d);
            imageMinimap.sprite = Sprite.Create(tex2d, texRect, pivotVec);
            yield return new WaitForSeconds(1f);
        }
    }

    private void UpdateTexture(ref Texture2D tex2d)
    {
        int idx = 0;

        while (idx < texW * texH)
        {
            tex2d.SetPixel(idx % texW, idx / texH, Color.black);
            ++idx;
        }

        PF_Node tempNode = null;


        foreach (PF_Node node in SelectableObjectManager.DicNodeUnderFriendlyUnit.Values)
            tex2d.SetPixel(node.gridX, node.gridY, Color.green);

        foreach (PF_Node node in SelectableObjectManager.DicNodeUnderEnemyUnit.Values)
            tex2d.SetPixel(node.gridX, node.gridY, Color.red);

        //for (int i = 0; i < SelectableObjectManager.DicNodeUnderFriendlyUnit.Count; ++i)
        //{
        //    tempNode = null;

        //    tempNode = SelectableObjectManager.DicNodeUnderFriendlyUnit[i];
        //    if (tempNode != null)
        //        tex2d.SetPixel(tempNode.gridX, tempNode.gridY, Color.green);
        //}

        //for (int i = 0; i < SelectableObjectManager.DicNodeUnderEnemyUnit.Count; ++i)
        //{
        //    tempNode = null;
        //    tempNode = SelectableObjectManager.DicNodeUnderEnemyUnit[i];
        //    if (tempNode != null)
        //        tex2d.SetPixel(tempNode.gridX, tempNode.gridY, Color.red);
        //}

        for (int i = 0; i < listStructureNode.Count; ++i)
        {
            tempNode = listStructureNode[i];
            tex2d.SetPixel(tempNode.gridX, tempNode.gridY, Color.green);
        }

        tex2d.Apply();
    }

    private Image imageMinimap = null;
    private Texture2D tex2d = null;
    private Rect texRect;
    private Vector2 pivotVec;
    private List<PF_Node> listStructureNode = null;

    private int texH = 0;
    private int texW = 0;
}

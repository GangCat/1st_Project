using System.Collections;
using System.Collections.Generic;
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

        UpdateNode();
        StartCoroutine("UpdateMinimap");
    }

    public void UpdateNode()
    {
        UpdateTexture(ref tex2d);
        imageMinimap.sprite = Sprite.Create(tex2d, texRect, pivotVec);
    }

    private IEnumerator UpdateMinimap()
    {
        while (true)
        {
            UpdateTexture(ref tex2d);
            imageMinimap.sprite = Sprite.Create(tex2d, texRect, pivotVec);
            yield return new WaitForSeconds(0.1f);
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

        for (int i = 0; i < SelectableObjectManager.DicNodeUnderFriendlyUnit.Count; ++i)
        {
            tempNode = null;
            SelectableObjectManager.DicNodeUnderFriendlyUnit.TryGetValue(i, out tempNode);
            if(tempNode != null)
                tex2d.SetPixel(tempNode.gridX, tempNode.gridY, Color.green);
        }

        for (int i = 0; i < SelectableObjectManager.DicNodeUnderEnemyUnit.Count; ++i)
        {
            tempNode = null;
            SelectableObjectManager.DicNodeUnderEnemyUnit.TryGetValue(i, out tempNode);
            if (tempNode != null)
                tex2d.SetPixel(tempNode.gridX, tempNode.gridY, Color.red);
        }

        tex2d.Apply();
    }

    private Image imageMinimap = null;
    private Texture2D tex2d = null;
    private Rect texRect;
    private Vector2 pivotVec;

    private int texH = 0;
    private int texW = 0;
}

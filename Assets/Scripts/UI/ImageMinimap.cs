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
        tex2d.name = "Set Pixel";
        texW = tex2d.width;
        texH = tex2d.height;

        texRect = new Rect(0, 0, texW, texH);
        pivotVec = new Vector2(0.5f, 0.5f);

        StartCoroutine("UpdateMinimap");
    }

    private IEnumerator UpdateMinimap()
    {
        while (true)
        {
            UpdateTexture();
            Sprite sprite = Sprite.Create(tex2d, texRect, pivotVec);
            imageMinimap.sprite = sprite;
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void UpdateTexture()
    {
        int idx = 0;

        while (idx < texW * texH)
            tex2d.SetPixel(idx % texW, idx / texH, Color.black);

        tex2d.SetPixel(Random.Range(0, texW), Random.Range(0, texH), Color.white);
        tex2d.Apply();
    }

    private Vector2 pivotVec;
    private Rect texRect;
    private Texture2D tex2d = null;
    private Image imageMinimap = null;

    private int texH = 0;
    private int texW = 0;
}

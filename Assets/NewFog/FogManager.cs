using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FogManager : MonoBehaviour
{
    private void Start()
    {
        //curFogTexture = GenerateTexture(fogRenderTexture);

        Shader.SetGlobalVectorArray("_UnitPositions", myArray);
        StartCoroutine("UpdateFogCoroutine");




        //backBuffRenderTexture = RenderTexture.active;
        //curFogTexture = GenerateTexture(backBuffRenderTexture);

        //UpdateProjectorMaterial();
        //UpdateFog();
    }

    private IEnumerator UpdateFogCoroutine()
    {
        while (true)
        {
            fogGenMaterial.SetTexture("_MainTex", tex);
            yield return null;
            Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
            sprite.name = "P";
            fogImage.sprite = sprite;
            yield return new WaitForSeconds(updateFogDelay);
        }
    }

    private void UpdateFog()
    {
        fogGenMaterial.SetTexture("_MainTex", tex);
        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        sprite.name = "P";
        fogImage.sprite = sprite;

        Invoke("UpdateFog", updateFogDelay);
    }


    private Texture2D GenerateTexture(RenderTexture _texture)
    {
        Texture2D newTexture = new Texture2D(_texture.width, _texture.height, TextureFormat.RGBA32, false);

        RenderTexture.active = _texture;
        newTexture.ReadPixels(new Rect(0, 0, _texture.width, _texture.height), 0, 0);
        newTexture.Apply();
        RenderTexture.active = null;
        return newTexture;
    }

    private void UpdateProjectorMaterial()
    {

    }

    [SerializeField]
    private float updateFogDelay = 0f;
    [SerializeField]
    private RenderTexture fogRenderTexture = null;
    [SerializeField]
    private RenderTexture backBuffRenderTexture = null;
    [SerializeField]
    private Material fogGenMaterial = null;
    [SerializeField]
    private Image fogImage = null;
    [SerializeField]
    private Vector4[] myArray = null;
    [SerializeField]
    private Texture2D tex = null;

    private Texture2D curFogTexture = null;
    private Texture2D backBufftexture = null;

}

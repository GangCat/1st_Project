using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FogManager : MonoBehaviour
{
    private void Start()
    {
        curFogTexture = GenerateTexture(fogRenderTexture);
        backBufftexture = GenerateTexture(fogRenderTexture);

        Shader.SetGlobalVectorArray("_UnitPositions", myArray);


        newFogRenderTexture = new RenderTexture(fogRenderTexture.width, fogRenderTexture.height, 0, RenderTextureFormat.ARGB32);
        newFogRenderTexture.enableRandomWrite = true;
        newFogRenderTexture.Create();
        newBackBuffRenderTexture = new RenderTexture(fogRenderTexture.width, fogRenderTexture.height, 0, RenderTextureFormat.ARGB32);
        newBackBuffRenderTexture.enableRandomWrite = true;
        newBackBuffRenderTexture.Create();

        fogComputeShader.SetTexture(0, "fogRenderTexture", newFogRenderTexture);
        fogComputeShader.SetTexture(0, "backBuffRenderTexture", newBackBuffRenderTexture);
        
        UpdateFogTexture();
    }

    private void UpdateFogTexture()
    {
        mainCam.RenderFog();

        Graphics.CopyTexture(fogRenderTexture, newFogRenderTexture);
        int threadGroupsX = Mathf.CeilToInt(newFogRenderTexture.width / 8f);
        int threadGroupsY = Mathf.CeilToInt(newFogRenderTexture.height / 8f);
        fogComputeShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        Graphics.CopyTexture(newFogRenderTexture, curFogTexture);
        Graphics.CopyTexture(newBackBuffRenderTexture, backBufftexture);

        fogGo.GetComponent<MeshRenderer>().material.SetTexture("_BackBufferTexture", backBufftexture);
        mapGo.GetComponent<MeshRenderer>().material.SetTexture("_FogTexture", curFogTexture);

        Sprite spriteFog = Sprite.Create(curFogTexture, new Rect(0, 0, curFogTexture.width, curFogTexture.height), new Vector2(0.5f, 0.5f));
        spriteFog.name = "Fog";
        fogImage.sprite = spriteFog;

        Sprite spriteBuffer = Sprite.Create(backBufftexture, new Rect(0, 0, backBufftexture.width, backBufftexture.height), new Vector2(0.5f, 0.5f));
        spriteBuffer.name = "Buffer";
        bufferImage.sprite = spriteBuffer;

        Invoke("UpdateFogTexture", updateFogDelay);
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

    [SerializeField]
    private float updateFogDelay = 0f;
    [SerializeField]
    private RenderTexture fogRenderTexture = null;
    [SerializeField]
    private RenderTexture backBuffRenderTexture = null;
    [SerializeField]
    private Image fogImage = null;
    [SerializeField]
    private Image bufferImage = null;
    [SerializeField]
    private Vector4[] myArray = null;
    [SerializeField]
    private Texture2D tex = null;
    [SerializeField]
    private ComputeShader fogComputeShader = null;
    [SerializeField]
    private Material fogMat = null;
    [SerializeField]
    private Material mapMat = null;
    [SerializeField]
    private GameObject fogGo = null;
    [SerializeField]
    private GameObject mapGo = null;
    [SerializeField]
    private TempCamera mainCam = null;

    private Texture2D curFogTexture = null;
    private Texture2D backBufftexture = null;

    private RenderTexture newFogRenderTexture = null;
    private RenderTexture newBackBuffRenderTexture = null;
}

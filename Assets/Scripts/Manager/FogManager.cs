using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FogManager : MonoBehaviour
{
    public void Init()
    {
        curFogTexture = GenerateTexture(fogRenderTexture);
        backBufftexture = GenerateTexture(fogRenderTexture);

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
        // fogRenderTexture ����
        mainCam.RenderFog();
        // ���̴��� �����迭�� ������ ��ġ, �ǹ��� ��ġ�� �迭�� ����.


        // �ش� ���� newFogRenderTexture�� ����
        Graphics.CopyTexture(fogRenderTexture, newFogRenderTexture);

        // newBackBufferRenderTexture�� newFogRenderTexture�� ���� �����
        int threadGroupsX = Mathf.CeilToInt(newFogRenderTexture.width / 8f);
        int threadGroupsY = Mathf.CeilToInt(newFogRenderTexture.height / 8f);
        fogComputeShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        // ������ �����ؽ����� ���� �ؽ��Ŀ� ����
        Graphics.CopyTexture(newFogRenderTexture, curFogTexture);
        Graphics.CopyTexture(newBackBuffRenderTexture, backBufftexture);

        // �� ������Ʈ�� ���׸��� �ؽ��� ����
        combineGo.GetComponent<MeshRenderer>().material.SetTexture("_FogTexture", curFogTexture);
        combineGo.GetComponent<MeshRenderer>().material.SetTexture("_BackBufferTexture", backBufftexture);
        combineGo.GetComponent<MeshRenderer>().material.SetTexture("_MapTexture", mapRenderTexture);

        // ������� ���� �̹����� ��ȯ�� ǥ��
        Sprite spriteFog = Sprite.Create(curFogTexture, new Rect(0, 0, curFogTexture.width, curFogTexture.height), new Vector2(0.5f, 0.5f));
        spriteFog.name = "Fog";
        fogImage.sprite = spriteFog;
        Sprite spriteBuffer = Sprite.Create(backBufftexture, new Rect(0, 0, backBufftexture.width, backBufftexture.height), new Vector2(0.5f, 0.5f));
        spriteBuffer.name = "Buffer";
        bufferImage.sprite = spriteBuffer;

        // �ݺ�
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
    private RenderTexture mapRenderTexture = null;
    [SerializeField]
    private Image fogImage = null;
    [SerializeField]
    private Image bufferImage = null;
    [SerializeField]
    private ComputeShader fogComputeShader = null;
    [SerializeField]
    private GameObject combineGo = null;
    [SerializeField]
    private CameraMovement mainCam = null;

    private Texture2D curFogTexture = null;
    private Texture2D backBufftexture = null;

    private RenderTexture newFogRenderTexture = null;
    private RenderTexture newBackBuffRenderTexture = null;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FogManager : MonoBehaviour, IPauseObserver
{
    public void Init()
    {
        ArrayPauseCommand.Use(EPauseCOmmand.REGIST, this);
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
        if (isPause)
        {
            Invoke("UpdateFogTexture", updateFogDelay);
            return;
        }

        // fogRenderTexture 갱신
        mainCam.RenderFog();
        // 쉐이더의 전역배열에 유닛의 위치, 건물의 위치를 배열로 보냄.


        // 해당 내용 newFogRenderTexture에 복사
        Graphics.CopyTexture(fogRenderTexture, newFogRenderTexture);

        // newBackBufferRenderTexture에 newFogRenderTexture의 내용 덮어쓰기
        int threadGroupsX = Mathf.CeilToInt(newFogRenderTexture.width / 8f);
        int threadGroupsY = Mathf.CeilToInt(newFogRenderTexture.height / 8f);
        fogComputeShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

        // 각각의 렌더텍스쳐의 내용 텍스쳐에 복사
        Graphics.CopyTexture(newFogRenderTexture, curFogTexture);
        Graphics.CopyTexture(newBackBuffRenderTexture, backBufftexture);

        // 각 오브젝트의 머테리얼에 텍스쳐 갱신
        combineGo.GetComponent<MeshRenderer>().material.SetTexture("_FogTexture", curFogTexture);
        combineGo.GetComponent<MeshRenderer>().material.SetTexture("_BackBufferTexture", backBufftexture);
        combineGo.GetComponent<MeshRenderer>().material.SetTexture("_MapTexture", mapRenderTexture);

        // 디버깅을 위해 이미지로 변환해 표시
        if (isDebugMode)
        {
        Sprite spriteFog = Sprite.Create(curFogTexture, new Rect(0, 0, curFogTexture.width, curFogTexture.height), new Vector2(0.5f, 0.5f));
        spriteFog.name = "Fog";
        fogImage.sprite = spriteFog;
        Sprite spriteBuffer = Sprite.Create(backBufftexture, new Rect(0, 0, backBufftexture.width, backBufftexture.height), new Vector2(0.5f, 0.5f));
        spriteBuffer.name = "Buffer";
        bufferImage.sprite = spriteBuffer;
        }

        // 반복
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

    public void CheckPause(bool _isPause)
    {
        isPause = _isPause;
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
    [SerializeField]
    private bool isDebugMode = false;

    private Texture2D curFogTexture = null;
    private Texture2D backBufftexture = null;

    private RenderTexture newFogRenderTexture = null;
    private RenderTexture newBackBuffRenderTexture = null;
    private bool isPause = false;
}

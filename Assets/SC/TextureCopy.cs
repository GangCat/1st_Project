using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class TextureCopy : MonoBehaviour
{
    private void Awake()
    {
        projector = GetComponent<DecalProjector>(); // 현재 객체에 부착된 프로젝터 컴포넌트를 가져옴
        projector.enabled = true; // 프로젝터 활성화

        
    }

    void Start()
    {

        // 렌더 텍스처와 같은 크기의 백 버퍼 텍스처를 생성합니다.
        //backBuffer = new RenderTexture(renderTexture.width, renderTexture.height, 0, RenderTextureFormat.ARGBFloat);

        t1 = GenerateRenderTexture();
        t2 = GenerateRenderTexture();

        projector.material.SetTexture("_FogTexture", t1);
        projector.material.SetTexture("_BackBufferTexture", t2);

        ClearRenderTarget(backBuffer, Color.black);
        backBuffer.enableRandomWrite = true;
        backBuffer.Create();

        // CopyTexture 커널을 실행하여 알파값이 0보다 큰 픽셀만 복사합니다.
        kernel = copyShader.FindKernel("CopyTexture");
        copyShader.SetTexture(kernel, "_InputTex", renderTexture);
        copyShader.SetTexture(kernel, "_BackBuffer", backBuffer);
        UpdateTexture();

    }


    RenderTexture GenerateRenderTexture()
    {
        RenderTexture rt = new RenderTexture(
            renderTexture.width,
            renderTexture.height,
            0,
            renderTexture.format)
        { filterMode = FilterMode.Bilinear };
        rt.antiAliasing = renderTexture.antiAliasing;

        return rt;
    }

    void UpdateTexture()
    {
        copyShader.Dispatch(kernel, Mathf.CeilToInt(renderTexture.width / 8f), Mathf.CeilToInt(renderTexture.height / 8f), 1);

        Graphics.Blit(t1, renderTexture);
        Graphics.Blit(t2, backBuffer);

        Invoke("UpdateTexture", UpdateFogDelay);
    }

    void ClearRenderTarget(RenderTexture targetTexture, Color clearColor)
    {
        // 현재 렌더 텍스처를 활성화합니다.
        RenderTexture.active = targetTexture;

        // 렌더 텍스처를 지정한 색상으로 초기화합니다.
        GL.Clear(true, true, clearColor);

        // 렌더 텍스처의 활성화를 해제합니다.
        RenderTexture.active = null;
    }

    public RenderTexture renderTexture; // 렌더 텍스처를 Inspector에서 할당해야 합니다.
    public RenderTexture backBuffer;
    public ComputeShader copyShader; // CopyTexture 컴퓨트 쉐이더
    int kernel;
    private DecalProjector projector; // 프로젝터 컴포넌트
    [SerializeField]
    private float UpdateFogDelay = 0.1f;
    //private RenderTexture backBuffer;

    private RenderTexture t1 = null;
    private RenderTexture t2 = null;
}

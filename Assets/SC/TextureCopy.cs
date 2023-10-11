using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class TextureCopy : MonoBehaviour
{
    private void Awake()
    {
        projector = GetComponent<DecalProjector>(); // ���� ��ü�� ������ �������� ������Ʈ�� ������
        projector.enabled = true; // �������� Ȱ��ȭ

        
    }

    void Start()
    {

        // ���� �ؽ�ó�� ���� ũ���� �� ���� �ؽ�ó�� �����մϴ�.
        //backBuffer = new RenderTexture(renderTexture.width, renderTexture.height, 0, RenderTextureFormat.ARGBFloat);

        t1 = GenerateRenderTexture();
        t2 = GenerateRenderTexture();

        projector.material.SetTexture("_FogTexture", t1);
        projector.material.SetTexture("_BackBufferTexture", t2);

        ClearRenderTarget(backBuffer, Color.black);
        backBuffer.enableRandomWrite = true;
        backBuffer.Create();

        // CopyTexture Ŀ���� �����Ͽ� ���İ��� 0���� ū �ȼ��� �����մϴ�.
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
        // ���� ���� �ؽ�ó�� Ȱ��ȭ�մϴ�.
        RenderTexture.active = targetTexture;

        // ���� �ؽ�ó�� ������ �������� �ʱ�ȭ�մϴ�.
        GL.Clear(true, true, clearColor);

        // ���� �ؽ�ó�� Ȱ��ȭ�� �����մϴ�.
        RenderTexture.active = null;
    }

    public RenderTexture renderTexture; // ���� �ؽ�ó�� Inspector���� �Ҵ��ؾ� �մϴ�.
    public RenderTexture backBuffer;
    public ComputeShader copyShader; // CopyTexture ��ǻƮ ���̴�
    int kernel;
    private DecalProjector projector; // �������� ������Ʈ
    [SerializeField]
    private float UpdateFogDelay = 0.1f;
    //private RenderTexture backBuffer;

    private RenderTexture t1 = null;
    private RenderTexture t2 = null;
}

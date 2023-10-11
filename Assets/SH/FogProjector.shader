Shader "Custom/FogProjection"
{
    Properties
    {
        _PrevTexture("Previous Texture", 2D) = "white" {}  // ���� �ؽ�ó�� ������ �Ӽ� ����
        _CurrTexture("Current Texture", 2D) = "white" {}  // ���� �ؽ�ó�� ������ �Ӽ� ����
        _MapTexture("Map Texture", 2D) = "white" {} // �� �ؽ��ĸ� ������ �Ӽ� ����
        _Color("Color", Color) = (0, 0, 0, 0)  // �ؽ�ó�� ������ ���� �Ӽ� ����
    }

        SubShader
        {
            Tags { "Queue" = "Transparent+100" }  // �׸� ���� ������ ���� ����

            Pass
            {
                ZWrite Off  // Z ���ۿ� ���⸦ �� (���� ��ü�� ���� �ʿ�)
                Blend SrcAlpha OneMinusSrcAlpha  // ���� ��ü�� ���� ����
                ZTest Equal  // Z �׽�Ʈ�� Equal�� �����Ͽ� ���̰��� ���� ���� �׸�

                CGPROGRAM  // CG ���α׷� ����

                #pragma vertex vert  // ���� ���̴� �Լ��� vert�� ����
                #pragma fragment frag  // �ȼ� ���̴� �Լ��� frag�� ����

                #include "UnityCG.cginc"  // UnityCG ���̴� ���̺귯�� ����

                struct appdata
                {
                    float4 vertex : POSITION;  // ������ ��ġ ����
                    float4 uv : TEXCOORD0;  // �ؽ�ó ��ǥ ����
                };

                struct v2f
                {
                    float4 uv : TEXCOORD0;  // �ؽ�ó ��ǥ�� ������ ����
                    float4 vertex : SV_POSITION;  // ������ ��ũ�� ��ǥ�� ������ ����
                };

                float4x4 unity_Projector;  // �������� ���
                sampler2D _CurrTexture;  // ���� �ؽ�ó�� ���� ����
                sampler2D _PrevTexture;  // ���� �ؽ�ó�� ���� ����
                sampler2D _MapTexture;
                fixed4 _Color;  // ���� ������ ���� ����

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);  // ���� ��ǥ�� Ŭ�� �������� ��ȯ
                    o.uv = mul(unity_Projector, v.vertex);  // �������� ����� ����Ͽ� �ؽ�ó ��ǥ ���
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target  // �ȼ� ���̴� �Լ�
                {
                    float aPrev = tex2Dproj(_PrevTexture, i.uv).a;  // ���� �ؽ�ó�� ���İ��� ������
                    float aCurr = tex2Dproj(_CurrTexture, i.uv).a;  // ���� �ؽ�ó�� ���İ��� ������
                    
                    fixed a = (aPrev + aCurr) / 2;

                    // _Color�� 0,0,0,1��

                    // �ȼ��� ������ a�� 0���� ũ�� �ϴ� ���� ��������.
                    // �ٵ� �� 0.7? �̻��� ���� ���� ������ ��.
                    // �׸��� 0���� ū ��� �� �ؽ��İ� ��������.
                    // �� �� �� �ؽ����� ���İ��� ���� 0�� ���������.

                    if (a > 0.7) {
                        _Color.a = max(0, _Color.a - a);
                        return _Color;  // ���� ���� ��ȯ
                    }


                    // �ϴ� �̰� ����.
                    // �ٵ� ���� ��ο��� �ϴµ� �ʹ� ����.
                    // ������ mapColor�� a�� �����ϸ� ���� ���� ������ a�� �׻� 1�̾����.
                    // ��� ���� a�� ���� ��ŭ ��ο����� ��.
                    // �׷� rgb�� ��� ���߸� �ǳ�?

                    fixed4 mapColor = tex2D(_MapTexture, i.uv);
                    mapColor.r *= a;
                    mapColor.g *= a;
                    mapColor.b *= a;
                    return mapColor;

                    // mapColor���ٰ� ��ٷ� a�� ���ϸ� alpha�� �������� ���� ����.
                    // �׷��� �� �׸񿡸� ���ϰ� �� ���� ��ȯ�ϰ� ��.
                    

                    //fixed4 mapColor = tex2D(_MapTexture, i.uv);
                    //mapColor.a = max(0, _Color.a - a);
                    //return mapColor;

                    // ������ ���� ���� ����ϰ� ������ ���� �ʵ��� ����

                }
                ENDCG  // CG ���α׷� ����
            }
        }
}

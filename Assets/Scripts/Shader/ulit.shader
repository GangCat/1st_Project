Shader "Custom/FogShader"
{
    Properties
    {
        _FogRenderTexture("Fog Render Texture", 2D) = "white" {}
        _BackBufferRenderTexture("Back Buffer Render Texture", 2D) = "white" {}
        _MapTexture("Map Texture", 2D) = "white" {}
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
    }
        SubShader
        {
            Tags { "RenderType" = "Transparent" }
            LOD 100

            ZWrite Off
            //ZTest LEqual
            Blend SrcAlpha OneMinusSrcAlpha

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #include "UnityCG.cginc"

                struct appdata
                {
                    float4 vertex : POSITION;
                };

                struct v2f
                {
                    float4 pos : POSITION;
                    float2 uv_FogRenderTexture : TEXCOORD0;
                    float2 uv_BackBufferRenderTexture : TEXCOORD1;
                    float2 uv_MapTexture : TEXCOORD2;
                };

                sampler2D _FogRenderTexture;
                sampler2D _BackBufferRenderTexture;
                sampler2D _MapTexture;
                float4 _BaseColor;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);
                    o.uv_FogRenderTexture = o.pos.xy * 0.5 + 0.5;
                    o.uv_BackBufferRenderTexture = o.pos.xy * 0.5 + 0.5;
                    o.uv_MapTexture = o.pos.xy * 0.5 + 0.5;
                    return o;
                }

                fixed4 frag(v2f i) : COLOR
                {
                    float fogAlpha = tex2D(_FogRenderTexture, i.uv_FogRenderTexture).a;
                    float backBufferAlpha = tex2D(_BackBufferRenderTexture, i.uv_BackBufferRenderTexture).a;
                    //float mapAlpha = tex2D(_MapTexture, i.uv_MapTexture).a;
                    float3 mapColor = tex2D(_MapTexture, i.uv_MapTexture).rgb;
                    float avgAlpha = (fogAlpha + backBufferAlpha) / 2.0;

                    fixed4 finalColor;
                    if (avgAlpha > 0.7)
                    {
                        finalColor = _BaseColor;
                        finalColor.a = 1.0 - avgAlpha;
                    }
                    else
                    {
                        finalColor.r = mapColor.r * avgAlpha;
                        finalColor.g = mapColor.g * avgAlpha;
                        finalColor.b = mapColor.b * avgAlpha;
                        //finalColor = mapColor * avgAlpha;
                        finalColor.a = 1.0 - avgAlpha;
                    }

                    return finalColor;
                }
                ENDCG
            }
        }
}

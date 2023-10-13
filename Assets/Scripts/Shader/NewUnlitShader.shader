Shader "Custom/NewUnlitShader"
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
        Tags { "RenderType"="Transparent" }
        LOD 100

        Pass
        {
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct v2f members uv_FogRenderTexture)
#pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag
            // make fog work

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                //UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float2 uv_FogRenderTexture;
            };

            sampler2D _FogRenderTexture;
            sampler2D _BackBufferRenderTexture;
            sampler2D _MapTexture;
            float4 _BaseColor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : COLOR
            {
                // sample the texture
                float3 col = tex2D(_FogRenderTexture, i.uv_FogRenderTexture).rgb;
                float alpha = tex2D(_FogRenderTexture, i.uv_FogRenderTexture).a;
                // apply fog
                //UNITY_APPLY_FOG(i.fogCoord, col);
                fixed4 finalColor = col;
                finalColor.a = alpha;
                return finalColor;
            }
            ENDCG
        }
    }
    Fallback "Diffuse"
}

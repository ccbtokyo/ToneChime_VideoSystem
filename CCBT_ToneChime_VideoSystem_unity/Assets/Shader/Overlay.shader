Shader "Custom/Overlay"
{
   Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _OverlayTex("Overlay Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _OverlayTex;
            float4 _MainTex_ST;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 col = tex2D(_MainTex, i.uv);
                fixed4 overlayCol = tex2D(_OverlayTex, i.uv);

                // http://www.cg-ya.net/2dcg/aboutimage/composite-is-math/
                col.rgb = col.rgb < 0.5 ? 2.0 * col.rgb * overlayCol.rgb : 1.0 - 2.0 * (1.0 - col.rgb) * (1.0 - overlayCol.rgb);
                return col;
            }
            ENDCG
        }
    }
}
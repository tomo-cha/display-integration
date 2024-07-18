Shader "Unlit/Grid"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Unit ("Unit", float) = 1.0
        _Color("Color", Color) = (0.5, 0.5, 0.5, 1.0)

        // グリッド平面指定
        [KeywordEnum( XZ, XY, YZ )] _Plane ( "Grid Plane", int ) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // 平面ごとにシェーダバリアントを作成
            #pragma multi_compile_local _PLANE_XZ _PLANE_XY _PLANE_YZ

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
            float4 _MainTex_ST;
            float _Unit;
            float4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                // UVはワールド座標ベースで
                float4 posW = mul(unity_ObjectToWorld, v.vertex);
                #ifdef _PLANE_XZ
                    o.uv = posW.xz / _Unit;
                #elif _PLANE_XY
                    o.uv = posW.xy / _Unit;
                #else
                    o.uv = posW.yz / _Unit;
                #endif

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                return col * _Color;
            }
            ENDCG
        }
    }
}

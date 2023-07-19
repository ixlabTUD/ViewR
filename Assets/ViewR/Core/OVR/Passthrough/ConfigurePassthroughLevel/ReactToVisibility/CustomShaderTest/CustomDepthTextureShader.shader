// Mostly from https://forum.unity.com/threads/depth-only-shader.590620/#post-3946669
// And https://forum.unity.com/threads/writing-to-depth-buffer-with-custom-shader.217814/

Shader "Custom/Depth Texture Only"
{
    Properties
    {
        _MainTex ("Trans (A)", 2D) = "white" {}
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }

    SubShader
    {
        Tags
        {
            "Queue"="AlphaTest" "IgnoreProjector"="True" "RenderType"="TransparentCutout"
        }

        Pass
        {
            Tags
            {
                "LightMode" = "Always"
            }
            ColorMask 0

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float4 vert() : SV_POSITION
            {
                return float4(0, 0, 0, 1);
            }

            void frag()
            {
            }
            ENDCG
        }

        Pass
        {
            Tags
            {
                "LightMode" = "ShadowCaster"
            }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_shadowcaster
            #include "UnityCG.cginc"

            struct v2f
            {
                V2F_SHADOW_CASTER;
                float2 uv : TEXCOORD1;
            };

            uniform sampler2D _MainTex;
            uniform float4 _MainTex_ST;
            uniform fixed _Cutoff;

            v2f vert(appdata_base v)
            {
                v2f o = (v2f)0;
                o.pos = float4(0, 0, 0, 1);

                #ifdef SHADOWS_DEPTH
                    // We're a camera depth pass
                    if (UNITY_MATRIX_P[3][3] == 0.0)
                    {
                        TRANSFER_SHADOW_CASTER_NORMALOFFSET(o)
                        o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
                    }
                #endif
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                fixed4 texcol = tex2D(_MainTex, i.uv);
                clip(texcol.a - _Cutoff);

                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
}
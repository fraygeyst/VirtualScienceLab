/* ScreenProximityWarningShader
 * MiddleVR
 * (c) MiddleVR
 */

Shader "MiddleVR/ScreenProximityWarningShader" 
{
    Properties
    {
        _NearDistance ("Near Distance", Float) = 0.01
        _Brightness ("Brightness", Float) = 1.0
        _MainTex ("Color Texture", 2D) = "white"
        _HeadPosition ("Head Position (world)", Vector) = (0.0, 0.0, 0.0, 1.0)
    }

    SubShader
    {
        // Transparent
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }

        Pass
        {
            // Renders over everything
            Lighting Off
            Cull Off
            ZTest Always
            ZWrite Off
            Fog { Mode Off}

            // Transparent
            Blend SrcAlpha OneMinusSrcAlpha

            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            uniform float4    _HeadPosition;
            uniform float     _NearDistance;
            uniform float     _Brightness;
            uniform sampler2D _MainTex;

            struct vertexInput
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct vertexOutput
            {
                float4 pos                     : SV_POSITION;
                float4 position_in_world_space : TEXCOORD0;
                float2 uv                      : TEXCOORD1;
            };

            float4 _MainTex_ST;

            vertexOutput vert(vertexInput input)
            {
                vertexOutput output;

                output.pos =  UnityObjectToClipPos(input.vertex);
                output.position_in_world_space = mul(unity_ObjectToWorld, input.vertex);
                output.uv = TRANSFORM_TEX (input.texcoord, _MainTex);

                return output;
            }

            float4 frag(vertexOutput input) : COLOR
            {
                float dist = distance(input.position_in_world_space, _HeadPosition);

                float distanceAlpha = 1.0 - clamp(dist / _NearDistance, 0.0, 1.0);
                distanceAlpha = pow(distanceAlpha, 0.8);

                float transparencyLimit = 0.4;
                if(distanceAlpha > transparencyLimit)
                {
                    distanceAlpha = 1.0;
                }
                else
                {
                    distanceAlpha = distanceAlpha * 1.0/transparencyLimit;
                }

                half4 textureColor = tex2D (_MainTex, input.uv);
                float4 brightness  = float4(_Brightness, _Brightness, _Brightness, 1);
                float4 alpha       = float4(1, 1, 1, distanceAlpha);

                return textureColor * brightness * alpha;
            }

            ENDCG
        }
    }
}

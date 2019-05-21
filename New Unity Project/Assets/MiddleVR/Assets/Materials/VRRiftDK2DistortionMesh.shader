/* VRRiftDK2DistortionMesh
 * MiddleVR
 * (c) MiddleVR
 * Translated to Cg by MiddleVR from the Oculus SDK.
 */
Shader "Hidden" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
	}
	SubShader {
		Pass {
			CGPROGRAM

			#pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag

			struct vertexInput
			{
				float4 pos : POSITION;
				float2 uvR : TEXCOORD0;
				float2 uvG : TEXCOORD1;
				float3 uvB : NORMAL;
			};

			struct vertexOutput
			{
				float4 pos : POSITION;
				float vignetteFactors : TEXCOORD0;
				float2 uvR : TEXCOORD1;
				float2 uvG : TEXCOORD2;
				float2 uvB : TEXCOORD3;
			};

			sampler2D _MainTex;

			float2 _ViewportOffset = float2(0, 0);
			float2 _DMScale = float2(0, 0);
			float2 _DMOffset = float2(0, 0);
			float4x4 _TimeWarpStart;
			float4x4 _TimeWarpEnd;
			
			float2 TimewarpTexCoordToWarpedPos(float2 inTexCoord, float4x4 rotMat)
			{
				float3 transformed = float3(mul(rotMat, float4(inTexCoord.xy, 1, 1)).xyz);
				float2 flattened = transformed.xy / transformed.z;
				return flattened * _DMScale + _DMOffset + _ViewportOffset;
			}

			vertexOutput vert(vertexInput vIN)
			{
				vertexOutput vOUT;
				float lerpFactor;
				
				vOUT.pos = float4(vIN.pos.xy, .5, 1.0);
				lerpFactor = vIN.pos.z;

				vOUT.vignetteFactors = vIN.uvB.z;

				float4x4 lerpedEyeRot = _TimeWarpStart + (_TimeWarpEnd - _TimeWarpStart) * lerpFactor;

				// If OpenGL is used for the rendering then we need to invert the vertical value of the UVs
				#ifdef SHADER_API_OPENGL
				vIN.uvR.y *= -1;
				vIN.uvG.y *= -1;
				vIN.uvB.y *= -1;
				#endif

				vOUT.uvR = TimewarpTexCoordToWarpedPos(vIN.uvR.xy, lerpedEyeRot);
				vOUT.uvG = TimewarpTexCoordToWarpedPos(vIN.uvG.xy, lerpedEyeRot);
				vOUT.uvB = TimewarpTexCoordToWarpedPos(vIN.uvB.xy, lerpedEyeRot);

				return vOUT;
			}

			float4 frag(vertexOutput vOUT) : COLOR
			{
				float r = tex2D(_MainTex, vOUT.uvR).r;
				float g = tex2D(_MainTex, vOUT.uvG).g;
				float b = tex2D(_MainTex, vOUT.uvB).b;

				return (vOUT.vignetteFactors * float4(r, g, b, 1));
			}

			ENDCG
		}
	}
	FallBack "Diffuse"
}

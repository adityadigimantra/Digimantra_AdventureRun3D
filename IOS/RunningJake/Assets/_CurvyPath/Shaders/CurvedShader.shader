// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Curved/CurvedShader" {
	Properties{
		_Color("Color", COLOR) = (1, 1, 1, 1)
		_Transparency("Transparency", Range(0,1)) = 1.0
		_CamColorDistModifier("Distance", Float) = 1.0
		_MainTex("Base (RGB)", 2D) = "white" {}
		_QOffset("Offset", Vector) = (0,0,0,0)
		_QOffsetOld("OldOffset", Vector) = (0,0,0,0)
		_Dist("Distance", Float) = 100.0
	}
		SubShader{

		Tags{ "Queue" = "Geometry" "RenderType" = "Opaque" }
		ZWrite on

		Pass
			{
			Tags {"LightMode" = "ForwardBase"}
				// Lighting Off

				CGPROGRAM

				#pragma vertex vert
				#pragma fragment frag
				#include "UnityCG.cginc"
				#include "Common.cginc"
				#include "Lighting.cginc"
				#include "UnityLightingCommon.cginc"
				#pragma multi_compile_fwdbase nolightmap nodirlightmap nodynlightmap novertexlight
				// shadow helper functions and macros
				#include "AutoLight.cginc"

				#pragma multi_compile DITHER_ON DITHER_OFF 
				#pragma multi_compile CURVED_ON CURVED_OFF		

				//#define GLOBAL_CURVED_ON

				half4 _Color;
				sampler2D _MainTex;
				float _Transparency;
				float _CamColorDistModifier;
				float4 _QOffset;
				float4 _QOffsetOld;
				float _Dist;
				uniform float4 _MainTex_ST;
				uniform float3 _CameraPosition;

				struct v2f {
					float4 pos : SV_POSITION;
					float2 uv : TEXCOORD0;
					float camDistance : FLOAT;
					float4 screenPos:TEXCOORD1;
					SHADOW_COORDS(3)
					fixed3 ambient : COLOR1;
					fixed3 diff : COLOR0;
				};

				v2f vert(appdata_base v)
				{
					v2f o;
					UNITY_INITIALIZE_OUTPUT(v2f, o);
					o.pos = UnityObjectToClipPos(v.vertex);

					o.uv = v.texcoord;
					half3 worldNormal = UnityObjectToWorldNormal(v.normal);
					half nl = max(0, dot(worldNormal, _WorldSpaceLightPos0.xyz));
					o.diff = nl * _LightColor0.rgb;
					o.ambient = ShadeSH9(half4(worldNormal,1));
					// compute shadows data
					TRANSFER_SHADOW(o)
					
					#ifdef GLOBAL_CURVED_ON 
						#if CURVED_ON
						float4 vp;
						float4 vPos = mul(UNITY_MATRIX_MV, v.vertex);
						float zOff = vPos.z / _Dist;
						vPos += _QOffset*zOff*zOff;
						float4 vPosOld= mul(UNITY_MATRIX_MV, v.vertex);
						vPosOld += _QOffsetOld*zOff*zOff;
						vp = lerp(vPosOld, vPos, v.vertex.z/25);
						o.pos = mul(UNITY_MATRIX_P, vp);
						#endif
					#else
					o.pos = UnityObjectToClipPos(v.vertex);
					#endif

					o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);
					
					// Calculate distance
					float dist0 = length(o.pos.xyz - _CameraPosition * float3(1.0f, 1.0f, 1.0f)) * .025f;// *0.1f;
					dist0 *= -1; dist0 += 2.0f;
					o.camDistance = pow(dist0, 2.8f)*0.2f;

					o.screenPos = ComputeScreenPos(o.pos);

					return o;
				}


				half4 frag(v2f i) : COLOR
				{
					fixed4 col = tex2D(_MainTex, i.uv)*_Color;
					// compute shadow attenuation (1.0 = fully lit, 0.0 = fully shadowed)
					fixed shadow = SHADOW_ATTENUATION(i);
					// darken light's illumination with shadow, keep ambient intact
					fixed3 lighting = i.diff * shadow + i.ambient;
					col.rgb *= lighting;
					return col;
				}
				ENDCG
			}
			UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
		}
		CustomEditor "CurvedMaterialEditor"
		FallBack "Mobile/Unlit"
}
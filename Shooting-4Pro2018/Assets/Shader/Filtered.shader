Shader "Custom/UI/Filtered"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)

		_StencilComp("Stencil Comparison", Float) = 8
		_Stencil("Stencil ID", Float) = 0
		_StencilOp("Stencil Operation", Float) = 0
		_StencilWriteMask("Stencil Write Mask", Float) = 255
		_StencilReadMask("Stencil Read Mask", Float) = 255

		_ColorMask("Color Mask", Float) = 15

		[Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip("Use Alpha Clip", Float) = 0
		_Mat0("Mat0", Vector) = (1,0,-1, -1)
		_Mat1("Mat1", Vector) = (2,0,-2, -1)
		_Mat2("Mat2", Vector) = (1,0,-1, -1)
		_Cutout("Cutout", Range(0.0,1.0)) = 0
	}

		SubShader
		{
			Tags
			{
				"Queue" = "Transparent"
				"IgnoreProjector" = "True"
				"RenderType" = "Transparent"
				"PreviewType" = "Plane"
				"CanUseSpriteAtlas" = "True"
			}

			Stencil
			{
				Ref[_Stencil]
				Comp[_StencilComp]
				Pass[_StencilOp]
				ReadMask[_StencilReadMask]
				WriteMask[_StencilWriteMask]
			}

			Cull Off
			Lighting Off
			ZWrite Off
			ZTest[unity_GUIZTestMode]
			Blend SrcAlpha OneMinusSrcAlpha
			ColorMask[_ColorMask]

			Pass
			{
				Name "Default"
			CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 2.0

				#include "UnityCG.cginc"
				#include "UnityUI.cginc"

				#pragma multi_compile __ UNITY_UI_ALPHACLIP

				struct appdata_t
				{
					float4 vertex   : POSITION;
					float4 color    : COLOR;
					float2 texcoord : TEXCOORD0;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct v2f
				{
					float4 vertex   : SV_POSITION;
					fixed4 color : COLOR;
					float2 texcoord  : TEXCOORD0;
					float4 worldPosition : TEXCOORD1;
					UNITY_VERTEX_OUTPUT_STEREO
				};

				fixed4 _Color;
				fixed4 _TextureSampleAdd;
				float4 _ClipRect;

				v2f vert(appdata_t v)
				{
					v2f OUT;
					UNITY_SETUP_INSTANCE_ID(v);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);
					OUT.worldPosition = v.vertex;
					OUT.vertex = UnityObjectToClipPos(OUT.worldPosition);

					OUT.texcoord = v.texcoord;

					OUT.color = v.color * _Color;
					return OUT;
				}

				sampler2D _MainTex;
				float4 _MainTex_TexelSize;
				float4 _Mat0;
				float4 _Mat1;
				float4 _Mat2;
				float _Cutout;
				uniform float3x3 _matrix;

				fixed4 frag(v2f IN) : SV_Target
				{
					float4 color;
					_matrix[0][0] = _Mat0[0];
					_matrix[0][1] = _Mat0[1];
					_matrix[0][2] = _Mat0[2];
					_matrix[1][0] = _Mat1[0];
					_matrix[1][1] = _Mat1[1];
					_matrix[1][2] = _Mat1[2];
					_matrix[2][0] = _Mat2[0];
					_matrix[2][1] = _Mat2[1];
					_matrix[2][2] = _Mat2[2];


					for (int u = -1; u <= 1; u++) {
						for (int v = -1; v <= 1; v++) {
						float x = clamp(IN.texcoord.x + u * _MainTex_TexelSize.x,0,1);
						float y = clamp(IN.texcoord.y + v * _MainTex_TexelSize.y,0,1);
						color += ((tex2D(_MainTex, float2(x,y)) + _TextureSampleAdd) * IN.color) * _matrix[u + 1][v + 1];
						}
					}
					float gray = color.r*0.3 + color.g*0.6 + color.b*0.1;
					if (_Cutout == 0 || gray > _Cutout)color.a = tex2D(_MainTex,IN.texcoord).a;

					color.a *= UnityGet2DClipping(IN.worldPosition.xy, _ClipRect);
					#ifdef UNITY_UI_ALPHACLIP
					clip(color.a - 0.001);
					#endif

					return color;
				}
			ENDCG
			}
		}
}
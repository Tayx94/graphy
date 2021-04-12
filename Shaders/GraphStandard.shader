Shader "Graphy/Graph Standard"
{
	Properties
	{
		[PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0

		_GoodColor("Good Color", Color) = (1,1,1,1)
		_CautionColor("Caution Color", Color) = (1,1,1,1)
		_CriticalColor("Critical Color", Color) = (1,1,1,1)

		_GoodThreshold("Good Threshold", Float) = 0.5
		_CautionThreshold("Caution Threshold", Float) = 0.25
	}

	SubShader
	{			
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		ZTest Off
		Blend One OneMinusSrcAlpha

		Pass
		{
			Name "Default"
			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
				
			#include "UnityCG.cginc"

			struct appdata_t
			{
				float4 vertex    : POSITION;
				float4 color     : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 vertex    : SV_POSITION;
				fixed4 color	 : COLOR;
				float2 texcoord  : TEXCOORD0;
				UNITY_VERTEX_OUTPUT_STEREO
			};

			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;

				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_INITIALIZE_OUTPUT(v2f, OUT);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(OUT);

				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;
			#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap(OUT.vertex);
			#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float _AlphaSplitEnabled;

			fixed4 SampleSpriteTexture(float2 uv)
			{
				fixed4 color = tex2D(_MainTex, uv);

			#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D(_AlphaTex, uv).r;
			#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}

			fixed4 _GoodColor;
			fixed4 _CautionColor;
			fixed4 _CriticalColor;

			fixed  _GoodThreshold;
			fixed  _CautionThreshold;

			uniform float Average;

			// NOTE: The size of this array can break compatibility with some older GPUs
			// If you see a pink box or that the graphs are not working, try lowering this value
			// or using the GraphMobile.shader
			uniform float GraphValues[512];

			uniform float GraphValues_Length;

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 color = IN.color;

				fixed xCoord = IN.texcoord.x;
				fixed yCoord = IN.texcoord.y;

				float graphValue = GraphValues[floor(xCoord * GraphValues_Length)];

				// Define the width of each element of the graph
				float increment = 1.0f / (GraphValues_Length - 1);
					
				// Assign the corresponding color
				if (graphValue > _GoodThreshold)
				{
					color *= _GoodColor;
				}
				else if (graphValue > _CautionThreshold)
				{
					color *= _CautionColor;
				}
				else
				{
					color *= _CriticalColor;
				}

				// Point coloring
				if (graphValue - yCoord > increment * 4)
				{
					//color.a = yCoord * graphValue * 0.3;
					color.a *= yCoord * 0.3 / graphValue;
				}

				// Set as transparent the part on top of the current point value
				if (yCoord > graphValue)
				{
					color.a = 0;
				}

				// Average white bar
				if (yCoord < Average && yCoord > Average - 0.02)
				{
					color = fixed4(1, 1, 1, 1);
				}

				// CautionColor bar
				if (yCoord < _CautionThreshold && yCoord > _CautionThreshold - 0.02)
				{
					color = _CautionColor;
				}

				// GoodColor bar
				if (yCoord < _GoodThreshold && yCoord > _GoodThreshold - 0.02)
				{
					color = _GoodColor;
				}

				// Fade the alpha of the sides of the graph
				if (xCoord < 0.03)
				{
					color.a *= 1 - (0.03 - xCoord) / 0.03;
				}
				else if (xCoord > 0.97)
				{
					color.a *= (1 - xCoord) / 0.03;
				}

				fixed4 c = SampleSpriteTexture(IN.texcoord) * color;

				c.rgb *= c.a;

				return c;
			}

			ENDCG
		}
	}
}
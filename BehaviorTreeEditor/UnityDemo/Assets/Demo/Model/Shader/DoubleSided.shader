Shader "GOD/DoubleSide" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,0.5)
		_PassThroughColor ("PassThrough Color", Color) = (0.09,0.15,0.31,0)
		_Emission ("Emmisive Color", Color) = (0,0,0,0)
		_AlphaCutoff ("AlphaCutoff", Range (0, 1)) = 0.5
		_MainTex ("Base (RGB)", 2D) = "white" { }
	}
	
	SubShader {
		Tags { "RenderType"="Opaque" }
		Pass {
			Lighting Off
			ZTest Greater
			ZWrite Off
			Color [_PassThroughColor]
		}

		Pass {
			Material {
				Diffuse [_Color]
				Ambient [_Color]
				Emission [_Emission]
			}
			Lighting On
			Cull Off
			AlphaTest Greater [_AlphaCutoff]
			SetTexture [_MainTex] {
				constantColor [_Color]
				Combine texture * primary + constant DOUBLE, texture * constant
			}
		}
	}
	
	Fallback "VertexLit"
}

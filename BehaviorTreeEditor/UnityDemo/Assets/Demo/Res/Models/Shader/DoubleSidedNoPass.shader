Shader "GOD/DoubleSideNoPass" {
	Properties {
		_Color ("Main Color", Color) = (1,1,1,0.5)
		_Emission ("Emmisive Color", Color) = (0,0,0,0)
		_AlphaCutoff ("AlphaCutoff", Range (0, 1)) = 0.5
		_MainTex ("Base (RGB)", 2D) = "white" { }
	}
	
	SubShader {
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

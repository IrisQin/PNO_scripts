Shader "Custom/AlphaIllu" {
	Properties{
		_Color("Main Color", Color) = (1,1,1,1)
		_SpecColor("Spec Color", Color) = (1,1,1,1)
		_Emission("Emmisive Color", Color) = (0,0,0,0)
		_Shininess("Shininess", Range(0.01, 1)) = 0.05
		_MainTex("Base (RGB)", 2D) = "white" { }
	}

		SubShader{ 
		Material{
		Diffuse[_Color]
		Ambient[_Color]
		Shininess[_Shininess]
		Specular[_SpecColor]
		Emission[_Emission]
	}
		Lighting On
		SeparateSpecular On

		// Set up alpha blending  
		Blend SrcAlpha OneMinusSrcAlpha

		// Front face transparency
		Pass {  
		    Cull Front  
		    SetTexture [_MainTex] {  
		        Combine Primary * Texture  
		    }  
		}  
		// Back face transparency   
		Pass{
		Cull Back
		SetTexture[_MainTex]{
		Combine Primary * Texture
	}
	}
	}
}

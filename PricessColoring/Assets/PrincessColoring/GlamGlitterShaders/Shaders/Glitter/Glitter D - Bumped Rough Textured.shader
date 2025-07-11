// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Spaventacorvi/Glitter/Glitter D - Bumped Rough Textured" {
    Properties {
        _Color ("Main color", Color) = (0.7058823,0.7058823,0.7058823,1)
        _MainTex ("Base (RGB)", 2D) = "white" {}
        wvwvww ("Shininess", Range(0, 1)) = 0.2
        vvxwwx ("Fake light", Range(0, 0.1)) = 0.05
        wxxwww ("Normalmap", 2D) = "bump" {}
        vwxwww ("Specular glitter", 2D) = "white" {}
        vxvvvw ("Specular power (0 - 5)", Range(0, 5)) = 1.5
        wxxxvv ("Specular contrast (1 - 3)", Range(1, 3)) = 1
        vvxxww ("Glitter map", 2D) = "white" {}
        vwxvww ("Glitter color", Color) = (1,1,1,1)
        vwwxwx ("Glitter power (0 - 10)", Range(0, 10)) = 2
        vxwwww ("Glitter contrast (1 - 3)", Range(1, 3)) = 1.5
        xvwvwx ("Glittery speed (0 - 1)", Range(0, 1)) = 0.5
        wvvxxw ("Glittery & mask dots scale", Range(0.1, 8)) = 2.5
        wvvxxv ("Mask", 2D) = "black" {}
        wvwwxv ("Mask adjust (0.5 - 1.5)", Range(0.5, 1.5)) = 1

			//ngocdu
			_Scale("Scale", float) = 1
		_Speed("Speed", float) = 1
		_Frequency("Frequency", float) = 1
    }
    SubShader {
        Tags {
            "RenderType"="Opaque"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D wvvxxv; uniform float4 wvvxxv_ST;
            uniform float wvwvww;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D vwxwww; uniform float4 vwxwww_ST;
            uniform float vxvvvw;
            uniform float vwwxwx;
            uniform float4 _Color;
            uniform sampler2D wxxwww; uniform float4 wxxwww_ST;
            uniform float vvxwwx;
            uniform float4 vwxvww;
            uniform float wvvxxw;
            uniform float xvwvwx;
            uniform sampler2D vvxxww; uniform float4 vvxxww_ST;
            uniform float vxwwww;
            uniform float wxxxvv;
            uniform float wvwwxv;

			//ngocdu
			float _Scale, _Speed, _Frequency;

            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
               
					//ngocdu
					//half offsetvert = ((v.vertex.x * v.vertex.x) + (v.vertex.z * v.vertex.z))
					//	half value0 = _Scale * sin(_Time.w * _Speed * _Frequency + offsetvert);
					////half value = _Scale * sin(_Time.w * _Speed + offsetvert * _Frequency);
					//v.vertex.y += value0;
				
					o.pos = UnityObjectToClipPos(v.vertex);
					UNITY_TRANSFER_FOG(o, o.pos);
					TRANSFER_VERTEX_TO_FRAGMENT(o)

                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 wxxwww_var = UnpackNormal(tex2D(wxxwww,TRANSFORM_TEX(i.uv0, wxxwww)));
                float3 normalLocal = wxxwww_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform ));
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float gloss = wvwvww;
                float specPow = exp2( gloss * 10.0+1.0);
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float xvxwww = 0.0;
                float2 vwwvxw = ((0.05*(xvwvwx - xvxwww)*mul(tangentTransform, viewDirection).xy + i.uv0).rg*((xvwvwx/2.0)+1.0)*wvvxxw);
                float4 wwvxvx = tex2D(vvxxww,TRANSFORM_TEX(vwwvxw, vvxxww));
                float vxxxwx = 0.0;
                float vwvxwv_ang = 3.14;
                float vwvxwv_spd = 1.0;
                float vwvxwv_cos = cos(vwvxwv_spd*vwvxwv_ang);
                float vwvxwv_sin = sin(vwvxwv_spd*vwvxwv_ang);
                float2 vwvxwv_piv = float2(0.5,0.5);
                float2 vwvxwv = (mul((0.05*((-1*xvwvwx) - xvxwww)*mul(tangentTransform, viewDirection).xy + i.uv0).rg-vwvxwv_piv,float2x2( vwvxwv_cos, -vwvxwv_sin, vwvxwv_sin, vwvxwv_cos))+vwvxwv_piv);
                float2 wvxwxw = (vwvxwv*wvvxxw*(1.0-(xvwvwx/3.141592654))*wvwwxv);
                float4 xwvwvw = tex2D(vvxxww,TRANSFORM_TEX(wvxwxw, vvxxww));
                float4 wvvxxv_var = tex2D(wvvxxv,TRANSFORM_TEX(i.uv0, wvvxxv));
                float4 vwxwww_var = tex2D(vwxwww,TRANSFORM_TEX(i.uv0, vwxwww));
                float3 wxvxxw = (lerp(pow(((vwwxwx*vwxvww.rgb)*wwvxvx.rgb),vxwwww),float3(vxxxwx,vxxxwx,vxxxwx),max((1.0 - xwvwvw.rgb),wvvxxv_var.rgb))+lerp(pow((vwxwww_var.rgb*vxvvvw),wxxxvv),float3(vxxxwx,vxxxwx,vxxxwx),wvvxxv_var.rgb));
                float3 specularColor = wxvxxw;
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb;
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 diffuseColor = (_MainTex_var.rgb*_Color.rgb);
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
                float3 node_3882 = (wxvxxw*vvxwwx);
                float3 emissive = node_3882;
                float3 finalColor = diffuse + specular + emissive;
                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDADD
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D wvvxxv; uniform float4 wvvxxv_ST;
            uniform float wvwvww;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D vwxwww; uniform float4 vwxwww_ST;
            uniform float vxvvvw;
            uniform float vwwxwx;
            uniform float4 _Color;
            uniform sampler2D wxxwww; uniform float4 wxxwww_ST;
            uniform float vvxwwx;
            uniform float4 vwxvww;
            uniform float wvvxxw;
            uniform float xvwvwx;
            uniform sampler2D vvxxww; uniform float4 vvxxww_ST;
            uniform float vxwwww;
            uniform float wxxxvv;
            uniform float wvwwxv;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float3 tangentDir : TEXCOORD3;
                float3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, float4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3x3 tangentTransform = float3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 wxxwww_var = UnpackNormal(tex2D(wxxwww,TRANSFORM_TEX(i.uv0, wxxwww)));
                float3 normalLocal = wxxwww_var.rgb;
                float3 normalDirection = normalize(mul( normalLocal, tangentTransform ));
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float gloss = wvwvww;
                float specPow = exp2( gloss * 10.0+1.0);
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float xvxwww = 0.0;
                float2 vwwvxw = ((0.05*(xvwvwx - xvxwww)*mul(tangentTransform, viewDirection).xy + i.uv0).rg*((xvwvwx/2.0)+1.0)*wvvxxw);
                float4 wwvxvx = tex2D(vvxxww,TRANSFORM_TEX(vwwvxw, vvxxww));
                float vxxxwx = 0.0;
                float vwvxwv_ang = 3.14;
                float vwvxwv_spd = 1.0;
                float vwvxwv_cos = cos(vwvxwv_spd*vwvxwv_ang);
                float vwvxwv_sin = sin(vwvxwv_spd*vwvxwv_ang);
                float2 vwvxwv_piv = float2(0.5,0.5);
                float2 vwvxwv = (mul((0.05*((-1*xvwvwx) - xvxwww)*mul(tangentTransform, viewDirection).xy + i.uv0).rg-vwvxwv_piv,float2x2( vwvxwv_cos, -vwvxwv_sin, vwvxwv_sin, vwvxwv_cos))+vwvxwv_piv);
                float2 wvxwxw = (vwvxwv*wvvxxw*(1.0-(xvwvwx/3.141592654))*wvwwxv);
                float4 xwvwvw = tex2D(vvxxww,TRANSFORM_TEX(wvxwxw, vvxxww));
                float4 wvvxxv_var = tex2D(wvvxxv,TRANSFORM_TEX(i.uv0, wvvxxv));
                float4 vwxwww_var = tex2D(vwxwww,TRANSFORM_TEX(i.uv0, vwxwww));
                float3 wxvxxw = (lerp(pow(((vwwxwx*vwxvww.rgb)*wwvxvx.rgb),vxwwww),float3(vxxxwx,vxxxwx,vxxxwx),max((1.0 - xwvwvw.rgb),wvvxxv_var.rgb))+lerp(pow((vwxwww_var.rgb*vxvvvw),wxxxvv),float3(vxxxwx,vxxxwx,vxxxwx),wvvxxv_var.rgb));
                float3 specularColor = wxvxxw;
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 diffuseColor = (_MainTex_var.rgb*_Color.rgb);
                float3 diffuse = directDiffuse * diffuseColor;
                float3 finalColor = diffuse + specular;
                fixed4 finalRGBA = fixed4(finalColor * 1,0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}

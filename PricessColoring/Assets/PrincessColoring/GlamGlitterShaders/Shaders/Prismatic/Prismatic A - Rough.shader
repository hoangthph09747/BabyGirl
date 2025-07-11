// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Spaventacorvi/Prismatic/Prismatic A - Rough" {
    Properties {
        _Color ("Main color", Color) = (0.7058823,0.7058823,0.7058823,1)
        xxvvwv ("Specular (0-1)", Range(0, 1)) = 0.3
        xwxvww ("Gloss (0-1)", Range(0, 1)) = 0.6
        _MainTex ("Base (RGB)", 2D) = "white" {}
        vvxwwx ("Mask", 2D) = "white" {}
        vvxxvv ("Foreground transp (White)", Range(0, 1)) = 0
        wxxwww ("Background transp (Black)", Range(0, 1)) = 0
        vwwvvx ("Prism color", Color) = (1,1,1,1)
        wwvxvx ("Prism raimbow", 2D) = "white" {}
        vxwwww ("Prism Mask", 2D) = "white" {}
        xvxwww ("Prism multiplier", Float ) = 12
        vxxxwx ("Prism power (1-5)", Range(1, 5)) = 1.5
        vxvvvw ("Prism contrast (1- 5)", Range(1, 5)) = 3
        wxxxww ("Silver back (0 - 1)", Range(0, 1)) = 0.5
        xvwvwx ("Speed (0 - 5)", Range(0, 5)) = 3.5
        wxxxvv ("Fade distance", Float ) = 40
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
            #pragma glsl
            uniform float4 _LightColor0;
            uniform float xwxvww;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _Color;
            uniform float4 vwwvvx;
            uniform sampler2D wwvxvx; uniform float4 wwvxvx_ST;
            uniform float vxxxwx;
            uniform float xvxwww;
            uniform float vxvvvw;
            uniform float xxvvwv;
            uniform sampler2D vvxwwx; uniform float4 vvxwwx_ST;
            uniform float wxxwww;
            uniform float vvxxvv;
            uniform sampler2D vxwwww; uniform float4 vxwwww_ST;
            uniform float wxxxww;
            uniform float xvwvwx;
            uniform float wxxxvv;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
                float4 projPos : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                o.screenPos = float4( o.pos.xy / o.pos.w, 0, 0 );
                o.screenPos.y *= _ProjectionParams.x;
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float gloss = xwxvww;
                float specPow = exp2( gloss * 10.0+1.0);
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float3 specularColor = float3(xxvvwv,xxvvwv,xxvvwv);
                float3 directSpecular = (floor(attenuation) * _LightColor0.xyz) * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb;
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 wvwvww = (_Color.rgb*_MainTex_var.rgb);
                float3 wvvxxw = (viewReflectDirection*lightDirection*xvwvwx).rgb;
                float wvwwxv = (wvvxxw.r+wvvxxw.g+wvvxxw.b+(float2(i.screenPos.x*(_ScreenParams.r/_ScreenParams.g), i.screenPos.y).r*float2(i.screenPos.x*(_ScreenParams.r/_ScreenParams.g), i.screenPos.y).g));
                float wvxwxw_ang = wvwwxv;
                float wvxwxw_spd = 1.0;
                float wvxwxw_cos = cos(wvxwxw_spd*wvxwxw_ang);
                float wvxwxw_sin = sin(wvxwxw_spd*wvxwxw_ang);
                float2 wvxwxw_piv = float2(0.5,0.5);
                float2 node_175 = ((0.7*frac((i.uv0*xvxwww)))-(-0.15));
                float2 wvxwxw = (mul(node_175-wvxwxw_piv,float2x2( wvxwxw_cos, -wvxwxw_sin, wvxwxw_sin, wvxwxw_cos))+wvxwxw_piv);
                float vwvxwv = 0.0;
                float4 wvvxxv = tex2Dlod(wwvxvx,float4(TRANSFORM_TEX(wvxwxw, wwvxvx),0.0,vwvxwv));
                float xwvwvw_ang = (1.0 - wvwwxv);
                float xwvwvw_spd = 1.0;
                float xwvwvw_cos = cos(xwvwvw_spd*xwvwvw_ang);
                float xwvwvw_sin = sin(xwvwvw_spd*xwvwvw_ang);
                float2 xwvwvw_piv = float2(0.5,0.5);
                float2 xwvwvw = (mul(node_175-xwvwvw_piv,float2x2( xwvwvw_cos, -xwvwvw_sin, xwvwvw_sin, xwvwvw_cos))+xwvwvw_piv);
                float4 vwxwww = tex2Dlod(vxwwww,float4(TRANSFORM_TEX(xwvwvw, vxwwww),0.0,vwvxwv));
                float3 node_1034 = (pow((wvvxxv.rgb*vwxwww.rgb),vxvvvw)*vxxxwx);
                float vwxvww_ang = wvwwxv;
                float vwxvww_spd = 1.0;
                float vwxvww_cos = cos(vwxvww_spd*vwxvww_ang);
                float vwxvww_sin = sin(vwxvww_spd*vwxvww_ang);
                float2 vwxvww_piv = float2(0.5,0.5);
                float2 vwxvww = (mul(((i.uv0*0.2)-(-0.15))-vwxvww_piv,float2x2( vwxvww_cos, -vwxvww_sin, vwxvww_sin, vwxvww_cos))+vwxvww_piv);
                float4 vwwvxw = tex2D(wwvxvx,TRANSFORM_TEX(vwxvww, wwvxvx));
                float3 node_2076 = lerp(((vwwvvx.rgb*node_1034)+saturate(((vwxwww.rgb*wxxxww)-lerp(node_1034,dot(node_1034,float3(0.3,0.59,0.11)),1.0)))),vwwvxw.rgb,saturate((saturate((partZ/wxxxvv))*1.5+-0.5)));
                float4 vvxwwx_var = tex2D(vvxwwx,TRANSFORM_TEX(i.uv0, vvxwwx));
                float3 diffuseColor = lerp(lerp((wvwvww*node_2076),node_2076,(1.0 - wxxwww)),wvwvww,(vvxwwx_var.rgb*vvxxvv));
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
                float3 finalColor = diffuse + specular;
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
            #pragma glsl
            uniform float4 _LightColor0;
            uniform float xwxvww;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform float4 _Color;
            uniform float4 vwwvvx;
            uniform sampler2D wwvxvx; uniform float4 wwvxvx_ST;
            uniform float vxxxwx;
            uniform float xvxwww;
            uniform float vxvvvw;
            uniform float xxvvwv;
            uniform sampler2D vvxwwx; uniform float4 vvxwwx_ST;
            uniform float wxxwww;
            uniform float vvxxvv;
            uniform sampler2D vxwwww; uniform float4 vxwwww_ST;
            uniform float wxxxww;
            uniform float xvwvwx;
            uniform float wxxxvv;
            struct VertexInput {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 screenPos : TEXCOORD3;
                float4 projPos : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                o.screenPos = float4( o.pos.xy / o.pos.w, 0, 0 );
                o.screenPos.y *= _ProjectionParams.x;
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 viewReflectDirection = reflect( -viewDirection, normalDirection );
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
                float3 halfDirection = normalize(viewDirection+lightDirection);
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
                float gloss = xwxvww;
                float specPow = exp2( gloss * 10.0+1.0);
                float NdotL = max(0, dot( normalDirection, lightDirection ));
                float3 specularColor = float3(xxvvwv,xxvvwv,xxvvwv);
                float3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                float3 specular = directSpecular;
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float3 directDiffuse = max( 0.0, NdotL) * attenColor;
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                float3 wvwvww = (_Color.rgb*_MainTex_var.rgb);
                float3 wvvxxw = (viewReflectDirection*lightDirection*xvwvwx).rgb;
                float wvwwxv = (wvvxxw.r+wvvxxw.g+wvvxxw.b+(float2(i.screenPos.x*(_ScreenParams.r/_ScreenParams.g), i.screenPos.y).r*float2(i.screenPos.x*(_ScreenParams.r/_ScreenParams.g), i.screenPos.y).g));
                float wvxwxw_ang = wvwwxv;
                float wvxwxw_spd = 1.0;
                float wvxwxw_cos = cos(wvxwxw_spd*wvxwxw_ang);
                float wvxwxw_sin = sin(wvxwxw_spd*wvxwxw_ang);
                float2 wvxwxw_piv = float2(0.5,0.5);
                float2 node_175 = ((0.7*frac((i.uv0*xvxwww)))-(-0.15));
                float2 wvxwxw = (mul(node_175-wvxwxw_piv,float2x2( wvxwxw_cos, -wvxwxw_sin, wvxwxw_sin, wvxwxw_cos))+wvxwxw_piv);
                float vwvxwv = 0.0;
                float4 wvvxxv = tex2Dlod(wwvxvx,float4(TRANSFORM_TEX(wvxwxw, wwvxvx),0.0,vwvxwv));
                float xwvwvw_ang = (1.0 - wvwwxv);
                float xwvwvw_spd = 1.0;
                float xwvwvw_cos = cos(xwvwvw_spd*xwvwvw_ang);
                float xwvwvw_sin = sin(xwvwvw_spd*xwvwvw_ang);
                float2 xwvwvw_piv = float2(0.5,0.5);
                float2 xwvwvw = (mul(node_175-xwvwvw_piv,float2x2( xwvwvw_cos, -xwvwvw_sin, xwvwvw_sin, xwvwvw_cos))+xwvwvw_piv);
                float4 vwxwww = tex2Dlod(vxwwww,float4(TRANSFORM_TEX(xwvwvw, vxwwww),0.0,vwvxwv));
                float3 node_1034 = (pow((wvvxxv.rgb*vwxwww.rgb),vxvvvw)*vxxxwx);
                float vwxvww_ang = wvwwxv;
                float vwxvww_spd = 1.0;
                float vwxvww_cos = cos(vwxvww_spd*vwxvww_ang);
                float vwxvww_sin = sin(vwxvww_spd*vwxvww_ang);
                float2 vwxvww_piv = float2(0.5,0.5);
                float2 vwxvww = (mul(((i.uv0*0.2)-(-0.15))-vwxvww_piv,float2x2( vwxvww_cos, -vwxvww_sin, vwxvww_sin, vwxvww_cos))+vwxvww_piv);
                float4 vwwvxw = tex2D(wwvxvx,TRANSFORM_TEX(vwxvww, wwvxvx));
                float3 node_2076 = lerp(((vwwvvx.rgb*node_1034)+saturate(((vwxwww.rgb*wxxxww)-lerp(node_1034,dot(node_1034,float3(0.3,0.59,0.11)),1.0)))),vwwvxw.rgb,saturate((saturate((partZ/wxxxvv))*1.5+-0.5)));
                float4 vvxwwx_var = tex2D(vvxwwx,TRANSFORM_TEX(i.uv0, vvxwwx));
                float3 diffuseColor = lerp(lerp((wvwvww*node_2076),node_2076,(1.0 - wxxwww)),wvwvww,(vvxwwx_var.rgb*vvxxvv));
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

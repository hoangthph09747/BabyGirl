// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Spaventacorvi/Glitter/GlitterB-OldData" {
    Properties {
        //_Color ("Main color", Color) = (0.7058823,0.7058823,0.7058823,1)
		_Color("Main color", Color) = (1,1,1,1)
       /*[PerRendererData]*/ _MainTex ("Base (RGB)", 2D) = "white" {}
        wvwvww ("Shininess", Range(0, 1)) = 1
        vvxwwx ("Fake light", Range(0, 0.1)) = 0.1
        vwxwww ("Specular glitter", 2D) = "white" {}
        vxvvvw ("Specular power (0 - 5)", Range(0, 5)) = 1.5
        wxxxvv ("Specular contrast (1 - 3)", Range(0, 3)) = 0.01
        vvxxww ("Glitter map", 2D) = "white" {}
        vwxvww ("Glitter color", Color) = (1,1,1,1)
        vwwxwx ("Glitter power (0 - 10)", Range(0, 10)) = 10
        vxwwww ("Glitter contrast (1 - 3)", Range(1, 3)) = 3
        xvwvwx ("Glittery speed (0 - 1)", Range(0, 1)) = 0.5
        wvvxxw ("Glittery & mask dots scale", Range(0.1, 8)) = 0.6 //2.5
        wvvxxv ("Mask", 2D) = "black" {}
        wvwwxv ("Mask adjust (0.5 - 1.5)", Range(0.5, 1.5)) = 1
          //  _MaskTex("Mask Texture", 2D) = "white" {}
        _MaskTex("Mask real", 2D) = "black" {}
        _isMask("is mask 0/1", int) = 1
        
			//ngocdu
			_RotationSpeed("Rotation Speed", float) = 0.0
			_isSpark("is spark 0/1", int) = 1

            // 0 - hiện cả phần bên trong
            // 1- chỉ hiện viền
        _isTranparent("is Tranparent 0/1", int) = 0
        _isGlitter("is glitter 0/1", int) = 1
    }
    SubShader {
        Tags {
            "Queue" = "Transparent"
            "RenderType"="Opaque"
			"IgnoreProjector" = "True"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }

            Cull Off // cái này ko để off thì cái nào flip là ko nhìn thấy
            Lighting Off
			ZWrite Off
             ZTest[unity_GUIZTestMode]
            // ColorMask[_ColorMask]

			Blend SrcAlpha OneMinusSrcAlpha
           
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"

			//ngocdu
			//#include "Lighting.cginc"

            #pragma multi_compile_fwdbase_fullshadows
            #pragma multi_compile_fog
           // #pragma exclude_renderers xbox360 ps3 
            #pragma target 3.0
			//#pragma target 2.0
            uniform fixed4 _LightColor0;
            uniform sampler2D wvvxxv; uniform fixed4 wvvxxv_ST;
            uniform fixed wvwvww;
            uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
            uniform sampler2D vwxwww; uniform fixed4 vwxwww_ST;
            uniform sampler2D _MaskTex; uniform fixed4 _MaskTex_ST;
            uniform fixed vxvvvw;
            uniform fixed vwwxwx;
            uniform fixed4 _Color;
            uniform fixed vvxwwx;
            uniform fixed4 vwxvww;
            uniform fixed wvvxxw;
            uniform fixed xvwvwx;
            uniform sampler2D vvxxww; uniform fixed4 vvxxww_ST;
            uniform fixed vxwwww;
            uniform fixed wxxxvv;
            uniform fixed wvwwxv;

			//ngocdu
			fixed _isSpark;
            fixed _isMask;
            fixed _isTranparent;
            fixed _isGlitter;

            struct VertexInput {
                fixed4 vertex : POSITION;
                fixed3 normal : NORMAL;
                fixed4 tangent : TANGENT;
                fixed2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                fixed4 pos : SV_POSITION;

				//ngocdu
				fixed4 col : COLOR;

                fixed2 uv0 : TEXCOORD0;
                fixed4 posWorld : TEXCOORD1;
                fixed3 normalDir : TEXCOORD2;
                fixed3 tangentDir : TEXCOORD3;
                fixed3 bitangentDir : TEXCOORD4;
                LIGHTING_COORDS(5,6)
                UNITY_FOG_COORDS(7)
            };

			//ngocdu
			fixed _RotationSpeed;
			fixed rand(fixed3 myVector) {
				return frac(sin(_Time[0] * dot(myVector, fixed3(12.9898, 78.233, 45.5432))) * 43758.5453);
			}
			//end

            VertexOutput vert (VertexInput v) {

				//ngocdu
				v.texcoord0.xy -= 0.5;
				fixed s = sin(_RotationSpeed * _Time);
				fixed c = cos(_RotationSpeed * _Time);
				fixed2x2 rotationMatrix = fixed2x2(c, -s, s, c);
				rotationMatrix *= 0.5;
				rotationMatrix += 0.5;
				rotationMatrix = rotationMatrix * 2 - 1;
				v.texcoord0.xy = mul(v.texcoord0.xy, rotationMatrix);
				v.texcoord0.xy += 0.5;

                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.tangentDir = normalize( mul( unity_ObjectToWorld, fixed4( v.tangent.xyz, 0.0 ) ).xyz );
                o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                fixed3 lightColor = _LightColor0.rgb;

				//ngocdu
				/*fixed4x4 modelMatrix = unity_ObjectToWorld;
				fixed4x4 modelMatrixInverse = unity_WorldToObject;

				fixed3 normalDirection = normalize(
					mul(fixed4(v.normal, 0.0), modelMatrixInverse).xyz);
				fixed3 lightDirection =  normalize(_WorldSpaceLightPos0.xyz);

				fixed3 diffuseReflection = _LightColor0.rgb * _Color.rgb
					* max(0.0, dot(normalDirection, lightDirection));

				o.col = fixed4(diffuseReflection, 1.0);*/
				
				


                o.pos = UnityObjectToClipPos(v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
				TRANSFER_VERTEX_TO_FRAGMENT(o)


					
                return o;
            }


			//ngocdu
			//// caculate parallax uv offset
			//inline fixed2 CaculateParallaxUV(v2f i, fixed heightMulti)
			//{
			//	fixed height = tex2D(_ParallaxMap, i.uv).r;
			//	//normalize view Dir
			//	fixed3 viewDir = normalize(i.lightDir_tangent);
			//	//偏移值 = 切线空间的视线方向.xy（uv空间下的视线方向）* height * 控制系数
			//	fixed2 offset = i.lightDir_tangent.xy * height * _HeightFactor * heightMulti;
			//	return offset;
			//}

            fixed4 frag(VertexOutput i) : COLOR {
                i.normalDir = normalize(i.normalDir);
                fixed3x3 tangentTransform = fixed3x3( i.tangentDir, i.bitangentDir, i.normalDir);
                fixed3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                fixed3 normalDirection = i.normalDir;
                fixed3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                fixed3 lightColor = _LightColor0.rgb;
                fixed3 halfDirection = normalize(viewDirection+lightDirection);
                fixed attenuation = LIGHT_ATTENUATION(i);
                fixed3 attenColor = attenuation * _LightColor0.xyz;
                fixed gloss = wvwvww;
                fixed specPow = exp2( gloss * 10.0+1.0);
                fixed NdotL = max(0, dot( normalDirection, lightDirection ));
                fixed xvxwww = 0.0;
                fixed2 vwwvxw = ((0.05*(xvwvwx - xvxwww)*mul(tangentTransform, viewDirection).xy + i.uv0).rg*((xvwvwx/2.0)+1.0)*wvvxxw);
                fixed4 wwvxvx = tex2D(vvxxww,TRANSFORM_TEX(vwwvxw, vvxxww));
                fixed vxxxwx = 0.0;
                fixed vwvxwv_ang = 3.14;
                fixed vwvxwv_spd = 1.0;
                fixed vwvxwv_cos = cos(vwvxwv_spd*vwvxwv_ang);
                fixed vwvxwv_sin = sin(vwvxwv_spd*vwvxwv_ang);
                fixed2 vwvxwv_piv = fixed2(0.5,0.5);
                fixed2 vwvxwv = (mul((0.05*((-1*xvwvwx) - xvxwww)*mul(tangentTransform, viewDirection).xy + i.uv0).rg-vwvxwv_piv,fixed2x2( vwvxwv_cos, -vwvxwv_sin, vwvxwv_sin, vwvxwv_cos))+vwvxwv_piv);
                fixed2 wvxwxw = (vwvxwv*wvvxxw*(1.0-(xvwvwx/3.141592654))*wvwwxv);
                fixed4 xwvwvw = tex2D(vvxxww,TRANSFORM_TEX(wvxwxw, vvxxww));
                fixed4 wvvxxv_var = tex2D(wvvxxv,TRANSFORM_TEX(i.uv0, wvvxxv));
                fixed4 vwxwww_var = tex2D(vwxwww,TRANSFORM_TEX(i.uv0, vwxwww));
                fixed3 wxvxxw = (lerp(pow(((vwwxwx*vwxvww.rgb)*wwvxvx.rgb),vxwwww),fixed3(vxxxwx,vxxxwx,vxxxwx),max((1.0 - xwvwvw.rgb),wvvxxv_var.rgb))+lerp(pow((vwxwww_var.rgb*vxvvvw),wxxxvv),fixed3(vxxxwx,vxxxwx,vxxxwx),wvvxxv_var.rgb));
                fixed3 specularColor = wxvxxw;
                fixed3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
                fixed3 specular = directSpecular;
                NdotL = max(0.0,dot( normalDirection, lightDirection ));
                fixed3 directDiffuse = max( 0.0, NdotL) * attenColor;
                fixed3 indirectDiffuse = fixed3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb;
                fixed4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
                fixed3 diffuseColor = (_MainTex_var.rgb*_Color.rgb);
                fixed3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
                fixed3 emissive = (wxvxxw*vvxwwx);

				//ngocdu sparkle
				//fixed2 uvOffset = CaculateParallaxUV(i, 1);
				fixed noise1 = tex2D(vvxxww, i.uv0 * 2 + fixed2 (0, _Time.x * 0.1) ).r;
				fixed noise2 = tex2D(vvxxww, i.uv0 * 2 * 1.4 + fixed2 (_Time.x * 0.1, 0)).r;
				fixed sparkle1 = pow(noise1 * noise2 * 2, 10);

				//uvOffset = CaculateParallaxUV(i, 2);
				noise1 = tex2D(vvxxww, i.uv0 * 2 + fixed2 (0.3, _Time.x * 0.1) ).r;
				noise2 = tex2D(vvxxww, i.uv0 * 2 * 1.4 + fixed2 (_Time.x * 0.1, 0.3) ).r;
				fixed sparkle2 = pow(noise1 * noise2 * 2, 10);

				//uvOffset = CaculateParallaxUV(i, 3);
				noise1 = tex2D(vvxxww, i.uv0 * 2 + fixed2 (0.6, _Time.x * 0.91) ).r;
				noise2 = tex2D(vvxxww, i.uv0 * 2 * 1.4 + fixed2 (_Time.x * 0.91, 0.6) ).r;
				fixed sparkle3 = pow(noise1 * noise2 * 2, 10);


				fixed3 finalColor;
                if (_isGlitter == 1)
                {
                    if (_isSpark == 1) {
                        finalColor = diffuse + specular + emissive + sparkle1 + sparkle2 + sparkle3;
                    }
                    else
                        finalColor = diffuse + specular + emissive;
                }
                else
                {
                    finalColor = diffuse;
                }
				//fixed3 finalColor = diffuse + specular + emissive;

               // fixed3 finalColor = diffuse + specular + emissive + sparkle1 + sparkle2 + sparkle3;

                fixed4 finalRGBA = fixed4(finalColor,1);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);

                if (_isTranparent == 1)
                {
                    if (_MainTex_var.a == 0) {
                        finalRGBA.a = 0;
                    }
                }

				/*else if (_MainTex_var.a != 0)
				{
					finalRGBA.a = 1;
					finalRGBA.r = 1;
					finalRGBA.g = 1;
					finalRGBA.b = 1;
				}*/

                if (_isMask == 1)
                {
                    fixed4 _Mask_var = tex2D(_MaskTex, TRANSFORM_TEX(i.uv0, _MaskTex));
                    if (_Mask_var.a == 0) {
                        finalRGBA.a = 0;
                       // finalRGBA.a *= _Mask_var.r;
                       // clip(finalRGBA.a - 0.01);
                    }
                }

                return finalRGBA;


			//	i.normalDir = normalize(i.normalDir);

			//// attenuation
			//fixed attenuation = LIGHT_ATTENUATION(i);
			//fixed3 attenColor = attenuation * _LightColor0.xyz;

			//// specular
			//fixed specularPow = exp2((1 - _Gloss) * 10.0 + 1.0);
			//fixed3 specularColor = fixed4 (_Specular,_Specular,_Specular,1);

			//fixed3 halfVector = normalize(i.lightDir + i.viewDir);
			//fixed3 directSpecular = pow(max(0,dot(halfVector, i.normalDir)), specularPow) * specularColor;
			//fixed3 specular = directSpecular * attenColor;

			//// sparkle
			//fixed2 uvOffset = CaculateParallaxUV(i, 1);
			//fixed noise1 = tex2D(_NoiseTex, i.uv * _NoiseSize + fixed2 (0, _Time.x * _ShiningSpeed) + uvOffset).r;
			//fixed noise2 = tex2D(_NoiseTex, i.uv * _NoiseSize * 1.4 + fixed2 (_Time.x * _ShiningSpeed, 0)).r;
			//fixed sparkle1 = pow(noise1 * noise2 * 2, SparklePower);

			//uvOffset = CaculateParallaxUV(i, 2);
			//noise1 = tex2D(_NoiseTex, i.uv * _NoiseSize + fixed2 (0.3, _Time.x * _ShiningSpeed) + uvOffset).r;
			//noise2 = tex2D(_NoiseTex, i.uv * _NoiseSize * 1.4 + fixed2 (_Time.x * _ShiningSpeed, 0.3) + uvOffset).r;
			//fixed sparkle2 = pow(noise1 * noise2 * 2, SparklePower);

			//uvOffset = CaculateParallaxUV(i, 3);
			//noise1 = tex2D(_NoiseTex, i.uv * _NoiseSize + fixed2 (0.6, _Time.x * _ShiningSpeed) + uvOffset).r;
			//noise2 = tex2D(_NoiseTex, i.uv * _NoiseSize * 1.4 + fixed2 (_Time.x * _ShiningSpeed, 0.6) + uvOffset).r;
			//fixed sparkle3 = pow(noise1 * noise2 * 2, SparklePower);

			//// diffuse
			//fixed NdotL = saturate(dot(i.normalDir, i.lightDir));
			//fixed3 directDiffuse = NdotL * attenColor;
			//fixed3 diffuseCol = lerp(_ShadowColor, _Tint, directDiffuse);

			//// Rim
			//fixed rim = 1.0 - max(0, dot(i.normalDir, i.viewDir));
			//fixed3 rimCol = _RimColor.rgb * pow(rim, _RimPower) * _RimIntensity;

			//// final color
			//fixed3 sparkleCol1 = sparkle1 * (specular * _specsparkleRate + directDiffuse * _diffsparkleRate + rimCol * _rimsparkleRate) * lerp(_SparkleColor, fixed3(1,1,1), 0.5);
			//fixed3 sparkleCol2 = sparkle2 * (specular * _specsparkleRate + directDiffuse * _diffsparkleRate + rimCol * _rimsparkleRate) * _SparkleColor;
			//fixed3 sparkleCol3 = sparkle3 * (specular * _specsparkleRate + directDiffuse * _diffsparkleRate + rimCol * _rimsparkleRate) * 0.5 * _SparkleColor;

			//fixed4 finalCol = fixed4(diffuseCol + specular + sparkleCol1 + sparkleCol2 + sparkleCol3 + rimCol, 1);

			//UNITY_APPLY_FOG(i.fogCoord, finalCol);

			////return finalCol;

			//fixed4 col = tex2D(_MainTex, i.uv);

			//return col * finalCol;
            }
            ENDCG
        }
   //     Pass {
   //         Name "FORWARD_DELTA"
   //         Tags {
   //             "LightMode"="ForwardAdd"
   //         }
   //         Blend One One
            
            
   //         CGPROGRAM
   //         #pragma vertex vert
   //         #pragma fragment frag
   //         #define UNITY_PASS_FORWARDADD
   //         #include "UnityCG.cginc"
   //         #include "AutoLight.cginc"
   //         #pragma multi_compile_fwdadd_fullshadows
   //         #pragma multi_compile_fog
   //         //#pragma exclude_renderers xbox360 ps3 
   //         #pragma target 3.0
   //         uniform fixed4 _LightColor0;
   //         uniform sampler2D wvvxxv; uniform fixed4 wvvxxv_ST;
   //         uniform fixed wvwvww;
   //         uniform sampler2D _MainTex; uniform fixed4 _MainTex_ST;
   //         uniform sampler2D vwxwww; uniform fixed4 vwxwww_ST;
   //         uniform sampler2D _MaskTex; uniform fixed4 _MaskTex_ST;
   //         uniform fixed vxvvvw;
   //         uniform fixed vwwxwx;
   //         uniform fixed4 _Color;
   //         uniform fixed vvxwwx;
   //         uniform fixed4 vwxvww;
   //         uniform fixed wvvxxw;
   //         uniform fixed xvwvwx;
   //         uniform sampler2D vvxxww; uniform fixed4 vvxxww_ST;
   //         uniform fixed vxwwww;
   //         uniform fixed wxxxvv;
   //         uniform fixed wvwwxv;
   //         struct VertexInput {
   //             fixed4 vertex : POSITION;
   //             fixed3 normal : NORMAL;
   //             fixed4 tangent : TANGENT;
   //             fixed2 texcoord0 : TEXCOORD0;
   //         };
   //         struct VertexOutput {
   //             fixed4 pos : SV_POSITION;
   //             fixed2 uv0 : TEXCOORD0;
   //             fixed4 posWorld : TEXCOORD1;
   //             fixed3 normalDir : TEXCOORD2;
   //             fixed3 tangentDir : TEXCOORD3;
   //             fixed3 bitangentDir : TEXCOORD4;
   //             LIGHTING_COORDS(5,6)
   //             UNITY_FOG_COORDS(7)
   //         };

			////ngocdu
			//fixed _RotationSpeed;
			//fixed _isSpark;
   //         fixed _isMask;
   //         fixed _isTranparent;

   //         VertexOutput vert (VertexInput v) {

			//	//ngocdu
			//	v.texcoord0.xy -= 0.5;
			//	fixed s = sin(_RotationSpeed * _Time);
			//	fixed c = cos(_RotationSpeed * _Time);
			//	fixed2x2 rotationMatrix = fixed2x2(c, -s, s, c);
			//	rotationMatrix *= 0.5;
			//	rotationMatrix += 0.5;
			//	rotationMatrix = rotationMatrix * 2 - 1;
			//	v.texcoord0.xy = mul(v.texcoord0.xy, rotationMatrix);
			//	v.texcoord0.xy += 0.5;

   //             VertexOutput o = (VertexOutput)0;
   //             o.uv0 = v.texcoord0;
   //             o.normalDir = UnityObjectToWorldNormal(v.normal);
   //             o.tangentDir = normalize( mul( unity_ObjectToWorld, fixed4( v.tangent.xyz, 0.0 ) ).xyz );
   //             o.bitangentDir = normalize(cross(o.normalDir, o.tangentDir) * v.tangent.w);
   //             o.posWorld = mul(unity_ObjectToWorld, v.vertex);
   //             fixed3 lightColor = _LightColor0.rgb;

				

   //             o.pos = UnityObjectToClipPos(v.vertex );
   //             UNITY_TRANSFER_FOG(o,o.pos);
   //             TRANSFER_VERTEX_TO_FRAGMENT(o)
   //             return o;
   //         }
   //         fixed4 frag(VertexOutput i) : COLOR {
   //             i.normalDir = normalize(i.normalDir);
   //             fixed3x3 tangentTransform = fixed3x3( i.tangentDir, i.bitangentDir, i.normalDir);
   //             fixed3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
   //             fixed3 normalDirection = i.normalDir;
   //             fixed3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
   //             fixed3 lightColor = _LightColor0.rgb;
   //             fixed3 halfDirection = normalize(viewDirection+lightDirection);
   //             fixed attenuation = LIGHT_ATTENUATION(i);
   //             fixed3 attenColor = attenuation * _LightColor0.xyz;
   //             fixed gloss = wvwvww;
   //             fixed specPow = exp2( gloss * 10.0+1.0);
   //             fixed NdotL = max(0, dot( normalDirection, lightDirection ));
   //             fixed xvxwww = 0.0;
   //             fixed2 vwwvxw = ((0.05*(xvwvwx - xvxwww)*mul(tangentTransform, viewDirection).xy + i.uv0).rg*((xvwvwx/2.0)+1.0)*wvvxxw);
   //             fixed4 wwvxvx = tex2D(vvxxww,TRANSFORM_TEX(vwwvxw, vvxxww));
   //             fixed vxxxwx = 0.0;
   //             fixed vwvxwv_ang = 3.14;
   //             fixed vwvxwv_spd = 1.0;
   //             fixed vwvxwv_cos = cos(vwvxwv_spd*vwvxwv_ang);
   //             fixed vwvxwv_sin = sin(vwvxwv_spd*vwvxwv_ang);
   //             fixed2 vwvxwv_piv = fixed2(0.5,0.5);
   //             fixed2 vwvxwv = (mul((0.05*((-1*xvwvwx) - xvxwww)*mul(tangentTransform, viewDirection).xy + i.uv0).rg-vwvxwv_piv,fixed2x2( vwvxwv_cos, -vwvxwv_sin, vwvxwv_sin, vwvxwv_cos))+vwvxwv_piv);
   //             fixed2 wvxwxw = (vwvxwv*wvvxxw*(1.0-(xvwvwx/3.141592654))*wvwwxv);
   //             fixed4 xwvwvw = tex2D(vvxxww,TRANSFORM_TEX(wvxwxw, vvxxww));
   //             fixed4 wvvxxv_var = tex2D(wvvxxv,TRANSFORM_TEX(i.uv0, wvvxxv));
   //             fixed4 vwxwww_var = tex2D(vwxwww,TRANSFORM_TEX(i.uv0, vwxwww));
   //             fixed3 wxvxxw = (lerp(pow(((vwwxwx*vwxvww.rgb)*wwvxvx.rgb),vxwwww),fixed3(vxxxwx,vxxxwx,vxxxwx),max((1.0 - xwvwvw.rgb),wvvxxv_var.rgb))+lerp(pow((vwxwww_var.rgb*vxvvvw),wxxxvv),fixed3(vxxxwx,vxxxwx,vxxxwx),wvvxxv_var.rgb));
   //             fixed3 specularColor = wxvxxw;
   //             fixed3 directSpecular = attenColor * pow(max(0,dot(halfDirection,normalDirection)),specPow)*specularColor;
   //             fixed3 specular = directSpecular;
   //             NdotL = max(0.0,dot( normalDirection, lightDirection ));
   //             fixed3 directDiffuse = max( 0.0, NdotL) * attenColor;
   //             fixed4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(i.uv0, _MainTex));
   //             fixed3 diffuseColor = (_MainTex_var.rgb*_Color.rgb);
   //             fixed3 diffuse = directDiffuse * diffuseColor;
				
   //             fixed4 _Mask_var = tex2D(_MaskTex, TRANSFORM_TEX(i.uv0, _MaskTex));

			//	//ngocdu sparkle
			//	//fixed2 uvOffset = CaculateParallaxUV(i, 1);
			//	fixed noise1 = tex2D(vvxxww, i.uv0 * 2 + fixed2 (0, _Time.x * 0.1)).r;
			//	fixed noise2 = tex2D(vvxxww, i.uv0 * 2 * 1.4 + fixed2 (_Time.x * 0.1, 0)).r;
			//	fixed sparkle1 = pow(noise1 * noise2 * 2, 10);

			//	//uvOffset = CaculateParallaxUV(i, 2);
			//	noise1 = tex2D(vwxwww, i.uv0 * 2 + fixed2 (0.3, _Time.x * 0.1)).r;
			//	noise2 = tex2D(vwxwww, i.uv0 * 2 * 1.4 + fixed2 (_Time.x * 0.1, 0.3)).r;
			//	fixed sparkle2 = pow(noise1 * noise2 * 2, 10);

			//	//uvOffset = CaculateParallaxUV(i, 3);
			//	noise1 = tex2D(vvxxww, i.uv0 * 2 + fixed2 (0.6, _Time.x * 0.1)).r;
			//	noise2 = tex2D(vvxxww, i.uv0 * 2 * 1.4 + fixed2 (_Time.x * 0.1, 0.6)).r;
			//	fixed sparkle3 = pow(noise1 * noise2 * 2, 10);

			//	//fixed3 finalColor = diffuse + specular  + sparkle1 + sparkle2 + sparkle3;

			//	fixed3 finalColor;
			//	if (_isSpark == 1) {
			//		finalColor = diffuse + specular  + sparkle1 + sparkle2 + sparkle3;
			//	}
			//	else
			//		finalColor = diffuse + specular ;

				
			//	//fixed3 finalColor = diffuse + specular;

   //             fixed4 finalRGBA = fixed4(finalColor, 0) * 0;
   //             //UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
			//	finalRGBA.a = 0;
   //             return finalRGBA;
   //         }
   //         ENDCG
   //     }
    }
    FallBack "Diffuse"
}

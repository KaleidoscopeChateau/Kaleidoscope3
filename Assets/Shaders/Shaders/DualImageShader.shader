Shader "TriplanarUltimate/DualSurfaceShader" {
        
        //DUAL SURFACE SHADER
        //FREE TO USE, JUST CREDIT ME SOMEWHERE - Luke "Mab" V

        //||ACKNOWLEDGEMENTS||
        // Reoriented Normal Mapping
        // https://bgolus.medium.com/normal-mapping-for-a-triplanar-shader-10bf39dca05a

    Properties{
        [NoScaleOffset][MainTexture] _MainTex("Main Texture", 2D) = "white" {}                          // The Main Colour Texture.
        [NoScaleOffset][Normal] _BumpMap("Normal Map", 2D) = "bump" {}                                  // The Nomal Textrue for your Main Colour Texture.
        _MainScaleFactor("Main Scale Factor", Vector) = (1, 1, 1)                                       // Non uniform scaling. Smaller number bigger texture.
        _MainOffsetFactor("Main Offset Factor", Vector) = (0, 0, 0)                                     // Non uniform offset. Used to scroll the texture in different directions.
        [NoScaleOffset] _UnderTex("Underneath Texture", 2D) = "white" {}                                // The Colour Texture that will be revealed with the interpolator.
        [NoScaleOffset][Normal] _BumpMapUnder("Underneath Normal Map", 2D) = "bump" {}                  // The Normal Texture for that.
        _SecScaleFactor("Secondary Scale Factor", Vector) = (1, 1, 1)                                   // Same as above affects secondary textures.
        _SecOffsetFactor("Secondary Offset Factor", Vector) = (0, 0, 0)                                 // While the top one affected the primary textures.
        _Blending("Blending", Range(0,1)) = 0                                                           // Controls how the different planes blend into each other. 0 is sharp, 1 is blended
        [MainColor]_MainColor("Main Color", Color) = (1,1,1,1)                                          // The main top and bottom colours 
        _SecColor("Secondary Color", Color) = (1,1,1,1)                                                 // 
        _CChangeHeight("Colour Change Point", Float) = 0.0                                              // The world y value that chooses which colour the surface will be coloured by.
        _CChangeGrad("Colour Change Distance", Float) = 0.0                                             // How far the gradient between colours travels.
        _CChangePenPos("Colour Depth", Range(0,1)) = 0                                                  //
        _CChangePenWidth("Colour Depth Width", Range(0,1)) = 0                                          // How deep the colour penetrates.
        _CChangePenGrad("Depth Cutoff Smoothness", Range(0,1)) = 0                                      // Smoothnes of the transition.
        _CChangeAmbCol("Underneath Colour", Color) = (1,1,1,1)                                          // The secondary colour
        _Glossiness("Smoothness", Range(0, 1)) = 0.5                                                    // Primary Smoothness and metalic values to make your material shiny.
        [Gamma] _Metallic("Metallic", Range(0, 1)) = 0.5                                                //
        _SmoothStart("Smooth+Metal Depth", Range(0, 1)) = 0.5                                           // Values dictating how deep the smoothness and metallic values apply,
        _SmoothEnd("Smooth+Metal Width", Range(0, 1)) = .5                                              // dictated by the height of the interpolator texture.
        _SmoothSmooth("Smooth+Metal Gradient", Range(0, 1)) = 0                                         // 
        _SecGlossiness("Secondary Smoothness", Range(0, 1)) = 0                                         // Determining what the shinyness of the materials are outside of this band 
        [Gamma] _SecMetallic("Secondary Metallic", Range(0, 1)) = 0                                     // 
        [NoScaleOffset] _InterpBump("Interpolator Bump Texture", 2D) = "black" {}                       // The Interpolator Texture that interpolated between the main and secondary textures.
        [NoScaleOffset][Normal] _InterpNormal("Interpolator Normal Texture", 2D) = "bump" {}            // The Normal Texture for that. Just get/make a heightmap of something and use https://cpetry.github.io/NormalMap-Online/ to turn it into a normal. 
                                                                                                        // On the site, crank the strength, level and sharp to the max. Dont forget to label the texture as a normal map in the editor!
                                                                                                        // Mark McKay made a great tool for generating tileing noise maps, cant recommend enough http://kitfox.com/projects/perlinNoiseMaker/
        _IntScaleFactor("Int Scale Factor", Vector) = (1, 1, 1)                                         // Nonuniform scaling for the interpolator.
        _IntOffsetFactor("Int Offset Factor", Vector) = (0, 0, 0)                                       // Nonuniform scrolling for the interpolator.
        _Sin("Sin(Int Rotation Factor)", Vector) = (0, 0, 0)                                            // Non Uniform rotating for the interpolator.
        _Cos("Cos(Int Rotation Factor)", Vector) = (1, 1, 1)                                            // Saved as the sin and cos of itself to improve the performance a smidge
        [NoScaleOffset] _SecInterpBump("Second Interpolator Bump Texture", 2D) = "black" {}             // Second Interpolator texture to allow for increased variation
        [NoScaleOffset][Normal] _SecInterpNormal("Second Interpolator Normal Texture", 2D) = "bump" {}  // The Normal Texture for that. 
        _SecIntScaleFactor("Second Int Scale Factor", Vector) = (1, 1, 1)                               // The second one of all that.
        _SecIntOffsetFactor("Second Int Offset Factor", Vector) = (0, 0, 0)                             // 
        _SecSin("Sin(Second Int Rotation Factor)", Vector) = (0, 0, 0)                                  //
        _SecCos("Cos(Second Int Rotation Factor)", Vector) = (1, 1, 1)                                  //
        _IntBlend("Interpolator Blend", Range(0,1)) = 0.5                                               // Interpolator value between the 2 interpolator values. Brain hurt yet? Mine does
        _MinNoise("Interpolator Low Threshold", Range(0, 0.99)) = 0                                     // Clamping values for the interpolator texture, 
        _MaxNoise("Interpolator High Threshold", Range(0.01, 1)) = 1                                    // Determines what part of the surface is flat and what is bumpy
        [Toggle]_IntPosInf("Use Interpolator Clamp", Float) = 0                                         // Toggle using start and end position
        _IntClampPos("Int Clamp Position", Vector) = (0, 0, 0)                                          // Positions and gradient to determine which parts of the material are influenced by the interpretor
        _IntClampWidth("Int Clamp Width", Vector) = (1, 1, 1)                                           //
        _IntPosGrad("Int Clamp Multiplier", Vector) = (1, 1, 1)                                         // Gradients for smooth interpolation
        _NoiseBumpStrength("Noise Bump Strength", Range(-1,1)) = 1                                      // Determines the intensity of the noise bump map. Can be negative to make the
                                                                                                        // second texture look raised above the main
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 200

            CGPROGRAM
            #pragma surface surf Standard fullforwardshadows

            #pragma target 3.0

            #include "UnityStandardUtils.cginc"


            //Declaring variables
            //Variables with names shared with those in the properties get the values set from them 
            //Other variables work as normal
            //To change a value from a c# script, use material.Set(Inserttypehere)("NameofVariable", value)
            //Variables do not need to be properties in order for you to change them via script
            //When giving the name of the variable, make sure you use the actual name of the variable and not the display name given in the inspector
            //ie when modifying _MainColor("Main Color", Color), use material.SetColor("_MainColor", Color.White) and not material.SetColor("Main Colour", Color.White)
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float3 _MainScaleFactor;
            float3 _MainOffsetFactor;
            float3 _SecScaleFactor;
            float3 _SecOffsetFactor;
            float _Blending; 
            sampler2D _BumpMap;
            sampler2D _UnderTex;
            sampler2D _BumpMapUnder;
            float4 _MainColor;
            float4 _SecColor;
            float _CChangeHeight;
            float _CChangeGrad;
            float _CChangePenPos;
            float _CChangePenWidth;
            float _CChangePenGrad;
            float4 _CChangeAmbCol;
            half _Glossiness;
            half _Metallic;
            half _SecGlossiness;
            half _SecMetallic;
            float _MinNoise;
            float _MaxNoise;
            float _NoiseBumpStrength;
            float _SmoothStart;
            float _SmoothEnd;
            float _SmoothSmooth;
            float3 _IntScaleFactor;
            float3 _IntOffsetFactor;
            float3 _Sin;
            float3 _Cos;
            float3 _SecIntScaleFactor;
            float3 _SecIntOffsetFactor;
            float3 _SecSin;
            float3 _SecCos;
            sampler2D _InterpBump;
            sampler2D _InterpNormal;
            sampler2D _SecInterpBump;
            sampler2D _SecInterpNormal;
            float _IntBlend;
            float3 _IntClampPos;
            float3 _IntClampWidth;
            float _IntPosInf;
            float3 _IntPosGrad;
            float3 _IntPosEGrad;

            struct Input {
                float3 worldPos;
                float3 worldNormal;
                INTERNAL_DATA
            };


            //Takes the world normal vector and converts it into a 2d tangent vector, otherwise it looks very wrong
            float3 WorldToTangentNormalVector(Input IN, float3 normal) {
                float3 t2w0 = WorldNormalVector(IN, float3(1,0,0));
                float3 t2w1 = WorldNormalVector(IN, float3(0,1,0));
                float3 t2w2 = WorldNormalVector(IN, float3(0,0,1));
                float3x3 t2w = float3x3(t2w0, t2w1, t2w2);
                return normalize(mul(t2w, normal));
            }


            void surf(Input IN, inout SurfaceOutputStandard o) {

                

                //  For whatever reason, even when displaying the normal map as the colour shows that everything is fine, without doing this the whole material turns black
                //  This also makes it so that if you dont input the normal at the end of the script, the colour displays incorrectly
                //  Shaders are fun
                IN.worldNormal = WorldNormalVector(IN, float3(0,0,1));

                // calculate triplanar blend
                half3 triblend = saturate(pow(IN.worldNormal, 4));                                                  //Multiplies the world normal by a power of 4, then clamps the values to be between 0 and 1
                triblend /= max(dot(triblend, half3(1,1,1)), 0.0001);                                               //Gets the average or something idk 
                half m = max(triblend.x, max(triblend.y, triblend.z));                                              //Calculates which direction is the highest 
                half3 Noblend = half3(triblend.x == m, triblend.y == m, triblend.z == m);                           //Sets that direction to 1 and the rest to 0, this results in no blending between the planes
                triblend = lerp(Noblend, triblend, _Blending);                                                      //Interpolating between the average and the max versions to allow for user control over the smoothness of the blending

                //Rotation Matrixes for the interpolators
                float2x2 mat1 = float2x2(_Cos.x, -_Sin.x, _Sin.x, _Cos.x); float2x2 mat2 = float2x2(_Cos.y, -_Sin.y, _Sin.y, _Cos.y); float2x2 mat3 = float2x2(_Cos.z, -_Sin.z, _Sin.z, _Cos.z);                
                //Warning! high rotation will result in the normals not being correct, so only rotate a little
                float2x2 mat21 = float2x2(_SecCos.x, -_SecSin.x, _SecSin.x, _SecCos.x); float2x2 mat22 = float2x2(_SecCos.y, -_SecSin.y, _SecSin.y, _SecCos.y); float2x2 mat23 = float2x2(_SecCos.z, -_SecSin.z, _SecSin.z, _SecCos.z);         
                // calculate triplanar uvs
                float2 uvX = IN.worldPos.zy * _MainScaleFactor.zy + _MainOffsetFactor.zy;                           //Taking the different world planes and applying scaling and offsetting
                float2 uvY = IN.worldPos.xz * _MainScaleFactor.xz + _MainOffsetFactor.xz;                           //These apply to the main texture and its normal 
                float2 uvZ = IN.worldPos.xy * _MainScaleFactor.xy + _MainOffsetFactor.xy;
                float2 uvX2 = IN.worldPos.zy * _SecScaleFactor.zy + _SecOffsetFactor.zy;                            //And these to the secondary texture and its normal
                float2 uvY2 = IN.worldPos.xz * _SecScaleFactor.xz + _SecOffsetFactor.xz;            
                float2 uvZ2 = IN.worldPos.xy * _SecScaleFactor.xy + _SecOffsetFactor.xy;                            //So if you were wondering what those W values did if you werent using the custom editor, it was absolutely nothing, unity only lets you display float4s for some reason 
                float2 uvX3 = mul(mat1 , (IN.worldPos.zy * _IntScaleFactor.zy + _IntOffsetFactor.zy));              //And these to the interpolator texture and its normal
                float2 uvY3 = mul(mat2 , (IN.worldPos.xz * _IntScaleFactor.xz + _IntOffsetFactor.xz));
                float2 uvZ3 = mul(mat3 , (IN.worldPos.xy * _IntScaleFactor.xy + _IntOffsetFactor.xy));
                float2 uvX4 = mul(mat21 , (IN.worldPos.zy * _SecIntScaleFactor.zy + _SecIntOffsetFactor.zy));        //And again to add another layer, break up repeating interp textures
                float2 uvY4 = mul(mat22 , (IN.worldPos.xz * _SecIntScaleFactor.xz + _SecIntOffsetFactor.xz));
                float2 uvZ4 = mul(mat23 , (IN.worldPos.xy * _SecIntScaleFactor.xy + _SecIntOffsetFactor.xy));

                fixed4 c3olX = lerp(tex2D(_InterpBump, uvX3), tex2D(_SecInterpBump, uvX4), _IntBlend);                      //Interpolator Bump Texture value
                fixed4 c3olY = lerp(tex2D(_InterpBump, uvY3), tex2D(_SecInterpBump, uvY4), _IntBlend);
                fixed4 c3olZ = lerp(tex2D(_InterpBump, uvZ3), tex2D(_SecInterpBump, uvZ4), _IntBlend);
                float dn = c3olX * triblend.x + c3olY * triblend.y + c3olZ * triblend.z;                                    //Unclamped texture for more consistent reference points, Some n values would cause things to break lol
                float3 v = abs(IN.worldPos - _IntClampPos) - _IntClampWidth;

                float nn = max(v.x / _IntPosGrad.x, max(v.y / _IntPosGrad.y, v.z / _IntPosGrad.z)) / 2;
                nn = saturate(nn) * _IntPosInf;
                dn = saturate(dn - nn);
                float n = saturate(clamp(dn, _MinNoise, _MaxNoise) - _MinNoise) / (_MaxNoise - _MinNoise);

                //variables before calcs to allow for if statements
                //if statements are generally bad for performance, but this shader is so taxing otherwise that adding them actually saves performance 
                //without if statements, we are doing 24 image samples, with if statements, on the parts of the texture that are 0 or 1 in the interpolator, its 12

                fixed4 col = fixed4(1, 1, 1, 1);
                fixed4 c2ol = fixed4(1, 1, 1, 1);
                half3 B1X = half3(0.5, 0.5, 1);
                half3 B1Y = half3(0.5, 0.5, 1);
                half3 B1Z = half3(0.5, 0.5, 1);
                half3 B2X = half3(0.5, 0.5, 1);
                half3 B2Y = half3(0.5, 0.5, 1);
                half3 B2Z = half3(0.5, 0.5, 1);
                half3 inormalX = half3(0.5, 0.5, 1);
                half3 inormalY = half3(0.5, 0.5, 1);
                half3 inormalZ = half3(0.5, 0.5, 1);

                // albedo textures
                if (n < 1)
                {
                    fixed4 colX = tex2D(_MainTex, uvX);                                                                         //Assigning the different planes of the triplanar mapping 
                    fixed4 colY = tex2D(_MainTex, uvY);
                    fixed4 colZ = tex2D(_MainTex, uvZ);
                    col = colX * triblend.x + colY * triblend.y + colZ * triblend.z;                                            //Blending them all together based on the triplanar blend value calculated earlier       
                    B1X = UnpackNormal(tex2D(_BumpMap, uvX));
                    B1Y = UnpackNormal(tex2D(_BumpMap, uvY));
                    B1Z = UnpackNormal(tex2D(_BumpMap, uvZ));
                }
                                                                                               
                if (n > 0)
                {
                    fixed4 c2olX = tex2D(_UnderTex, uvX2);
                    fixed4 c2olY = tex2D(_UnderTex, uvY2);
                    fixed4 c2olZ = tex2D(_UnderTex, uvZ2);
                    c2ol = c2olX * triblend.x + c2olY * triblend.y + c2olZ * triblend.z;
                    B2X = UnpackNormal(tex2D(_BumpMapUnder, uvX2));
                    B2Y = UnpackNormal(tex2D(_BumpMapUnder, uvY2));
                    B2Z = UnpackNormal(tex2D(_BumpMapUnder, uvZ2));
                }
                    

                // tangent space normal maps
                half3 tnormalX = lerp(B1X, B2X, n);        //Interpolating between the different normal maps based on the interp value
                half3 tnormalY = lerp(B1Y, B2Y, n);
                half3 tnormalZ = lerp(B1Z, B2Z, n);

                float i = 0;

                if (n - int(n) != 0)
                {
                    inormalX = lerp(UnpackNormal(tex2D(_InterpNormal, uvX3)), UnpackNormal(tex2D(_SecInterpNormal, uvX4)), _IntBlend);        //Interpolating between interpolator normal texture values
                    inormalY = lerp(UnpackNormal(tex2D(_InterpNormal, uvY3)), UnpackNormal(tex2D(_SecInterpNormal, uvY4)), _IntBlend);        //Causes occasional incorrect looking normal maps, but the alternative was to calculate the combined normal of 
                    inormalZ = lerp(UnpackNormal(tex2D(_InterpNormal, uvZ3)), UnpackNormal(tex2D(_SecInterpNormal, uvZ4)), _IntBlend);        //The 2 bump maps on the fly, which would have been a lot more buggy, unstable, and complex
                    inormalX.x *= sign(_NoiseBumpStrength);                                                                                   //Flipping the normal texture if strength is negative
                    inormalY.x *= sign(_NoiseBumpStrength);
                    inormalZ.x *= sign(_NoiseBumpStrength);
                    inormalX.y *= -sign(_NoiseBumpStrength);
                    inormalY.y *= -sign(_NoiseBumpStrength);
                    inormalZ.y *= -sign(_NoiseBumpStrength);
                    inormalX = lerp(tnormalX, inormalX, abs(_NoiseBumpStrength));                                                             //Lerping between the regular normal map and the interps for strength
                    inormalY = lerp(tnormalY, inormalY, abs(_NoiseBumpStrength));
                    inormalZ = lerp(tnormalZ, inormalZ, abs(_NoiseBumpStrength));
                    i = saturate((dn > _MinNoise && dn < _MaxNoise) - nn);
                    tnormalX = lerp(tnormalX, inormalX, i);
                    tnormalY = lerp(tnormalY, inormalY, i);
                    tnormalZ = lerp(tnormalZ, inormalZ, i);
                }
                
               
                tnormalX = half3(tnormalX.xy + IN.worldNormal.zy, IN.worldNormal.x);
                tnormalY = half3(tnormalY.xy + IN.worldNormal.xz, IN.worldNormal.y);
                tnormalZ = half3(tnormalZ.xy + IN.worldNormal.xy, IN.worldNormal.z); 


                // sizzle tangent normals to match world normal and blend together
                half3 worldNormal = normalize(
                    tnormalX.zyx * triblend.x +
                    tnormalY.xzy * triblend.y +
                    tnormalZ.xyz * triblend.z
                    );


                // set surface ouput properties
                
                float colp = (abs(dn - _CChangePenPos) - _CChangePenWidth)/ _CChangePenGrad;                                                                    //lerp value between the primary colours and the secondary colour


                float3 Normal = WorldToTangentNormalVector(IN, worldNormal);                                                                                    //Turn the world normal into a tangent one
                float4 cc = lerp(lerp(_SecColor, _MainColor, clamp((IN.worldPos.y - _CChangeHeight)/_CChangeGrad, 0,1)), _CChangeAmbCol, saturate(colp));       //Applying the colour to the based on the parameters given
                float4 ccc = lerp(col, c2ol, n) * cc;                                                                                                           //Lerping between the 2 Base colours. sorry about my naming conventions lol  
                o.Albedo = ccc;                                                                                                                                 //setting the colour of the material 
                float sm = (abs(dn - _SmoothStart) - _SmoothEnd) / _SmoothSmooth;                                                                               //lerp value between the primary and secondary smoothness
                o.Metallic = lerp(_Metallic, _SecMetallic, saturate(sm))* ccc.a;                                                                                       //applying which smoothness value based on that
                o.Smoothness = lerp(_Glossiness, _SecGlossiness, saturate(sm))* ccc.a;

               
                
                
                

                o.Normal = Normal;                                                                                                                              //Applies normal
                                                                                                   
                                                                                                  
            }
            ENDCG
        }
            FallBack "Diffuse"
            CustomEditor "DualSurfaceShaderEditor"
}

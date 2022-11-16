
struct appdata {
	float4 vertex : POSITION;
	float2 uv : TEXCOORD0;
};
			
struct v2f {
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
	UNITY_FOG_COORDS(1)
};

sampler2D _MainTex;
float4 _MainTex_ST;
fixed4 _Color;
bool _Flip;
			
v2f vert (appdata v)
{
	v2f o;
				
	float3 scale = float3(length(mul(unity_ObjectToWorld, float4(1, 0, 0, 0)).xyz), length(mul(unity_ObjectToWorld, float4(0, 1, 0, 0)).xyz), length(mul(unity_ObjectToWorld, float4(0, 0, 1, 0)).xyz));
	if(_Flip){
		scale.x *= -1;
	}
	float4 objPos = mul(unity_ObjectToWorld, float4(0, 0, 0, 1));

	#if defined(USING_STEREO_MATRICES)
		float3 cameraPos = (unity_StereoWorldSpaceCameraPos[0] + unity_StereoWorldSpaceCameraPos[1]) * .5;
	#else
		float3 cameraPos = _WorldSpaceCameraPos;
	#endif

	float3 direction = normalize(cameraPos - objPos.xyz);

	float4x4 billboardMatrix = 0;
	billboardMatrix._m02 = direction.x;
	billboardMatrix._m12 = direction.y;
	billboardMatrix._m22 = direction.z;
	float3 xAxis = normalize(float3(-direction.z, 0, direction.x));
	billboardMatrix._m00 = xAxis.x;
	billboardMatrix._m10 = 0;
	billboardMatrix._m20 = xAxis.z;
	float3 yAxis = normalize(cross(xAxis, direction));
	billboardMatrix._m01 = yAxis.x;
	billboardMatrix._m11 = yAxis.y;
	billboardMatrix._m21 = yAxis.z;
				
	o.pos = mul(UNITY_MATRIX_VP, mul(billboardMatrix, v.vertex * float4(scale, 0)) + objPos);
	o.uv = TRANSFORM_TEX(v.uv, _MainTex);
	UNITY_TRANSFER_FOG(o,o.pos);

	return o;
}

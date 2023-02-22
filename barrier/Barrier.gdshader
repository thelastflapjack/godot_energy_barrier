shader_type spatial;

uniform vec3 colorGranted : source_color;
uniform vec3 colorDenied : source_color;

instance uniform float deniedWeight : hint_range(0.0, 1.0, 0.01) = 1.0;
instance uniform float poweredWeight : hint_range(0.0, 1.0, 0.01) = 1.0;

uniform sampler2D noise;

vec3 lerp(vec3 a, vec3 b, float weight)
{
	vec3 diff = b - a;
	return a + (weight * diff);
}

float cubicPulse( float center, float width, float x ){
    x = abs(x - center);
    x /= width;
    return 1.0 - x * x * (0.5 * x);
}

void fragment() {
	vec2 noiseReadPos = vec2(UV.y, (TIME * 0.015));
	float noiseVal = max(texture(noise, noiseReadPos).b * 0.75, 0.25);
	
	float edgeFadeWeight = 20.0;
	float edgeFade = pow(UV.x, edgeFadeWeight) + pow((1.0 - UV.x) , edgeFadeWeight);
	edgeFade = clamp(edgeFade, 0.0, 1.0);
	
	float pulseWidth = 1.0 - poweredWeight;
	float powerFade = 1.0 - cubicPulse(0.5, pulseWidth, UV.x);
	powerFade = clamp(powerFade, 0.0, 1.0);
	
	//ALBEDO = vec3(powerFade);
	//ALBEDO = vec3(edgeFade + noiseVal) * powerFade;
	
	ALBEDO = lerp(colorGranted, colorDenied, deniedWeight);
	ALPHA = clamp(edgeFade + noiseVal, 0.0, 0.8) * powerFade;
}

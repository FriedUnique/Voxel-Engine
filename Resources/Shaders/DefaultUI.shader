#shader vertex
#version 330 core

layout (location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;
layout(location = 2) in float aBrightness;

out vec2 texCoord;
out float brightness;


void main() 
{
    gl_Position = vec4(aPosition, 1.0);
    texCoord = aTexCoord;
    brightness = aBrightness;
}

#shader fragment
#version 330 core

out vec4 color;

in vec2 texCoord;
in float brightness;

uniform sampler2D texture0;

void main() 
{
    vec4 texColor = texture(texture0, texCoord);
    if(texColor.a < 0.1)
        discard;

    color = brightness * texColor;
}
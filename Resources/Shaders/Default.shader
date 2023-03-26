#shader vertex
#version 330 core

layout (location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;
layout(location = 2) in float aBrightness;

out vec2 texCoord;
out float brightness;

uniform mat4 MVP;


void main() 
{
    gl_Position = MVP * vec4(aPosition, 1.0);
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
    color = brightness * texture(texture0, texCoord);
}
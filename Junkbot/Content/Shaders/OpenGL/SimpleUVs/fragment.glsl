#version 330 core

in vec2 UV;

out vec3 Colour;

uniform sampler2D TextureSampler;


void main()
{
    Colour = texture(TextureSampler, UV).rgb;
}
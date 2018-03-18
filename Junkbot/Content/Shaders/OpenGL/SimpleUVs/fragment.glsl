#version 330 core

in vec2 UV;

out vec4 Colour;

uniform sampler2D TextureSampler;


void main()
{
    Colour = texture(TextureSampler, UV).rgba;
}
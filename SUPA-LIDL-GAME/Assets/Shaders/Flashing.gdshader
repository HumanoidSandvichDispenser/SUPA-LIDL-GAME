shader_type canvas_item;

uniform bool active = true;

void fragment() {
    vec4 prev_color = texture(TEXTURE, UV);
    vec4 white = vec4(1, 1, 1, prev_color.a);
    COLOR = prev_color * float(!active) + white * float(active);
}
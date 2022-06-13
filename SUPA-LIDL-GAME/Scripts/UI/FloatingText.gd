extends Position2D


onready var label = $Label
onready var tween = $Tween
export var text: String

func _ready():
    var rng = RandomNumberGenerator.new()
    rng.randomize()
    var start = position + Vector2(rng.randf_range(-8, 8), 0)
    var end = start + Vector2(0, rng.randf_range(-4, -16))
    print(start)
    print(end)
    label.set_text(text)
    tween.interpolate_property(self, "position", start, end, 0.5,
            Tween.TRANS_QUINT, Tween.EASE_OUT)
    tween.interpolate_property(self, "modulate", modulate, Color(1, 1, 1, 0),
            0.5, Tween.TRANS_EXPO, Tween.EASE_OUT, 1.5)
    tween.interpolate_property(self, "scale", scale, Vector2(0.5, 0.5), 0.5,
            Tween.TRANS_LINEAR, Tween.EASE_OUT, 1.5)
    tween.start()
    $AnimationPlayer.play("Flash")

func _on_Tween_tween_all_completed():
    queue_free() # remove from the world once the tween is finished

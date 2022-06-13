extends Node2D

onready var animation_player = $AnimationPlayer
onready var stats = $Stats

var floating_text = preload("res://Objects/UI/FloatingText.tscn")

func _on_Hitbox_area_entered(_area: Area2D) -> void:
    print("Dummy got hit. FeelsDankMan")
    var instance = floating_text.instance()
    instance.text = "20"
    instance.global_position = global_position + Vector2.UP * 8
    get_tree().get_root().add_child(instance)
    animation_player.play("TrainingDummyHit")
    stats.Health -= 20


func _on_Stats_HealthChanged(_old_health: float, new_health: float):
    if new_health <= 0:
        queue_free()
    else:
        print("I am dying! Pogerino")

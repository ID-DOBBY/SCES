extends StaticBody2D
@export var speed:int = 1.9
var interpolation_value = 0
var starting_position
var PlayerPos : Vector2
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	starting_position = self.global_position
	pass # Replace with function body.
@onready var PickupArea: Area2D = $Area2D


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	if PickupArea.get_overlapping_bodies():
		for body in PickupArea.get_overlapping_bodies():
			if body is CharacterBody2D:
				PlayerPos = body.global_position
			else:
				PlayerPos = starting_position
		if (interpolation_value < 1):
			interpolation_value += delta * speed
			if (interpolation_value > 1):
				interpolation_value = 1
				self.queue_free()
			global_transform.origin = starting_position.lerp(PlayerPos, interpolation_value)

extends StaticBody2D
const DROPPED_STONE = preload("res://Scenes/dropped_stone.tscn")
var interpolation_value = 0#
var speed = 0.4
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	add_to_group("rock")
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass
	


func _drop_stone() -> void:
	var _inited_dropped_stone = DROPPED_STONE.instantiate()
	_inited_dropped_stone.starting_position = Vector2(randi_range(-5,5), randi_range(-5,5))
	self.add_child(_inited_dropped_stone)





func _on_area_2d_area_shape_entered(area_rid: RID, area: Area2D, area_shape_index: int, local_shape_index: int) -> void:
	print(area.get_parent().name)
	if(area.name == "Node"):
		area.get_parent().queue_free()
	pass # Replace with function body.

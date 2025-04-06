extends StaticBody2D
const DROPPED_STONE = preload("res://Scenes/dropped_stone.tscn")
var interpolation_value = 0
var speed = 0.4
var drop_indices := []
@export var total_hits := 45
@export var total_drops := 20
var current_hit := 0

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	add_to_group("rock")
	var all_indices = range(total_hits)
	all_indices.shuffle()
	drop_indices = all_indices.slice(0, total_drops)
	drop_indices.sort()
	print(drop_indices)
	pass # Replace with function body.


func _drop_stone() -> void:
	if current_hit>total_hits:
		queue_free()
		pass
	if drop_indices.has(current_hit):
		var _inited_dropped_stone = DROPPED_STONE.instantiate()
		_inited_dropped_stone.end_position = Vector2(randi_range(randi_range(-1200,-2000),randi_range(1200,2000)), randi_range(randi_range(-1200,-2000),randi_range(1200,2000)))
		_inited_dropped_stone.starting_position = self.position
		self.get_parent().add_child(_inited_dropped_stone)
	current_hit +=1
	print(current_hit)
	


func _on_area_2d_area_shape_entered(area_rid: RID, area: Area2D, area_shape_index: int, local_shape_index: int) -> void:
	if(area.name == "Node"):
		area.get_parent().queue_free()
	pass

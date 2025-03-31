extends Node2D
@onready var tile_map: TileMapLayer = $TileMapLayer
var cell_pos
const WALL = preload("res://Scenes/wall.tscn")
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body.


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	pass

func _input(event: InputEvent) -> void:
	if Input.is_action_just_pressed("enter_build"):
		
	if event is InputEventMouseButton and event.is_pressed():
		if event.button_index == MOUSE_BUTTON_LEFT:
			place(tile_map.local_to_map(get_global_mouse_position()))
			
func place(position):
	var wall_instance = WALL.instantiate()
	wall_instance.position = tile_map.map_to_local(position)
	add_child(wall_instance)

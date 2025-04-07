extends Node2D

# Reference to the TileMapLayer node, assigned when the scene is ready
@onready var tile_map: TileMapLayer = $TileMapLayer

# Stores the cell position where a wall will be placed
var cell_pos
@onready var label: Label = $Label

var is_on:bool = false

# Preload the wall scene to instantiate it when needed
const WALL = preload("res://Scenes/wall.tscn")

# Called when the node enters the scene tree for the first time
func _ready() -> void:
	label.vertical_alignment = VERTICAL_ALIGNMENT_TOP
	label.horizontal_alignment = HORIZONTAL_ALIGNMENT_CENTER
	pass # No initialization needed for now

# Called every frame. 'delta' is the elapsed time since the previous frame
func _process(delta: float) -> void:
	if Input.is_action_just_pressed("Build_mode"):
		print("Build Pressed")
		is_on = !is_on
		self.visible = is_on
		print(self.visible)
	pass # No per-frame logic required

# Handles user input events
func _input(event: InputEvent) -> void:
	
	# Check if the event is a mouse button press
	if event is InputEventMouseButton and is_on == true:
		# Check if the left mouse button was pressed
		if event.button_index == MOUSE_BUTTON_LEFT:
			# Convert the global mouse position to a tile map position and place a wall there
			place(tile_map.local_to_map(get_global_mouse_position()))

# Function to place a wall at a given tilemap position
func place(position):
	# Create a new instance of the wall scene
	var wall_instance = WALL.instantiate()
	print("Placed")
	# Convert tilemap coordinates to local world coordinates and set the wall's position
	wall_instance.position = tile_map.map_to_local(position)
	
	# Add the wall instance to the scene tree as a child of this node
	self.get_parent().get_parent().add_child(wall_instance)

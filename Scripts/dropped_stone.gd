extends StaticBody2D

# Exported speed variable (modifiable in the editor) that controls how fast interpolation occurs.
@export var speed: float = 2.0
@export var speedTwo : float =  0.1
# This value is used to gradually interpolate between two positions. It starts at 0 (beginning)
# and increases toward 1 (fully interpolated).
var interpolation_value_player = 0
var interpolation_value_position = 0

# Stores the object's starting global position.
var starting_position : Vector2
var player_start_pos : Vector2
var end_position :Vector2
# Will store the player's position if found.
var PlayerPos: Vector2

var in_final_pos:bool
var direction = (end_position - starting_position).normalized()
# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	pass # Replace with function body if needed.

# Get the Area2D node responsible for detecting overlapping bodies (e.g., the player).
@onready var PickupArea: Area2D = $Area2D

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	# Check if there are any bodies overlapping the PickupArea.
	if !PickupArea.has_overlapping_bodies():
		if (interpolation_value_player < 1):
					interpolation_value_position += delta * speed
					# Clamp the interpolation_value_player to 1 if it exceeds 1.
					if (interpolation_value_position > 1):
						interpolation_value_position = 1
						# Once fully interpolated (i.e., interpolation_value_player reaches 1), remove this node.
						self.queue_free()
					# Update the global transform's origin using linear interpolation between starting_position and PlayerPos.
					# The 'lerp' function takes two positions and a weight (interpolation_value_player) and returns a value that is
					# proportionally between them. When interpolation_value_player is 0, the result is starting_position.
					# When interpolation_value_player is 1, the result is PlayerPos.
					global_transform.origin = starting_position.lerp(end_position, interpolation_value_position)
					player_start_pos = global_transform.origin

	if PickupArea.get_overlapping_bodies():
		# Loop through all overlapping bodies.
		
		for body in PickupArea.get_overlapping_bodies():
			# If the body is a CharacterBody2D (e.g., the player), update PlayerPos with its global position.
			if body.name == "Player":
				PlayerPos = body.global_position
				# If interpolation_value_player is less than 1, gradually increase it based on delta time and speed.
				if (interpolation_value_player < 1):
					interpolation_value_player += delta * speed
					# Clamp the interpolation_value_player to 1 if it exceeds 1.
					if (interpolation_value_player > 1):
						interpolation_value_player = 1
						# Once fully interpolated (i.e., interpolation_value_player reaches 1), remove this node.
						add_stone(body)
						self.queue_free()
					# Update the global transform's origin using linear interpolation between starting_position and PlayerPos.
					# The 'lerp' function takes two positions and a weight (interpolation_value_player) and returns a value that is
					# proportionally between them. When interpolation_value_player is 0, the result is starting_position.
					# When interpolation_value_player is 1, the result is PlayerPos.
					global_transform.origin = player_start_pos.lerp(PlayerPos, interpolation_value_player)
			
func add_stone(body:CharacterBody2D):
	body.get_node("Inventory").stone+=1
	body.get_parent().get_node("UI").get_node("StoneLabel").text = "Stone: " + str(body.get_node("Inventory").stone)

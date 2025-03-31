extends StaticBody2D

# Exported speed variable (modifiable in the editor) that controls how fast interpolation occurs.
@export var speed: int = 1.9

# This value is used to gradually interpolate between two positions. It starts at 0 (beginning)
# and increases toward 1 (fully interpolated).
var interpolation_value = 0

# Stores the object's starting global position.
var starting_position

# Will store the player's position if found.
var PlayerPos: Vector2

# Called when the node enters the scene tree for the first time.
func _ready() -> void:
	# Record the starting position of the object.
	starting_position = self.global_position
	pass # Replace with function body if needed.

# Get the Area2D node responsible for detecting overlapping bodies (e.g., the player).
@onready var PickupArea: Area2D = $Area2D

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta: float) -> void:
	# Check if there are any bodies overlapping the PickupArea.
	if PickupArea.get_overlapping_bodies():
		# Loop through all overlapping bodies.
		for body in PickupArea.get_overlapping_bodies():
			# If the body is a CharacterBody2D (e.g., the player), update PlayerPos with its global position.
			if body is CharacterBody2D:
				PlayerPos = body.global_position
			else:
				# If the overlapping body is not the player, reset PlayerPos to the starting position.
				PlayerPos = starting_position
		
		# If interpolation_value is less than 1, gradually increase it based on delta time and speed.
		if (interpolation_value < 1):
			interpolation_value += delta * speed
			# Clamp the interpolation_value to 1 if it exceeds 1.
			if (interpolation_value > 1):
				interpolation_value = 1
				# Once fully interpolated (i.e., interpolation_value reaches 1), remove this node.
				self.queue_free()
			# Update the global transform's origin using linear interpolation between starting_position and PlayerPos.
			# The 'lerp' function takes two positions and a weight (interpolation_value) and returns a value that is
			# proportionally between them. When interpolation_value is 0, the result is starting_position.
			# When interpolation_value is 1, the result is PlayerPos.
			global_transform.origin = starting_position.lerp(PlayerPos, interpolation_value)

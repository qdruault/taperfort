extends KinematicBody2D

const SPEED : int = 200
const GRAVITY : int = 800

var velocity : Vector2 = Vector2()
var stateMachine;

func _ready():
	stateMachine = $AnimationTree.get("parameters/playback")

func _get_input():

	velocity = Vector2.ZERO
	
	var currentState = stateMachine.get_current_node();
	if (currentState == "Attack1"):
		return
	
	if Input.is_action_just_pressed("attack"):
		stateMachine.start("Attack1")
		return
		
	if Input.is_action_pressed("right"):
		velocity.x += 1
		$Sprite.scale.x = 2
	if Input.is_action_pressed("left"):
		velocity.x -= 1
		$Sprite.scale.x = -2		
	velocity = velocity.normalized() * SPEED
	
	if velocity.length() == 0:
		stateMachine.travel("Idle")
	if velocity.length() > 0:
		stateMachine.travel("Run")
		
func _physics_process(delta):
	_get_input()
	velocity = move_and_slide(velocity, Vector2.UP)	

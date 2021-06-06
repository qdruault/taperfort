extends KinematicBody2D

const SPEED : int = 200
const GRAVITY : int = 800

var velocity : Vector2 = Vector2()
var stateMachine;

export(Texture) var idleTextureSheet;
export(Texture) var runTextureSheet;
var hurtTextureSheet;
var attackTextureSheet;

export var playerIndex = 1;

func _ready():
	stateMachine = $AnimationTree.get("parameters/playback")
	
	# Texture Management
	if (playerIndex == 1):
		idleTextureSheet = load("res://sprites/Buck Borris/idle.png")
		runTextureSheet = load("res://sprites/Buck Borris/run.png")
		hurtTextureSheet = load("res://sprites/Buck Borris/damaged.png")
		attackTextureSheet = load("res://sprites/Buck Borris/attacks.png")
	else:
		idleTextureSheet = load("res://sprites/Buck Borris/p2_idle.png")
		runTextureSheet = load("res://sprites/Buck Borris/p2_run.png")
		hurtTextureSheet = load("res://sprites/Buck Borris/p2_damaged.png")
		attackTextureSheet = load("res://sprites/Buck Borris/p2_attacks.png")
	$Sprite.set_texture(idleTextureSheet)

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

func _animationInitialization(animationName):
	print("salut" + animationName)
	match animationName:
		"Attack":
			$Sprite.set_texture(attackTextureSheet)
		"Run":
			$Sprite.set_texture(runTextureSheet)
		"Idle":
			$Sprite.set_texture(idleTextureSheet)
		"Hurt":
			$Sprite.set_texture(hurtTextureSheet)

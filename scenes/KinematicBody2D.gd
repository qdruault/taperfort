extends KinematicBody2D

var speed : int = 200
var gravity : int = 800

var vel : Vector2 = Vector2()

onready var sprite : AnimatedSprite = get_node("AnimatedSprite")

func _physics_process(delta):
	
	sprite.play("Idle")
	
	vel.x = 0
	
	if Input.is_action_pressed("left"):
		vel.x -= speed 
	if Input.is_action_pressed("right"):
		vel.x += speed
		
	vel = move_and_slide(vel, Vector2.UP)	
	
	if vel.x < 0 :
		sprite.flip_h = true
	elif vel.x > 0 :
		sprite.flip_h = false
		
	if Input.is_action_just_pressed("attack"):
		sprite.play("Attack1")	

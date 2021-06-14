using Godot;
using System;

public class Player : KinematicBody2D
{
    #region Constantes
    private const int SPEED = 200;
    private const int GRAVITY = 800;
    #endregion

    #region Signaux

    #endregion

    #region Export

    [Export]
    private Texture _idleTextureSheet;
    [Export]
    private Texture _runTextureSheet;
    [Export]
    private Texture _hurtTextureSheet;
    [Export]
    private Texture _attackTextureSheet;
    [Export]
    private bool _isPlayerOne;
    [Export]
    private NodePath _otherPlayerNodePath;
    [Export]
    private string _rightInput;
    [Export]
    private string _downInput;
    [Export]
    private string _leftInput;
    [Export]
    private string _upInput;
    [Export]
    private string _lightInput;
    [Export]
    private string _mediumInput;
    [Export]
    private string _heavyInput;
    [Export]
    private string _specialInput;
    #endregion

    #region Attributs

    private Vector2 _velocity;
    private AnimationNodeStateMachinePlayback _stateMachine;
    private Sprite _sprite;
    private Player _otherPlayer;
    #endregion

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _stateMachine = (AnimationNodeStateMachinePlayback)GetNode("AnimationTree").Get("parameters/playback");
        _sprite = GetNode<Sprite>("Sprite");
        _otherPlayer = GetNode<Player>(_otherPlayerNodePath);

        if (_isPlayerOne)
        {
            _idleTextureSheet = (Texture)ResourceLoader.Load("res://sprites/Buck Borris/idle.png");
            _runTextureSheet = (Texture)ResourceLoader.Load("res://sprites/Buck Borris/run.png");
            _hurtTextureSheet = (Texture)ResourceLoader.Load("res://sprites/Buck Borris/damaged.png");
            _attackTextureSheet = (Texture)ResourceLoader.Load("res://sprites/Buck Borris/attacks.png");
        }
        else
        {
            _idleTextureSheet = (Texture)ResourceLoader.Load("res://sprites/Buck Borris/p2_idle.png");
            _runTextureSheet = (Texture)ResourceLoader.Load("res://sprites/Buck Borris/p2_run.png");
            _hurtTextureSheet = (Texture)ResourceLoader.Load("res://sprites/Buck Borris/p2_damaged.png");
            _attackTextureSheet = (Texture)ResourceLoader.Load("res://sprites/Buck Borris/p2_attacks.png");
        }

        _sprite.Texture = _idleTextureSheet;
    }

    private void GetInput()
    {
        _velocity = Vector2.Zero;

        string currentState = _stateMachine.GetCurrentNode();
        // Si on est dans l'anim de l'attaque on ne peut rien faire.
        if (currentState == "Attack1")
        {
            return;
        }

        #region Directions

        if (Input.IsActionPressed(_rightInput))
        {
            GD.Print(string.Format("{0} pressed", _rightInput));
            _velocity.x += 1;
        }
        else if (Input.IsActionPressed(_leftInput))
        {
            GD.Print(string.Format("{0} pressed", _leftInput));
            _velocity.x -= 1;
        }
        else if (Input.IsActionPressed(_upInput))
        {
            GD.Print(string.Format("{0} pressed", _upInput));
        }
        else if (Input.IsActionPressed(_downInput))
        {
            GD.Print(string.Format("{0} pressed", _downInput));
        }

        _velocity = _velocity.Normalized() * SPEED;

        if (_velocity.Length() == 0)
        {
            _stateMachine.Travel("Idle");
        }
        else if (_velocity.Length() > 0)
        {
            _stateMachine.Travel("Run");
        }

        #endregion

        #region Attaques

        if (Input.IsActionJustPressed(_lightInput))
        {
            GD.Print(string.Format("{0} pressed", _lightInput));
            _stateMachine.Start("Attack1");
            return;
        }
        else if (Input.IsActionJustPressed(_mediumInput))
        {
            GD.Print(string.Format("{0} pressed", _mediumInput));
        }
        else if (Input.IsActionJustPressed(_heavyInput))
        {
            GD.Print(string.Format("{0} pressed", _heavyInput));
        }
        else if (Input.IsActionJustPressed(_specialInput))
        {
            GD.Print(string.Format("{0} pressed", _specialInput));
        }

        #endregion
    }

    public override void _PhysicsProcess(float _delta)
    {
        GetInput();
        _velocity = MoveAndSlide(_velocity, Vector2.Up);
        // Pour que les 2 joueurs se regardent.
        _sprite.FlipH = !IsFacingRight();
    }

    public void InitAnim(string pAnimationName)
    {
        GD.Print("Anim: " + pAnimationName);
        
        switch (pAnimationName)
        {
            case "Attack":
                _sprite.Texture = _attackTextureSheet;
                break;
            case "Run":
                _sprite.Texture = _runTextureSheet;
                break;
            case "Hurt":
                _sprite.Texture = _hurtTextureSheet;
                break;
            default:
                _sprite.Texture = _idleTextureSheet;
                break;
        }
    }

    private bool IsFacingRight()
    {
        return _otherPlayer.Position.x > Position.x;
    }
}

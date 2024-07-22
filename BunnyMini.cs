using Godot;
using System;

public partial class BunnyMini : CharacterBody2D
{
	private AnimatedSprite2D _sprite;
	private Vector2 _direction;
	private float _speed = 15f;
	private float _radius;
	private Vector2 _movementAreaCenter;
	private Timer _movementTimer;
	private Random _random;
	
	private const float MinWaitTime = 1.0f;
	private const float MaxWaitTime = 2.5f;

	public override void _Ready()
	{
		_sprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_random = new Random();

		_radius = 200.0f; 
		_movementAreaCenter = GlobalPosition;

		_movementTimer = new Timer();
		_movementTimer.OneShot = false;
		_movementTimer.Timeout += UpdateMovementDirection;
		AddChild(_movementTimer);

		UpdateMovementDirection();
	}

	public override void _PhysicsProcess(double delta)
	{
		Velocity = _direction.Normalized() * _speed;

		if (GlobalPosition.DistanceTo(_movementAreaCenter) > _radius)
		{
			_direction = -_direction;
		}
		if (Velocity.X != 0)
		{
			_sprite.FlipH = Velocity.X < 0;
			_sprite.Play("move");
		}else{
			_sprite.Play("idle");
		}
		
		MoveAndSlide();
	}

	private void UpdateMovementDirection()
	{
		float randomValue = (float)_random.NextDouble();
		if (randomValue < 0.5f)
		{
			_direction = Vector2.Zero;
		}
		else
		{
			_direction = new Vector2((float)(_random.NextDouble() * 2 - 1), (float)(_random.NextDouble() * 2 - 1));
		}
		
		StartRandomTimer();
	}
	
	private void StartRandomTimer()
	{
		float waitTime = (float)(_random.NextDouble() * (MaxWaitTime - MinWaitTime) + MinWaitTime);
		_movementTimer.WaitTime = waitTime;
		_movementTimer.Start();
	}
}



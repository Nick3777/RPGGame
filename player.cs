using Godot;
using System;

public partial class player : CharacterBody2D
{
	private PlayerState playerState;
	public int maxHealth;
	public int health;
	public float speed;
	private Vector2 direction;
	private int dmg;
	
	public float _invincibilityDuration = 1.0f;
	public float _noHealingDuration = 2.0f;
	
	private float _invincibilityTimer = 0.0f;
	private float _noHealingTimer = 0.0f;
	
	private bool _isCured = false;
	private bool _isInvincible = false;
	public bool isAttacking = false;
	
	public String player_state;
	private AnimatedSprite2D animatedSprite2D;
	private HealthBar healthBar;
	private SwordAttack sword;
	private Vector2 swordPosition;
	private AnimationNodeStateMachinePlayback stateMachine;
	private AnimationTree animationTree;
	
	public override void _Ready()
	{
		playerState = GetNode<PlayerState>("/root/PlayerState");
		health = playerState.PlayerHealth;
		maxHealth = playerState.PlayerMaxHealth;
		speed = playerState.PlayerSpeed;
		dmg = playerState.PlayerDamage;
		direction = Vector2.Zero;
		
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		healthBar = GetNode<HealthBar>("CanvasLayer/HealthBar");
		healthBar.SetHealthBar(health, maxHealth);
		
		//CallDeferred(nameof(InitializeStateMachine));
		animationTree = GetNode<AnimationTree>("AnimationTree");
		stateMachine = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");
	}
	
	public override void _PhysicsProcess(double delta)
	{
		if (_isCured)
		{
			_noHealingTimer -= (float)delta;
			if (_noHealingTimer <= 0)
			{
				_isCured = false;    
			}
		}
		
		if (_isInvincible)
		{
			_invincibilityTimer -= (float)delta;
			if (_invincibilityTimer <= 0)
			{
				_isInvincible = false;
			}
		}
		
		direction = Input.GetVector("left", "right", "up", "down");
		if(Input.IsActionPressed("right") || Input.IsActionPressed("left")){
			direction.Y = 0;
			direction.X = Input.IsActionPressed("right") ? 1 : -1;
		}
		else if(Input.IsActionPressed("up") || Input.IsActionPressed("down")){
			direction.Y = Input.IsActionPressed("up") ? -1 : 1;
			direction.X = 0;
		}
			
		if (Input.IsActionPressed("attack") && playerState.swordPicked)
		{
			isAttacking = true;
			animationTree.Set("parameters/Attack/blend_position", animatedSprite2D.FlipH ? -1 : 1);
			stateMachine.Travel("Attack");
		}
		else
		{
			if (direction == Vector2.Zero)
			{
				stateMachine.Travel("Idle");
			}
			else
			{
				speed = Input.IsActionPressed("run") ? playerState.PlayerRunSpeed : playerState.PlayerSpeed;
				string animationState = Input.IsActionPressed("run") ? "Run" : "Move";
				animationTree.Set($"parameters/{animationState}/blend_position", direction);
				stateMachine.Travel(animationState);
			}
		}
	
		if(isAttacking) 
			Velocity = Vector2.Zero;
		else
			Velocity = direction * speed;
			
		MoveAndSlide();
	}

	
	
	public void take_dmg(int damage){
		if (_isInvincible) return;
		
		health -= damage;
		playerState.PlayerHealth = health;
		if(health <= 0){
			health = 0;
			handle_death();
		}
		healthBar.ChangeHealth(-damage);
		_isInvincible = true;
		_invincibilityTimer = _invincibilityDuration;
	}
	
	public void take_heal(int heal){
		if (_isCured) return;
		
		health += heal;
		playerState.PlayerHealth = health;
		if(health >= maxHealth){
			health = maxHealth;
			playerState.PlayerHealth = health;
		}
		healthBar.ChangeHealth(heal);
		
		_isCured = true;
		_noHealingTimer = _noHealingDuration;
	}
	
	public void handle_death(){
		playerState.PlayerHealth = 100;
		QueueFree();
		GetTree().ChangeSceneToFile("res://start_ui.tscn");
	}
	
	private void _on_area_2d_area_entered(Area2D area)
	{
		var groups = area.GetGroups();
		if (area.IsInGroup("Enemy"))
		{
			Node enemyNode = area.GetParent();
			if (enemyNode is BunnyMonster enemy)
			{
				enemy.TakeDmg(dmg); 
			}
		}
	}
}



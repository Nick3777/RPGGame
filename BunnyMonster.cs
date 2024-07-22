using Godot;
using System;

public partial class BunnyMonster : CharacterBody2D
{
	[Export] 
	public int dmgPoints = 15;
	
	public const float Speed = 70.0f;
	public int health = 100;
	public bool isDead = false;
	public bool player_in_area = false;
	public bool player_in_Damage_area = false;
	
	
	private AnimatedSprite2D animatedSprite2D;
	private Area2D area2D;
	private CollisionShape2D hitbox;
	private CollisionShape2D collisionShape2D;
	private Vector2 collisionShapeOffset = new Vector2(-4, 0);
	private Marker2D spawn;
	private player player;
	private Label label;
	private bool labelShown = false;
	private Timer labelTimer;
	private Label labelDmgTaken;
	private Timer labelDmgTakenTimer;
	
	public override void _Ready()
	{
		isDead = false;
		spawn = GetNode<Marker2D>("SpawnPoint");
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		area2D = GetNode<Area2D>("DetectArea");
		collisionShape2D = area2D.GetNode<CollisionShape2D>("CollisionShape2D");
		hitbox = GetNode<CollisionShape2D>("CollisionShape2D");
		spawn.Position = Position;
		
		label = GetNode<Control>("Labels").GetNode<Label>("Quote");
		label.Hide();
		labelTimer = GetNode<Timer>("labelTimer");
		
		labelDmgTaken = GetNode<Control>("Labels").GetNode<Label>("DmgTaken");
		labelDmgTakenTimer = GetNode<Timer>("LabelDmgTakenTimer");
	}

	public override void _PhysicsProcess(double delta)
	{	
		
		if (!isDead)
		{
			collisionShape2D.Disabled = false;
			if(player_in_area){
				 Vector2 p_direction = (player.Position - Position).Normalized();
				 Velocity = p_direction * Speed;
					if(player.Position.X > Position.X){
						hitbox.Position = collisionShapeOffset;
						animatedSprite2D.FlipH = false;
						animatedSprite2D.Play("move");
					}else{
						animatedSprite2D.FlipH = true;
						hitbox.Position = -collisionShapeOffset;
						animatedSprite2D.Play("move");
					}
				}
			else if(!player_in_area && (spawn.Position - Position).Length() > 1){
				Vector2 s_direction = (spawn.Position - Position).Normalized();
				Velocity = s_direction * Speed;
					if(spawn.Position.X > Position.X){
						hitbox.Position = collisionShapeOffset;
						animatedSprite2D.FlipH = false;
						animatedSprite2D.Play("move");
					}else{
						animatedSprite2D.FlipH = true;
						hitbox.Position = -collisionShapeOffset;
						animatedSprite2D.Play("move");
					}
			}else{
				Velocity = Vector2.Zero;
				animatedSprite2D.Play("idle");
			}
		}
		else
		{
			collisionShape2D.Disabled = true;
		}
		
		if(player_in_Damage_area){
			player.take_dmg(dmgPoints);
		}
		MoveAndSlide();
	}

	
	private void _on_detect_area_body_entered(Node2D body)
	{
		if(body.Name == "Player"){
			player_in_area = true;
			player = (player)body;
			_showLabel();
		}
	}


	private void _on_detect_area_body_exited(Node2D body)
	{
		if(body.Name == "Player"){
			player_in_area = false;
		}
	}
	
	private void _on_dmg_area_body_entered(Node2D body)
	{
		if(body.Name == "Player"){
			player_in_Damage_area = true;
		}
	}
	
	private void _on_dmg_area_body_exited(Node2D body)
	{
		if(body.Name == "Player"){
			player_in_Damage_area = false;
		}
	}
	
	private void _showLabel(){
		if(!labelShown){
			label.Show();		
			labelShown = true;
			labelTimer.Start();
		}
	}
	
	private void _on_label_timer_timeout()
	{
		label.Hide();
	}
	
	public void TakeDmg(int dmg){
		health -= dmg;
		
		labelDmgTaken.Text = "- " + dmg.ToString() + "HP";
		labelDmgTakenTimer.Start();
		
		if(health <= 0){
			QueueFree();
		}
	}
	
	private void _on_label_dmg_taken_timer_timeout()
	{
		labelDmgTaken.Text = " ";
	}
}




using Godot;
using System;

public partial class sword : Node2D
{
	public bool player_in_area = false;
	public AnimationPlayer animationPlayer;
	public PlayerState playerState;
	
	public override void _Ready()
	{
		playerState = GetNode<PlayerState>("/root/PlayerState");
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
	}

	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("pick") && player_in_area)
			animationPlayer.Play("pickup");
	}
	
	private void _on_pickable_area_body_entered(Node2D body)
	{    
		if (body.Name == "Player") 
		{
			player_in_area = true;
		}
	}

	private void _on_pickable_area_body_exited(Node2D body)
	{
		if (body.Name == "Player") 
		{
			player_in_area = false;
		}
	}
	
	private void _on_animation_player_animation_finished(StringName anim_name)
	{
		if(anim_name == "pickup" ){
			playerState.swordPicked = true;
			QueueFree();
		}
	}
}

using Godot;
using System;

public partial class HealingArea : Area2D
{
	private bool player_in_Healing_area = false;
	private player player;
	private int heal = 30;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(player_in_Healing_area){
			player.take_heal(heal);
		}
	}
	
	
	private void _on_body_entered(Node2D body)
	{
		GD.Print(body.Name);
		if(body.Name == "Player"){
			player_in_Healing_area = true;
			player = (player) body;
		}
	}


	private void _on_body_exited(Node2D body)
	{
		if(body.Name == "Player"){
			player_in_Healing_area = false;
		}
	}
}


using Godot;
using System;

public partial class sword : Node2D
{
	public bool player_in_area = false;
	private AnimatedSprite2D animatedSprite2D;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		 if (!animatedSprite2D.IsPlaying() || animatedSprite2D.Animation != "pick")
		{
			animatedSprite2D.Play("no_pick");
		}
		
		if(player_in_area){
			if(Input.IsActionJustPressed("pick")){
				animatedSprite2D.Play("pick");
			}
		}
	}
	
	private void _on_pickable_area_body_entered(Node2D body)
	{    
		if (body.Name == "Player") // Assicurati che il nodo del giocatore si chiami "Player"
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
	
	private void _on_animated_sprite_2d_animation_finished()
	{
		 if (animatedSprite2D.Animation == "pick")
			{
				QueueFree(); // Destroy the node
			}
	}
}


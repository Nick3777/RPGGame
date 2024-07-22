using Godot;
using System;

public partial class VillageScene : Node2D
{
	private bool _changeScene = false;
	private AnimationPlayer SceneTransitionAnimation;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		SceneTransitionAnimation = GetNode<AnimationPlayer>("SceneTransition/AnimationPlayer");
		var colorRect = SceneTransitionAnimation.GetParent().GetNode<ColorRect>("ColorRect");
		Color newColor = colorRect.Color;
		newColor.A = 255;
		colorRect.Color = newColor;
		
		SceneTransitionAnimation.Play("fade_out");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if(Input.IsActionJustPressed("pick") && _changeScene)
			{
				_changeScene = false;
				SceneTransitionAnimation.Play("fade_in");
			
				var timer = new Timer();
				timer.WaitTime = 0.4f;
				timer.OneShot = true; 
				
				timer.Timeout += OnTimeout;
				AddChild(timer); 
				timer.Start(); 
			}	
	}
	
	private void OnTimeout()
	{
		GetTree().ChangeSceneToFile("res://BunnyAreaScene.tscn");
	}
	
	private void _on_change_scenes_body_entered(Node2D body)
	{
		if(body.Name == "Player")
		{
			_changeScene = true; 
		}
	}
	
	private void _on_change_scenes_body_exited(Node2D body)
	{
		if(body.Name == "Player")
		{
			_changeScene = false; 
		}
	}
	
	private void _on_cartel_to_bunny_body_entered(Node2D body)
	{
		if(body.Name == "Player")
		{
			GetNode<Label>("CartelToBunny/ToBunnyLabel").Show();
		}
	}


	private void _on_cartel_to_bunny_body_exited(Node2D body)
	{
		GetNode<Label>("CartelToBunny/ToBunnyLabel").Hide();
	}
}



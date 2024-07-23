using Godot;
using System;

public partial class StartUI : Control
{
	private PackedScene SceneTransition;
	private Area2D areaPlay;

	public override void _Ready()
	{
		SceneTransition = GD.Load<PackedScene>("res://TransitionScene.tscn");
		areaPlay = GetNode<Area2D>("MarginContainer/VBoxContainer/PlayButton/Area2D");
	}


	public override void _Process(double delta)
	{
	}
	
	private void _OnButtonAreaEntered(String button)
	{
		AnimatedSprite2D animatedSprite2D= GetNode<AnimatedSprite2D>($"MarginContainer/VBoxContainer/{button}/AnimatedSprite2D");
		animatedSprite2D.Play("default");
		Color newColor = animatedSprite2D.Modulate;
		newColor = new Color(1, 1, 1);
		animatedSprite2D.Modulate = newColor;
		
	}

	private void _OnButtonAreaExited(String button)
	{
		AnimatedSprite2D animatedSprite2D= GetNode<AnimatedSprite2D>($"MarginContainer/VBoxContainer/{button}/AnimatedSprite2D");
		animatedSprite2D.Stop();
		Color newColor = animatedSprite2D.Modulate;
		newColor = new Color(0.8f, 0.8f, 0.8f);
		animatedSprite2D.Modulate = newColor;
	}

	private void _OnPlayButtonInputEvent(Node viewport, InputEvent @event, long shape_idx)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
			{
				var instance = SceneTransition.Instantiate();
				AddChild(instance);
				
				var SceneTransitionAnimation = GetNode<AnimationPlayer>("SceneTransition/AnimationPlayer");
				SceneTransitionAnimation.Play("fade_in");
					
				var timer = new Timer();
				timer.WaitTime = 0.4f;
				timer.OneShot = true; 
				
				timer.Timeout += OnTimeout;
				AddChild(timer); 
				timer.Start(); 
			}
		}
	}
	
	private void _OnQuitButtonInputEvent(Node viewport, InputEvent @event, long shape_idx)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.Pressed && mouseEvent.ButtonIndex == MouseButton.Left)
			{
				GetTree().Quit();
			}
		}
	}
	
	private void OnTimeout()
	{
		GetTree().ChangeSceneToFile("res://VillageScene.tscn");
	}
	
	private void _OnPlayButtonAreaEntered(){
		_OnButtonAreaEntered("PlayButton");
	}
	
	private void _OnPlayButtonAreaExited(){
		_OnButtonAreaExited("PlayButton");
	}
	
	private void _OnQuitButtonAreaEntered(){
		_OnButtonAreaEntered("QuitButton");
	}
	
	private void _OnQuitButtonAreaExited(){
		_OnButtonAreaExited("QuitButton");
	}
}


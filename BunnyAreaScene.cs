using Godot;
using System;
using System.Threading.Tasks;

public partial class BunnyAreaScene : Node2D
{
	private AnimationPlayer SceneTransitionAnimation;
	private bool _changeScene = false;
	public PackedScene miniBunnyScene;
	
	public override void _Ready()
	{
		SceneTransitionAnimation = GetNode<AnimationPlayer>("SceneTransition/AnimationPlayer");
		var colorRect = SceneTransitionAnimation.GetParent().GetNode<ColorRect>("ColorRect");
		Color newColor = colorRect.Color;
		newColor.A = 255;
		colorRect.Color = newColor;
		
		SceneTransitionAnimation.Play("fade_out");
		
		miniBunnyScene = GD.Load<PackedScene>("res://BunnyMini.tscn");
		
		Random random = new Random();
		int upperLimit = random.Next(10, 15);
		for (int i = 0; i < upperLimit; i++)
		{
			BunnyMini bunnyMini = miniBunnyScene.Instantiate<BunnyMini>();
			
			var miniBunnySpawn = GetNode<PathFollow2D>("MiniBunnyPath/MiniBunnySpawn");
			miniBunnySpawn.ProgressRatio = GD.Randf();
			
			bunnyMini.Position = miniBunnySpawn.Position;
			AddChild(bunnyMini);
		}
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
		GetTree().ChangeSceneToFile("res://VillageScene.tscn");
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
			GetNode<Label>("CartelToVillage/ToVillageLabel").Show();
		}
	}


	private void _on_cartel_to_bunny_body_exited(Node2D body)
	{
		GetNode<Label>("CartelToVillage/ToVillageLabel").Hide();
	}	
}

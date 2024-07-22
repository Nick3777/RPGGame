using Godot;
using System;

public partial class HealthBar : ProgressBar
{
	public Label label;
	private float _margin = 5.0f;
	private CanvasLayer _canvasLayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Trova il CanvasLayer
		_canvasLayer = GetParent().GetParent().GetNode<CanvasLayer>("CanvasLayer");
		label = GetNode<Label>("Label");

		// Posiziona la barra della salute
	}

	public override void _Process(double delta)
	{
		// Non è necessario fare nulla qui se la barra è già posizionata correttamente
	}
	
	public void SetHealthBar(int health, int maxHealth){
		MaxValue = maxHealth;
		Value = health;
		label.Text = health.ToString();
	}
	
	public void ChangeHealth(int newValue){
		Value += newValue;
		label.Text = Value.ToString();
	}
	
}

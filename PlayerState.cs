using Godot;
using System;

public partial class PlayerState : Node
{
	public int PlayerHealth { get; set; } = 100;
	public int PlayerMaxHealth { get; set; } = 150;
	public float PlayerSpeed { get; set; } = 45.0f;
	public float PlayerRunSpeed { get; set; } = 90.0f;
	public int PlayerDamage { get; set; } = 20;
	public bool swordPicked {get; set;} = false;

	public override void _Ready()
	{
		// Assicurati che questo nodo non venga eliminato quando cambi scena
		SetProcess(false);
	}
}


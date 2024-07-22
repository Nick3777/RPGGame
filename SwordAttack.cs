using Godot;
using System;

public partial class SwordAttack : Node2D
{
	private int dmg = 20;
	private AnimationPlayer animationPlayer;
	private Timer hideSwordTimer;
	private Timer attackWaitTime;
	private player player;
	private bool canAttack;
	
	public override void _Ready()
	{
		animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
		hideSwordTimer = GetNode<Timer>("HideSword");
		attackWaitTime = GetNode<Timer>("AttackWaitTime");
		
		canAttack = true;
		player = GetParent<player>();
		
		Hide();
	}

	public override void _Process(double delta)
	{		
		if(Input.IsActionJustPressed("attack") && canAttack){
			Show();
			hideSwordTimer.Stop();
			animationPlayer.Play("attack");
			Attack();
		}
		
		player.isAttacking = animationPlayer.IsPlaying();
		
	}
	
	private void _on_hit_box_area_entered(Area2D area)
	{
		var groups = area.GetGroups();
		if (area.IsInGroup("Enemy"))
		{
			Node enemyNode = area.GetParent();
			if(enemyNode is BunnyMonster enemy){
				enemy.TakeDmg(dmg);
			}
		}
	}
	
	private void Attack(){
		canAttack = false;
		attackWaitTime.Start();
		hideSwordTimer.Start();
	}
	
	private void OnSwordHideTimeout(){
		Hide();
	}
	
	private void OnAttackWaitTimetimeout(){
		canAttack = true;
	}
}


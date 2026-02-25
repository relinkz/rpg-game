using System;
using Godot;

public partial class Actor : Node2D
{
	[Export]
	public int Health { get; set; } = 100;
	[Export]
	public int Initiative { get; set; } = 10;
	[Export]
	public int[] basePhysicalDamage { get; set; } = { 1, 6 };

	Vector2 activeOffset = new Vector2(0, -50);
	Vector2 originalPosition;


	bool myTurn = false;

	[Signal]
	public delegate void TurnEndedEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		originalPosition = Position;
	}

	public override void _Process(double delta)
	{
		if (myTurn)
		{
			Position = originalPosition + activeOffset;
		}
		else
		{
			Position = originalPosition;
		}
	}

	public void attack(Actor target)
	{
		var dice = new Dice();
		int damage = dice.Roll(basePhysicalDamage[0], basePhysicalDamage[1]);
		target.TakeDamage(damage);
	}

	public void TakeDamage(int damage)
	{
		Health -= damage;
		GD.Print($"{Name} takes {damage} damage! Remaining health: {Health}");
		if (Health <= 0)
		{
			GD.Print($"{Name} has been defeated!");
			QueueFree();
		}
	}

	public virtual void OnTurnStart()
	{
		myTurn = true;
	}

	public virtual void EndTurn()
	{
		myTurn = false;
		EmitSignal(SignalName.TurnEnded);
	}
}

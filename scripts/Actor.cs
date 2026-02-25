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

	private Vector2 originalPosition;
	private Tween _idleTween;

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
	}

	public void attack(Actor target)
	{
		var dice = new Dice();
		int damage = dice.Roll(basePhysicalDamage[0], basePhysicalDamage[1]);

		// Create attack animation tween
		var tween = CreateTween();
		tween.SetTrans(Tween.TransitionType.Linear);

		Vector2 targetPosition = target.Position;
		float attackSpeed = 0.15f;  // Duration of lunge forward
		float returnSpeed = 0.15f;  // Duration of bounce back

		// Lunge toward target
		tween.TweenProperty(this, "position", targetPosition, attackSpeed);

		// Bounce back to original position
		tween.TweenProperty(this, "position", originalPosition, returnSpeed);

		// Deal damage after animation completes
		tween.TweenCallback(Callable.From(() => target.TakeDamage(damage)));
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

		// Start idle rotation animation
		_idleTween = CreateTween();
		_idleTween.SetTrans(Tween.TransitionType.Sine);
		_idleTween.SetLoops();  // Loop continuously

		float rotationAmount = 0.15f;  // Radians (~8.6 degrees)
		float rotationSpeed = 0.6f;    // Duration per rotation swing

		// Rotate right
		_idleTween.TweenProperty(this, "rotation", rotationAmount, rotationSpeed);

		// Rotate left
		_idleTween.TweenProperty(this, "rotation", -rotationAmount, rotationSpeed);
	}

	public virtual void EndTurn()
	{
		myTurn = false;

		// Stop idle animation and reset rotation
		if (_idleTween != null)
		{
			_idleTween.Kill();
		}
		Rotation = 0f;

		EmitSignal(SignalName.TurnEnded);
	}
}

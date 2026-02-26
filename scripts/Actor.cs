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
	private HealthBar _healthBar;

	bool myTurn = false;

	[Signal]
	public delegate void TurnEndedEventHandler();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		originalPosition = Position;

		// Initialize health bar
		var sprite = GetNode<Sprite2D>("Sprite2D");
		_healthBar = new HealthBar();
		AddChild(_healthBar);

		if (_healthBar != null && sprite != null)
		{
			_healthBar.Initialize(Health, sprite);
		}
	}

	public override void _Process(double delta)
	{
	}

	public void playPhysicalAttackAnimation(Actor target)
	{
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

	}

	public void TakeDamage(int damage)
	{
		Health -= damage;
		_healthBar?.UpdateHealth(Health);
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

		// Start idle swaying animation
		_idleTween = CreateTween();
		_idleTween.SetTrans(Tween.TransitionType.Sine);
		_idleTween.SetLoops();  // Loop continuously

		float swayAmount = 10f;  // Pixels to move up/down
		float swaySpeed = 0.8f;  // Duration per sway swing

		// Sway up
		_idleTween.TweenProperty(this, "position:y", originalPosition.Y - swayAmount, swaySpeed);

		// Sway down
		_idleTween.TweenProperty(this, "position:y", originalPosition.Y + swayAmount, swaySpeed);
	}

	public virtual void EndTurn()
	{
		myTurn = false;

		// Stop idle animation and reset position
		if (_idleTween != null)
		{
			_idleTween.Kill();
		}
		Position = originalPosition;

		EmitSignal(SignalName.TurnEnded);
	}
}

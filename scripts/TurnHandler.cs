using Godot;

using System.Collections.Generic;
public partial class TurnHandler : Node
{
	private Queue<Actor> _turnQueue;
	private Actor _currentEntity;

	private bool _activeCombat = false;
	private int _roundCounter = 0;
	// TurnHandler.cs
	private Label _roundLabel;

	private Button _attackButton;
	private Button _defendButton;
	private Button _endTurnButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_turnQueue = new Queue<Actor>();
		_roundLabel = GetNode<Label>("../Control/TurnLabel");

		// Get button references
		_attackButton = GetNode<Button>("../Control/ActionBar/BasicAbilities/Attack");
		_defendButton = GetNode<Button>("../Control/ActionBar/BasicAbilities/Defend");
		_endTurnButton = GetNode<Button>("../Control/ActionBar/BasicAbilities/EndTurn");

		// Connect button signals
		_attackButton.Pressed += OnAttackPressed;
		_defendButton.Pressed += OnDefendPressed;
		_endTurnButton.Pressed += OnEndTurnPressed;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Input now handled via button signals
	}

	private void OnAttackPressed()
	{
		if (!_activeCombat) return;

		var enemy = FindChild("enemy_*", owned: false);
		if (enemy != null && enemy is Actor target)
		{
			_currentEntity.attack(target);
		}
		else
		{
			GD.Print("No target found!");
		}
	}

	private void OnDefendPressed()
	{
		if (!_activeCombat) return;

		GD.Print($"{_currentEntity.Name} defends!");
		// TODO: Implement defend logic
	}

	private void OnEndTurnPressed()
	{
		if (!_activeCombat) return;

		_currentEntity?.EndTurn();
	}

	public void StartCombat()
	{
		_activeCombat = true;

		BuildInitiativeOrder();
		StartNextTurn();
	}

	private void BuildInitiativeOrder()
	{
		GD.Print("Fetching entities for new round...");
		var actors = new List<Actor>();
		foreach (var child in GetChildren())
		{
			if (child is Actor actor)
			{
				actors.Add(actor);
			}
		}

		// Sort by initiative (highest first)
		actors.Sort((a, b) => b.Initiative.CompareTo(a.Initiative));

		foreach (var actor in actors)
		{
			_turnQueue.Enqueue(actor);
		}

		GD.Print($"Round {_roundCounter++}");
		_roundLabel.Text = $"Round {_roundCounter}";

	}

	private void StartNextTurn()
	{
		if (_turnQueue.Count == 0)
		{
			BuildInitiativeOrder();
		}

		_currentEntity = _turnQueue.Dequeue();
		_currentEntity.TurnEnded += OnActorTurnEnded;
		_currentEntity.OnTurnStart();
		GD.Print($"{_currentEntity.Name}'s turn!");
	}

	private void OnActorTurnEnded()
	{
		_currentEntity.TurnEnded -= OnActorTurnEnded;
		StartNextTurn();
	}
}

using Godot;
using Godot.Collections;
using System.Collections.Generic;
public partial class TurnHandler : Node
{
	private Queue<Actor> _turnQueue;
	private Actor _currentEntity;

	private bool _activeCombat = false;
	private int _roundCounter = 0;

	private ActionBar _actionBar;
	private Label _turnLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_turnQueue = new Queue<Actor>();

		_actionBar = GetNode<ActionBar>("../TurnHandler/ActionBar");
		_turnLabel = GetNode<Label>("../TurnUiCtrl/TurnLabel");
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
			_currentEntity.playPhysicalAttackAnimation(target);
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
		var actors = new List<Actor>();
		var Enemies = GetTree().GetNodesInGroup("Enemy");
		Array<Enemy> enemyArray = new Array<Enemy>();
		Array<Player> playerArray = new Array<Player>();

		foreach (Node node in Enemies)
		{
			Actor typedNode = node as Actor;
			if (typedNode != null)
			{
				actors.Add(typedNode);
			}
			Enemy enemyNode = node as Enemy;
			if (enemyNode != null)
			{
				enemyArray.Add(enemyNode);
			}
		}

		var Players = GetTree().GetNodesInGroup("Player");
		foreach (Node node in Players)
		{
			Actor typedNode = node as Actor;
			if (typedNode != null)
			{
				actors.Add(typedNode);
			}
			Player playerNode = node as Player;
			if (playerNode != null)
			{
				playerArray.Add(playerNode);
			}
		}

		GD.Print("Found " + actors.Count + " actors for initiative order.");

		// Sort by initiative (highest first)
		actors.Sort((a, b) => b.Initiative.CompareTo(a.Initiative));
		foreach (var actor in actors)
		{
			_turnQueue.Enqueue(actor);
		}

		var targetSelector = BattleActors.GetInstance();
		targetSelector.setPlayers(playerArray);
		targetSelector.setEnemies(enemyArray);
		GD.Print("BattleActors singleton initialized with " + playerArray.Count + " players and " + enemyArray.Count + " enemies.");

		_roundCounter++;
		_turnLabel.Text = $"Round {_roundCounter}";
	}

	private void StartNextTurn()
	{
		if (_turnQueue.Count == 0)
		{
			BuildInitiativeOrder();
		}

		if (_turnQueue.Peek().Health <= 0)
		{
			GD.Print("Skipping defeated actor:");
			_turnQueue.Dequeue(); // Remove the defeated actor
			StartNextTurn(); // Try again
			return;
		}

		_currentEntity = _turnQueue.Dequeue();
		_currentEntity.TurnEnded += OnActorTurnEnded;
		_currentEntity.OnTurnStart();

		if (_currentEntity is Player)
		{
			_actionBar.PlayerAbilitySelect((Player)_currentEntity);
			_actionBar.Show();
		}
		else
		{
			_actionBar.Hide();
		}
	}

	private void OnActorTurnEnded()
	{
		_currentEntity.TurnEnded -= OnActorTurnEnded;

		StartNextTurn();
	}
}

using System;
using Godot;

public partial class Enemy : Actor
{
    // Enemy-specific properties go here
    [Export]
    public string EnemyType { get; set; } = "Slime";

    [Export]
    public float ThinkingTime { get; set; } = 1.5f;  // Seconds the enemy "thinks"

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        GD.Print($"{Name} ({EnemyType}) starts their turn");

        // Delay the action without blocking the game
        var timer = GetTree().CreateTimer(ThinkingTime);
        timer.Timeout += ExecuteAction;
    }

    private void ExecuteAction()
    {
        var target = GetTree().GetFirstNodeInGroup("Player") as Player;
        if (target != null)
        {
            Dice dice = new Dice();
            int damage = dice.Roll(basePhysicalDamage[0], basePhysicalDamage[1]);
            GD.Print($"{Name} attacks {target.Name} for {damage} damage!");
            target.TakeDamage(damage);
        }

        EndTurn();
    }

    public override void EndTurn()
    {
        GD.Print($"{Name} ends their turn");
        base.EndTurn();
        // Add enemy-specific end turn logic here
    }
}

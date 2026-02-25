using System;
using Godot;

public partial class Enemy : Actor
{
    // Enemy-specific properties go here
    [Export]
    public string EnemyType { get; set; } = "Slime";

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        GD.Print($"{Name} ({EnemyType}) starts their turn");
        // Add enemy-specific AI logic here
        // For example: Attack player, use special abilities, etc.
    }

    public override void EndTurn()
    {
        GD.Print($"{Name} ends their turn");
        base.EndTurn();
        // Add enemy-specific end turn logic here
    }
}

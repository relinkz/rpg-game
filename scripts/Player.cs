using System;
using Godot;

public partial class Player : Actor
{
    // Player-specific properties go here
    [Export]
    public string PlayerClass { get; set; } = "Rogue";

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        GD.Print($"{Name} ({PlayerClass}) starts their turn");
        // Add player-specific turn start logic here
    }

    public override void EndTurn()
    {
        GD.Print($"{Name} ends their turn");
        base.EndTurn();
        // Add player-specific end turn logic here
    }
}

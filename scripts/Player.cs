using Godot;

public partial class Player : Actor
{
    // Player-specific properties go here
    [Export]
    public string PlayerClass { get; set; } = "Rogue";

    public override void _Ready()
    {
        base._Ready();
        AddToGroup("Player");
    }
    public override void OnTurnStart()
    {
        base.OnTurnStart();
        GD.Print($"{Name} ({PlayerClass}) starts their turn");
    }

    public override void EndTurn()
    {
        GD.Print($"{Name} ends their turn");
        base.EndTurn();
        // Add player-specific end turn logic here
    }
}

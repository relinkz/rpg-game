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
    }

    public override void EndTurn()
    {
        base.EndTurn();
        // Add player-specific end turn logic here
    }
}

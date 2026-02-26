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

        // Delay the action without blocking the game
        var timer = GetTree().CreateTimer(ThinkingTime);
        timer.Timeout += ExecuteAction;
    }

    private void ExecuteAction()
    {
        var target = GetTree().GetFirstNodeInGroup("Player") as Player;
        if (target != null)
        {
            base.playPhysicalAttackAnimation(target);
            base.Abilities[0].Execute(this, target);  // Assuming the enemy uses the first ability for now
        }

        EndTurn();
    }

    public override void EndTurn()
    {
        base.EndTurn();
        // Add enemy-specific end turn logic here
    }
}

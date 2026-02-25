// ActionBar.cs
using Godot;

public partial class ActionBar : Control
{
    [Export] public PackedScene AbilityButtonScene;

    private HBoxContainer abilitiesContainer;
    private Player currentPlayer;

    public override void _Ready()
    {
        abilitiesContainer = GetNode<HBoxContainer>(
            "Panel/MarginContainer/HBoxContainer"
        );
    }

    public void SetCharacter(Player player)
    {
        currentPlayer = player;

        // Clear old buttons
        foreach (Node child in abilitiesContainer.GetChildren())
            child.QueueFree();

        if (currentPlayer == null)
            return;

        // Create buttons dynamically
        foreach (var ability in currentPlayer.Abilities)
        {
            var button = AbilityButtonScene.Instantiate<AbilityButton>();
            abilitiesContainer.AddChild(button);
            button.Setup(ability, currentPlayer);
        }
    }
}
using Godot;

public partial class AbilityButton : Button
{
    private AbilityData ability;
    private Actor owner;
    private Actor _target;

    public void Setup(AbilityData abilityData, Actor player)
    {
        ability = abilityData;
        owner = player;

        Icon = ability.Icon;
        TooltipText = ability.Tooltip;

        _target = GetNode<Actor>("/root/BattleScene/TurnHandler/enemy_slime"); // add targeting logic
    }

    private void OnPressed()
    {
        if (ability != null)
        {
            ability.Execute(owner, _target); // Targeting logic can be added here
        }
    }

    private void OnMouseEntered()
    {
        // show tooltip or highlight button
    }

    public override void _Ready()
    {
        Pressed += OnPressed;
    }

    public void CreateActionbarButton(AbilityData abilityData, Player player)
    {
        var button = new AbilityButton();
        button.Setup(abilityData, player);
        GetParent().AddChild(button);
    }
}
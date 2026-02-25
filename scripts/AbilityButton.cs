using Godot;

public partial class AbilityButton : Button
{
    private AbilityData ability;
    private Player owner;

    public void Setup(AbilityData abilityData, Player player)
    {
        ability = abilityData;
        owner = player;

        Text = ability.Name;
        Icon = ability.Icon;
        TooltipText = ability.Tooltip;
    }

    private void OnPressed()
    {
        if (ability != null)
        {
            ability.Execute(owner, null); // Targeting logic can be added here
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
using Godot;

public partial class AbilityButton : Button
{
    private AbilityData ability;
    private Actor owner;

    public void Setup(AbilityData abilityData, Actor player)
    {
        ability = abilityData;
        owner = player;

        Icon = ability.Icon;
        TooltipText = ability.Tooltip;
    }

    private void OnPressed()
    {
        var targetSelector = BattleActors.GetInstance();
        Actor target = targetSelector.getRandomEnemy();

        if (ability != null)
        {
            switch (ability.TargetType)
            {
                case TargetType.singleEnemy:
                    // Targeting logic for single enemy can be added here
                    target = targetSelector.getCurrentTarget();
                    GD.Print("hello?");
                    break;
                case TargetType.allEnemies:
                    // Targeting logic for all enemies can be added here
                    break;
                case TargetType.singleAlly:
                    // Targeting logic for ally can be added here
                    break;
                case TargetType.allAllies:
                    // Targeting logic for all allies can be added here
                    break;
                case TargetType.allActors:
                    break;
                case TargetType.self:
                    target = owner; // Target self
                    break;
            }

            ability.Execute(owner, target); // Targeting logic can be added here
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
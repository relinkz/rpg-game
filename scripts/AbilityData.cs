using Godot;

public enum DamageType
{
    Physical,
    Magical,
    True
}

[GlobalClass]
public partial class AbilityData : Resource
{
    [Export]
    public string Name { get; set; } = "Default Ability";
    [Export]
    public int[] Damage { get; set; } = { 1, 4 }; // which dice to roll for damage (e.g., 1d4)

    [Export]
    public DamageType Type = DamageType.Physical;

    [Export]
    public int[] Healing { get; set; } = { 0, 0 }; // which dice to roll for healing (e.g., 0d0 for no healing) 
    [Export]
    public int[] DodgeChance { get; set; } = { 0, 0, 0 }; // which dice to roll for dodge chance (e.g., 0d0 for no dodge), last digit is nr of turn buff last
    [Export]
    public int[] Armor { get; set; } = { 0, 0 }; // first digit is armor value, second is nr of turn buff last

    [Export]
    public string Tooltip { get; set; } = "Additional information about the ability goes here.";
    [Export]
    public Texture2D Icon;

    public virtual void Execute(Actor user, Actor target)
    {

        Dice dice = new Dice();
        int damage = dice.Roll(Damage[0], Damage[1]);
        GD.Print($"{user.Name} used {Name}, dealing {damage} ({Damage[0]}d{Damage[1]}) {Type} damage to {target.Name}!");
        user.playPhysicalAttackAnimation(target);
        target.TakeDamage(damage);
        user.EndTurn();
    }
}
using Godot;
public struct Dice
{
    public int Roll(int low, int high)
    {
        GD.Randomize();
        int roll = (int)(GD.Randi() % (high) + low);
        return roll;
    }
}
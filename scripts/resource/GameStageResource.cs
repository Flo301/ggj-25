using Godot;

[GlobalClass]
public partial class GameStageResource : Resource
{
    [Export]
    public int EndsAtRound = 0;
    [Export]
    public int RequiredPoints = 0;
}
using Godot;
using System;

public partial class GridItem : Node2D
{
	[Export]
	public float MultiplierIncreasePerRound { get; protected set; } = 0.0f;
	[Export]
	public int BasePointsOnTrigger { get; protected set; } = 1;
	[Export]
	public float Multiplier { get; protected set; } = 1f;
	[Export]
	public bool ConsumeBubble { get; protected set; } = false;
	[Export]
	public ERarity Rarity { get; protected set; } = ERarity.COMMON;
	[Export]
	public string Description { get; protected set; } = "";

    // Called when the node enters the scene tree for the first time.
    public override void _EnterTree()
    {
		ItemManager.Instance.AddItem(this);
		base._EnterTree();
	}

    public override void _ExitTree()
    {
		ItemManager.Instance.RemoveItem(this);
        base._ExitTree();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
	}

	public virtual void Trigger(Bubble bubble)
	{
		var points = (int)(BasePointsOnTrigger * Multiplier);
		GameManager.Instance.AddPoints(points);

		//spawn point label
		var obj = new FlyingPointsLabel();
		GetWindow().AddChild(obj);
		obj.Init(points, GlobalPosition);

		//consume bubble
		if (ConsumeBubble)
		{
			bubble.Destroy();
		}

		//Add velocity to bubble
		//bubble.AngularVelocity = 
	}

	public virtual void OnRoundEnd()
	{
		Multiplier += MultiplierIncreasePerRound;
	}
}

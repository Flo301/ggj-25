using Godot;
using System;

public partial class GridItem : Node2D
{
	[Export]
	protected float MultiplierIncreasePerRound = 0.0f;
	[Export]
	protected int BasePointsOnTrigger = 1;
	[Export]
	protected float Multiplier = 1f;
	[Export]
	protected bool ConsumeBubble = false;
	[Export]
	protected ERarity Rarity = ERarity.COMMON;
	[Export]
	protected string Description = "";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
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
			bubble.QueueFree();
		}

		//Add velocity to bubble
		//bubble.AngularVelocity = 
	}

	public virtual void OnRoundEnd()
	{
		Multiplier += MultiplierIncreasePerRound;
	}
}

using Godot;
using System;

public partial class GridItem : Node2D
{
	public int BasePointsOnTrigger = 1;
	public float Multiplier = 1f;

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

		//Add velocity to bubble
		//bubble.AngularVelocity = 
	}
}

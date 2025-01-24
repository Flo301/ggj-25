using Godot;
using System;

public partial class GridItem : Node2D
{
	public int BasePointsOnTrigger = 1;

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
		GameManager.Instance.AddPoints(BasePointsOnTrigger);
		//Add velocity to bubble
		//bubble.AngularVelocity = 
	}	
}

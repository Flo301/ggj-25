using Godot;
using System;

public partial class Bubble : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnCollide(Node node)
	{
		GD.Print(Name + " collide with " + node.Name);
		var item = node as GridItem;
		if(item != null)
		{
			item.Trigger(this);
		}
	}
}

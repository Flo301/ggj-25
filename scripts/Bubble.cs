using Godot;
using System;

public partial class Bubble : RigidBody2D
{
	private GridItem LastCollide = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BubbleManager.Instance.AddBubble(this);
	}

	public override void _PhysicsProcess(double delta)
	{
		var randomXForce = (float)Math.Sin(Time.GetTicksMsec() / 1000f);
		LinearVelocity = LinearVelocity + Vector2.Right * randomXForce * 5f * (float)delta;
		base._PhysicsProcess(delta);
	}

	private void OnCollide(Node node)
	{
		//GD.Print(Name + " collide with " + node.Name);
		var item = node as GridItem;
		if (item != null && LastCollide != item)
		{
			LastCollide = item;
			item.Trigger(this);
		}
	}

	public void Destroy()
	{
		BubbleManager.Instance.RemoveBubble(this);
		QueueFree();
	}
}

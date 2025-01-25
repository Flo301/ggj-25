using Godot;
using System;

public partial class Bubble : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BubbleManager.Instance.AddBubble(this);
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

	public void Destroy()
	{
		BubbleManager.Instance.RemoveBubble(this);
		QueueFree();
	}
}

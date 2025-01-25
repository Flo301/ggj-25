using Godot;
using System;

public partial class Bubble : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		BubbleManager.Instance.AddBubble(this);
	}

    public override void _Process(double delta)
    {
		//ToDo: Delete bubble when reaching deadzone UwU 03:19

        base._Process(delta);
    }

    private void OnCollide(Node node)
	{
		//GD.Print(Name + " collide with " + node.Name);
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

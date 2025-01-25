using Godot;
using System;

public partial class GridItem : Node2D
{
	//ATTRIBUTES
	[Export]
	public float MultiplierIncreasePerRound { get; protected set; } = 0.0f;
	[Export]
	public int BasePointsOnTrigger { get; protected set; } = 1;
	[Export]
	public float Multiplier { get; protected set; } = 1f;
	[Export]
	public bool ConsumeBubble { get; protected set; } = false;
	[Export]
	public PackedScene SpawnWhenTriggert { get; protected set; } = null;
	[Export]
	public float SpawnForce { get; protected set; } = 30f;
	[Export]
	public float CooldownInSeconds { get; protected set; } = 0f;

	//INFO-DISPLAY
	[Export]
	public ERarity Rarity { get; protected set; } = ERarity.COMMON;
	[Export]
	public string Description { get; protected set; } = "";

	//PROPERTIES
	private float CurrentCooldown = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ItemManager.Instance.AddItem(this);
		base._Ready();
	}

	public override void _ExitTree()
	{
		ItemManager.Instance.RemoveItem(this);
		base._ExitTree();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (CurrentCooldown > 0)
		{
			CurrentCooldown -= (float)delta;
			if (CurrentCooldown <= 0)
			{
				//ToDo: mark as active
			}
		}
	}

	public virtual void Trigger(Bubble bubble)
	{
		if (CurrentCooldown > 0f) return;

		CurrentCooldown = CooldownInSeconds;
		if (CurrentCooldown > 0f)
		{
			//ToDo: mark as inactive
		}

		//create points
		var points = (int)(BasePointsOnTrigger * Multiplier);
		GameManager.Instance.AddPoints(points);

		//spawn point label
		var pointLabel = new FlyingPointsLabel();
		GetWindow().AddChild(pointLabel);
		pointLabel.Init(points, GlobalPosition);

		//consume bubble
		if (ConsumeBubble)
		{
			bubble.Destroy();
		}

		//spawn item
		if (SpawnWhenTriggert != null)
		{
			var newNode = SpawnWhenTriggert.Instantiate<Node2D>();
			GetWindow().CallDeferred("add_child", newNode);
			newNode.GlobalPosition = GlobalPosition + Vector2.Down * 50f;

			var rigidBody = newNode.GetNodeOrNull<RigidBody2D>("");
			if (rigidBody != null)
			{
				var rng = new RandomNumberGenerator();
				rigidBody.LinearVelocity = new Vector2(rng.RandfRange(-1f, 1f), 1) * SpawnForce;
			}
		}

		//Add velocity to bubble
		//bubble.AngularVelocity = 
	}

	public virtual void OnRoundEnd()
	{
		Multiplier += MultiplierIncreasePerRound;
	}
}

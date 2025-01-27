using Godot;
using Godot.NativeInterop;
using System;

public partial class GridItem : Node2D
{
	//ATTRIBUTES
	[Export]
	public float MultiplierIncreasePerRound { get; protected set; } = 0.0f;
	[Export]
	public int BasePointsOnTrigger { get; protected set; } = 1;
	[Export]
	private float multiplier = 1f;
	public float Multiplier
	{
		get => multiplier;
		set
		{
			multiplier = value;

			if (value != 1)
			{
				if (MultiplierScene == null)
				{
					GD.PrintErr("missing MultiplierScene for " + Name);
					return;
				}

				//spawn label if required
				if (MultiplierLabel == null)
				{
					var obj = MultiplierScene.Instantiate<Control>();
					AddChild(obj);
					MultiplierLabel = obj.GetNode<Label>("%Label");
				}
				MultiplierLabel.Text = "x" + Math.Round(value, 1);
			}
		}
	}

	[Export]
	public bool ConsumeBubble { get; protected set; } = false;
	[Export]
	public PackedScene SpawnWhenTriggert { get; protected set; } = null;
	[Export]
	public float SpawnForce { get; protected set; } = 30f;
	[Export]
	public float CooldownInSeconds { get; protected set; } = 0f;
	[Export]
	public bool ExtraBounce { get; protected set; } = false;

	//INFO-DISPLAY
	[Export]
	public ERarity Rarity { get; protected set; } = ERarity.COMMON;
	[Export]
	public string Description { get; protected set; } = "";
	[Export]
	//ToDo: this should maybe be a global resource
	private PackedScene MultiplierScene;

	//PROPERTIES
	private float CurrentCooldown = 0;
	private AudioStreamPlayer2D TriggerSoundPlayer;
	private Label MultiplierLabel;
	private Sprite2D Sprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ItemManager.Instance.AddItem(this);
		TriggerSoundPlayer = GetNode<AudioStreamPlayer2D>("%TriggerSound");
		Sprite = GetNodeOrNull<Sprite2D>("Sprite2D");

		base._Ready();
	}

	public override void _ExitTree()
	{
		if (this.IsQueuedForDeletion())
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
				//mark as active
				Sprite.Modulate = Colors.White;
			}
		}
	}

	public virtual void Trigger(Bubble bubble)
	{
		if (TriggerSoundPlayer != null)
		{
			TriggerSoundPlayer.Play();
		}

		if (CurrentCooldown > 0f) return;

		CurrentCooldown = CooldownInSeconds;
		if (CurrentCooldown > 0f)
		{
			//mark as inactive
			Sprite.Modulate = Sprite.Modulate.Darkened(0.4f);
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

		if (ExtraBounce)
		{
			//Add velocity to bubble
			bubble.SetAxisVelocity(Vector2.Down * 300);
			bubble.CollisionMask = 0;
			GetTree().CreateTimer(1.3f).Timeout += () => { bubble.CollisionMask = 1; };
		}
	}

	public virtual void OnRoundEnd()
	{
		Multiplier += MultiplierIncreasePerRound;
	}
}

using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class BubbleManager : Node
{
	#region Singleton
	static public BubbleManager Instance { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Instance != null)
		{
			//ToDo: this makes problems by reload game :(
			GD.PrintErr("Singleton already instantiated");
		}
		Instance = this;
	}
	#endregion

	[Export]
	private int BubbleAmountPerRound = 1;
	[Export]
	public PackedScene BubbleScene;
	[Export]
	private Node2D Arena;

	private List<Bubble> ActiveBubbles = new List<Bubble>();

	private Vector2 GetBubbleSpawnpoint()
	{
		var spawnLine = Arena?.GetNodeOrNull<Line2D>("Spawn");

		if (spawnLine == null || spawnLine.Points.Count() < 2)
		{
			//Fallback
			return new Vector2(new Random().Next(0, 900), 600);
		}

		return spawnLine.Points[0].Lerp(spawnLine.Points[1], GD.Randf());
	}

	public void SpawnBubbles()
	{
		//Spawn bubble at random place
		for (int i = 0; i < BubbleAmountPerRound; i++)
		{
			Bubble bubble = BubbleScene.Instantiate<Bubble>();
			//ToDo: Get better grid based random spawnpoint (don't use the same twice)
			bubble.GlobalPosition = GetBubbleSpawnpoint();
			AddChild(bubble);
		}
	}

	public void AddBubble(Bubble bubble)
	{
		if (!ActiveBubbles.Contains(bubble))
			ActiveBubbles.Add(bubble);
	}

	public void RemoveBubble(Bubble bubble)
	{
		ActiveBubbles.Remove(bubble);
		GD.Print("ActiveBubbleCount: " + ActiveBubbles.Count);
		if (ActiveBubbles.Count == 0)
		{
			GameManager.Instance.OnRoundEnd();
		}
	}
}

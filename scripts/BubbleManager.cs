using Godot;
using System;
using System.Collections.Generic;

public partial class BubbleManager : Node
{
	#region Singleton
	static public BubbleManager Instance { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Instance != null)
		{
			throw new Exception("Singleton already instantiated");
		}
		Instance = this;
	}
	#endregion

	[Export]
	private int BubbleAmountPerRound = 1;
	[Export]
	public PackedScene BubbleScene;

	private List<Bubble> ActiveBubbles = new List<Bubble>();

	public void SpawnBubbles()
	{
		//Spawn bubble at random place
		for (int i = 0; i < BubbleAmountPerRound; i++)
		{
			Bubble bubble = BubbleScene.Instantiate<Bubble>();
			//ToDo: Get better grid based random spawnpoint (don't use the same twice)
			bubble.GlobalPosition = new Vector2(new Random().Next(0, 900), 600);
			AddBubble(bubble);
			AddChild(bubble);
		}
	}

	public void AddBubble(Bubble bubble)
	{
		ActiveBubbles.Add(bubble);
	}

	public void RemoveBubble(Bubble bubble)
	{
		ActiveBubbles.Remove(bubble);
		if (ActiveBubbles.Count == 0)
		{
			GameManager.Instance.OnRoundEnd();
		}
	}
}

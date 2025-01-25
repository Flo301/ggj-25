using Godot;
using System;
using System.Collections.Generic;

public partial class GameManager : Node
{
	#region Singleton
	static public GameManager Instance { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Instance != null)
		{
			throw new Exception("Singleton already instantiated");
		}
		Instance = this;

		PointLabel = GetNode<Label>("%PointLabel");
		AddPoints(0);
		
		//ToDo: Start with Random Item
		SpawnBubbles();
		//GetRandomItem();
	}
	#endregion

	//SETTINGS
	private int BubbleAmountPerRound = 1;
	[Export]
	private PackedScene BubbleScene;

	//NODES
	private Label PointLabel;

	//PROPERTIES
	private EGameState State = EGameState.PlaceItem;
	private int Points = 0;
	private int Round = 0;
	private List<Bubble> ActiveBubbles = new List<Bubble>();
	private List<GridItem> ActiveItems = new List<GridItem>();

	public void GetRandomItem()
	{
		//ToDo: Roll random items
		//ToDo: Open item selection
	}

	public void OnItemPlace(GridItem item)
	{
		ActiveItems.Add(item);

		//ToDo: check if further items musst be choosen with item selection
		SpawnBubbles();
	}

	public void SpawnBubbles()
	{
		State = EGameState.PlayBubble;

		//Spawn bubble at random place
		for (int i = 0; i < BubbleAmountPerRound; i++)
		{
			Bubble bubble = BubbleScene.Instantiate<Bubble>();
			//ToDo: Get better grid based random spawnpoint (don't use the same twice)
			bubble.GlobalPosition = new Vector2(new Random().Next(0, 900), 150);
			ActiveBubbles.Add(bubble);
			AddChild(bubble);
		}
	}

	public void OnBubbleRemove(Bubble bubble)
	{
		ActiveBubbles.Remove(bubble);
		if (ActiveBubbles.Count == 0)
		{
			OnRoundEnd();
		}
	}

	private void OnRoundEnd()
	{
		Round++;
		ActiveItems.ForEach(x => x.OnRoundEnd());
		GetRandomItem();
	}

	public void AddPoints(int amount)
	{
		Points += amount;
		PointLabel.Text = Points + "P";
	}

	enum EGameState
	{
		PlaceItem,
		PlayBubble
	}
}

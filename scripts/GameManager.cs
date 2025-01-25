using Godot;
using System;

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
		BubbleManager.Instance.SpawnBubbles();
		ItemManager.Instance.GetRandomItem();
	}
	#endregion

	//NODES
	private Label PointLabel;

	//PROPERTIES
	private bool IsRoundActive = false;
	private int Points = 0;
	private int Round = 0;

	public void StartNextRound()
	{
		IsRoundActive = true;
		BubbleManager.Instance.SpawnBubbles();
	}

	public void OnRoundEnd()
	{
		Round++;
		IsRoundActive = false;
		ItemManager.Instance.OnRoundEnd();
		ItemManager.Instance.GetRandomItem();
	}

	public void AddPoints(int amount)
	{
		Points += amount;
		PointLabel.Text = Points + "P";
	}
}

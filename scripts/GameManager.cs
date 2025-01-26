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
		RoundLabel = GetNode<Label>("%RoundLabel");
		OnStartSoundPlayer = GetNode<AudioStreamPlayer2D>("%RoundStartPlayer");
		Round = 0;
		AddPoints(0);

		ItemManager.Instance.GetRandomItem();
	}
	#endregion

	//NODES
	private Label PointLabel;
	private Label RoundLabel;
	private AudioStreamPlayer2D OnStartSoundPlayer;

	//PROPERTIES
	private bool IsRoundActive = false;
	private int Points = 0;
	private int round = 0;
	private int Round { get => round; set { RoundLabel.Text = "Round " + value; round = value; } }
	private double Stage = 0;
	private int StagePointsBase = 450;

	public void StartNextRound()
	{
		GD.Print("OnRoundStart");
		Round++;
		IsRoundActive = true;
		BubbleManager.Instance.SpawnBubbles();
		OnStartSoundPlayer.Play();
	}

	public void OnRoundEnd()
	{
		GD.Print("OnRoundEnd");
		IsRoundActive = false;
		if(Round % 5 != 0)
		{
			ItemManager.Instance.OnRoundEnd();
			ItemManager.Instance.GetRandomItem();
			return;
		}
		else if(GetCurrentStageGoal() <= Points)
		{
			Stage++;
			//gerade nur ein workaround um die punkteanzeige zu aktualisieren
			AddPoints(0);
			
			ItemManager.Instance.OnRoundEnd();
			// hinzufügen das die anzahl an Items und Upgrades Variabel ist GetRandomItem => GetRandomCards(items: X, upgrades: Y)
			ItemManager.Instance.GetRandomItem();
			return;
		}
		GD.Print("LOST");
	}

	public void AddPoints(int amount)
	{
		Points += amount;
		PointLabel.Text = Points + "P / " + GetCurrentStageGoal() + "P";
	}
	
	public double GetCurrentStageGoal()
	{
		switch (Stage)
		{
			case >= 5:
				return (StagePointsBase + (StagePointsBase * Stage) * Stage + (75 * Stage) * Stage );
			case >= 2:
				return (Math.Floor(StagePointsBase + (StagePointsBase * Stage) * (0.75 * Stage) + (75 * Stage) * (0.75 * Stage) ));
			default:
				return (StagePointsBase + (StagePointsBase * Stage) + (75 * Stage) );
		}
	}
}

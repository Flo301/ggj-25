using Godot;
using System;
using System.Linq;

public partial class GameManager : Node
{
	#region Singleton
	static public GameManager Instance { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Instance != null)
		{
			//ToDo: this makes problems by reload game :(
			GD.PrintErr("Singleton already instantiated");
		}
		Instance = this;

		PointLabel = GetNode<Label>("%PointLabel");
		RoundLabel = GetNode<Label>("%RoundLabel");
		StageLabel = GetNode<Label>("%StageLabel");
		OnStartSoundPlayer = GetNode<AudioStreamPlayer2D>("%RoundStartPlayer");
		Round = 1;
		AddPoints(0);

		RoundLabel.Text = (CurrentStage.EndsAtRound - Round) + " Round/s left";
		ItemManager.Instance.GetRandomItem();
	}
	#endregion

	//Popups
	[Export]
	private PackedScene WinLoosePopup;
	[Export]
	private PackedScene StageIntroPopup;

	//Config
	[Export]
	private GameStageResource[] GameStages;

	//NODES
	private Label PointLabel;
	private Label RoundLabel;
	private Label StageLabel;
	private AudioStreamPlayer2D OnStartSoundPlayer;

	//PROPERTIES
	private bool IsRoundActive = false;
	private int Points;
	private int round;
	private int Round { get => round; set { round = value; } }
	public int Stage { get; private set; } = 1;
	public GameStageResource CurrentStage => Stage <= GameStages.Count() ? GameStages[Stage - 1] : null;

	public void StartNextRound()
	{
		GD.Print("OnRoundStart");
		IsRoundActive = true;
		BubbleManager.Instance.SpawnBubbles();
		OnStartSoundPlayer.Play();
	}

	public void OnRoundEnd()
	{
		GD.Print("OnRoundEnd");
		IsRoundActive = false;

		//Stage transition handling
		if (CurrentStage.EndsAtRound == Round)
		{
			if (CurrentStage.RequiredPoints > Points)
			{
				PopupWinLoose PopupInstance = WinLoosePopup.Instantiate<PopupWinLoose>();
				PopupInstance.Win = false;
				AddChild(PopupInstance);
				//ToDo: LOST
				GD.Print("LOST");
				return;
			}
			Stage++;
			StageLabel.Text = "Stage " + Stage;

			if (CurrentStage == null)
			{
				PopupWinLoose PopupInstance = WinLoosePopup.Instantiate<PopupWinLoose>();
				PopupInstance.Win = true;
				AddChild(PopupInstance);
				//ToDo: GAME END
				GD.Print("GAME END");
				return;
			}
			//spawn popup
			PopupStageIntro StagePopupInstance = StageIntroPopup.Instantiate<PopupStageIntro>();
			StagePopupInstance.StageTitleString = "Welcome to Stage " + Stage;
			StagePopupInstance.StageMessageString = "You have to hit the Goal of " + CurrentStage.RequiredPoints + "P!\nYou have until Round: " + CurrentStage.EndsAtRound;
			ItemManager.Instance.AddChild(StagePopupInstance);

			RoundLabel.Text = (CurrentStage.EndsAtRound - Round) + "   Round/s left";

			ItemManager.Instance.OnRoundEnd();

			AddPoints(-Points); //Reset points
			return;
		}

		ItemManager.Instance.OnRoundEnd();
		ItemManager.Instance.GetRandomItem();

		Round++;
	}

	public void AddPoints(int amount)
	{
		Points += amount;
		PointLabel.Text = Points + "P / " + CurrentStage?.RequiredPoints + "P";
	}

	public void RestartGame()
	{
		//Restart Game
		GetTree().ReloadCurrentScene();
		//Singletons are being Reinstantiated
	}

	public void ExitGame()
	{
		//Exit Game
		GetTree().Quit();
	}
}

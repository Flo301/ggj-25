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
			throw new Exception("Singleton already instantiated");
		}
		Instance = this;

		PointLabel = GetNode<Label>("%PointLabel");
		RoundLabel = GetNode<Label>("%RoundLabel");
		OnStartSoundPlayer = GetNode<AudioStreamPlayer2D>("%RoundStartPlayer");
		Round = 1;
		AddPoints(0);

		ItemManager.Instance.GetRandomItem();
	}
	#endregion

	[Export]
	private PackedScene WinLoosePopup;

	//Config
	[Export]
	private GameStageResource[] GameStages;

	//NODES
	private Label PointLabel;
	private Label RoundLabel;
	private AudioStreamPlayer2D OnStartSoundPlayer;

	//PROPERTIES
	private bool IsRoundActive = false;
	private int Points;
	private int round;
	private int Round { get => round; set { RoundLabel.Text = "Round " + value; round = value; } }
	public int Stage { get; private set; }
	public GameStageResource CurrentStage => Stage <= GameStages.Count() - 1 ? GameStages[Stage] : null;

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

			if (CurrentStage == null)
			{
				PopupWinLoose PopupInstance = WinLoosePopup.Instantiate<PopupWinLoose>();
				PopupInstance.Win = true;
				AddChild(PopupInstance);
				//ToDo: GAME END
				GD.Print("GAME END");
				return;
			}

			AddPoints(-Points); //Reset points
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

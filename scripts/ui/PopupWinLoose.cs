using Godot;
using System;

public partial class PopupWinLoose : Control
{
	[Export]
	public string WinTitleString;
	[Export]
	public string WinMessageString;
	
	[Export]
	public string LooseTitleString;
	[Export]
	public string LooseMessageString;
	
	public bool Win = false;
	private Label TitleLabel;
	private Label MessageLabel;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		TitleLabel = GetNode<Label>("%TitleLabel");
		MessageLabel = GetNode<Label>("%MessageLabel");
		if(Win)
		{
			TitleLabel.Text = WinTitleString;
			MessageLabel.Text = WinMessageString;
		}
		else
		{
			TitleLabel.Text = LooseTitleString;
			MessageLabel.Text = LooseMessageString;
		}
	}
	
	public void OnReplayPressed()
	{
		GameManager.Instance.RestartGame();
	}
	
	public void OnExitPressed()
	{
		GameManager.Instance.ExitGame();
	}
}

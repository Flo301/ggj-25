using Godot;
using System;

public partial class PopupStageIntro : Control
{
	[Export]
	public string StageTitleString;
	[Export]
	public string StageMessageString;
	
	private Label TitleLabel;
	private Label MessageLabel;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Label>("%TitleLabel").Text = StageTitleString;
		GetNode<Label>("%MessageLabel").Text = StageMessageString;
	}
	
	public void OnOkayPressed()
	{
		ItemManager.Instance.GetRandomItem();
		this.QueueFree();
	}
}

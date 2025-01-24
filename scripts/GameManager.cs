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
	}
	#endregion

	private int Points = 0;
	private Label PointLabel;

	public void AddPoints(int amount)
	{
		Points += amount;
		PointLabel.Text = Points + "P";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//DEBUG POINTS
		//AddPoints(1);
	}
}
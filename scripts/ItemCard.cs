using Godot;
using System;

public partial class ItemCard : Control
{
	[Export]
	public Texture2D Icon;
	private TextureRect IconRect;
	
	[Export]
	public ERarity Rarity = ERarity.COMMON;
	
	//[Export]
	//public Texture2D CardBackground;
	private NinePatchRect CardBackgroundRect;
	
	[Export]
	public string NameText;
	private Label NameLabel;
	
	[Export]
	public string DescriptionText;
	private Label DescriptionLabel;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		IconRect = GetNode<TextureRect>("%Icon");
		CardBackgroundRect = GetNode<NinePatchRect>("%CardBackground");
		NameLabel = GetNode<Label>("%NameLabel");
		DescriptionLabel = GetNode<Label>("%DescriptionLabel");
		
		IconRect.Texture = Icon;
		CardBackgroundRect.Texture = (Texture2D)GD.Load("res://sprites/BoxUI/BoxUI-" + Rarity.ToString().ToLower() + ".png");
		NameLabel.Text = NameText;
		DescriptionLabel.Text = DescriptionText;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void OnMouseEnter()
	{
		
	}
}

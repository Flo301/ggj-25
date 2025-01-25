using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class CardSelector : Control
{
	[Export]
	public PackedScene ItemCardScene;
	public PackedScene ItemScenes;
	public BoxContainer CardContainer;
	static public CardSelector Instance {get; private set;}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CardContainer = GetNode<BoxContainer>("%ItemSelectHContainer");
		//Get Random Cards
		//Initialise Scenes
		//Show CardSelector
	}
	
	public void SetRandomItems(IEnumerable<Godot.PackedScene> ItemScenes)
	{
		GD.Print(string.Join(",", ItemScenes.Select(x => x.ResourcePath)));
		foreach(PackedScene Item in ItemScenes)
		{
			ItemCard Card = ItemCardScene.Instantiate<ItemCard>();
			GridItem ItemInstance = Item.Instantiate<GridItem>();
			//Card.Icon = Item.Icon;
			Card.Rarity = ItemInstance.Rarity;
			Card.NameText = ItemInstance.Name;
			Card.DescriptionText = ItemInstance.Description;
			Card.ScenePath = Item.ResourcePath;
			Card.Parent = Instance;
			CardContainer.AddChild(Card);
		}
	}
	
	public void OnCardSelected(string ScenePath)
	{
		//Item or Relic?
		
		// --- ITEMS ---
		CardContainer.Visible = false;
		//Hide CardSelector
		
		//LeftClick to place
		//RightClick to cancel
		// - Unhide ItemSelector
		// --- RELICS ---
		//Hide CardSelector
		//Add Relic to Relics
		//Destroy CardSelector
	}
	
	public void OnPlacement(Node GridItem)
	{
		//Check if Valid
		//Destroy CardSelector
		//Destroy ItemSlot
		//Spawn Item
	}
	
	public void OnPlacementCanceled()
	{
		CardContainer.Visible = true;
		//Unhide CardSelector
	}
	

}

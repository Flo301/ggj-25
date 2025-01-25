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

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CardContainer = GetNode<BoxContainer>("%ItemSelectHContainer");
		//Get Random Cards
		//Initialise Scenes
		//Show CardSelector
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
		if (CurrentItemToPlace != null)
		{
			CurrentItemToPlace.GlobalPosition = GetGlobalMousePosition();

			if(Input.IsMouseButtonPressed(MouseButton.Left))
			{
				//Get Nearest ItemSlot and replace
				var SelectedGridItem = ItemManager.Instance.GetNearestItem(GetGlobalMousePosition(), CurrentItemToPlace);
				GD.Print("SelectedGridItem: " + SelectedGridItem.Name);
				CurrentItemToPlace.GlobalPosition = SelectedGridItem.GlobalPosition;
				GD.Print("SelectedGridItemPosition: " + SelectedGridItem.GlobalPosition);
				CurrentItemToPlace.Reparent(SelectedGridItem.GetParent());
				SelectedGridItem.QueueFree();
				OnPlacement();
			} 
			else if(Input.IsMouseButtonPressed(MouseButton.Right))
			{
				CurrentItemToPlace.QueueFree();
				OnPlacementCanceled();
			}
		}
	}

	public void SetRandomItems(IEnumerable<PackedScene> ItemScenes)
	{
		foreach (PackedScene Item in ItemScenes)
		{
			ItemCard Card = ItemCardScene.Instantiate<ItemCard>();
			CardContainer.AddChild(Card);

			Card.Init(this, Item);
		}
	}

	private GridItem CurrentItemToPlace;
	public void OnCardSelected(PackedScene selectedItem)
	{
		GD.Print("choose: " + selectedItem.ResourcePath);

		CurrentItemToPlace = selectedItem.Instantiate<GridItem>();
		GetWindow().AddChild(CurrentItemToPlace);
		//Item or Relic?

		// --- ITEMS ---
		this.Visible = false;
		//Hide CardSelector

		//LeftClick to place
		//RightClick to cancel
		// - Unhide ItemSelector
		// --- RELICS ---
		//Hide CardSelector
		//Add Relic to Relics
		//Destroy CardSelector
	}

	public void OnPlacement()
	{
		//Check if Valid

		//Destroy ItemSlot
		//Spawn Item

		ItemManager.Instance.ItemPlaced(CurrentItemToPlace);
		CurrentItemToPlace = null;

		//Destroy CardSelector
		QueueFree();
	}

	public void OnPlacementCanceled()
	{
		this.Visible = true;
		//Unhide CardSelector
	}
	
	
}

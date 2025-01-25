using Godot;
using System;

public partial class CardSelector : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Get Random Cards
		//Initialise Scenes
		//Show CardSelector
	}
	
	public void OnCardSelected(CardInfo)
	{
		//Item or Relic?
		
		// --- ITEMS ---
		//Hide CardSelector
		
		//LeftClick to place
		// - Destroy CardSelector
		
		//RightClick to cancel
		// - Unhide ItemSelector
		
		// --- RELICS ---
		//Hide CardSelector
		//Add Relic to Relics
		//Destroy CardSelector
	}
	
	public void OnPlacement(GridItem)
	{
		//Check if Valid
		//Destroy CardSelector
		//Destroy ItemSlot
		//Spawn Item
	}
	
	public void OnPlacementCanceled()
	{
		//Unhide CardSelector
	}
	

}

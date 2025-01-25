using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class ItemManager : Node
{
	#region Singleton
	static public ItemManager Instance { get; private set; }

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Instance != null)
		{
			throw new Exception("Singleton already instantiated");
		}
		Instance = this;
	}
	#endregion

	[Export]
	public PackedScene[] AvailableItems;
	[Export]
	public int ItemAmountToChoose = 3;

	private List<GridItem> ActiveItems = new List<GridItem>();

	public void GetRandomItem()
	{
		//Roll random items
		var rng = new RandomNumberGenerator();
		var rngItems = new PackedScene[ItemAmountToChoose].Select(x => AvailableItems[rng.RandiRange(0, AvailableItems.Count() - 1)]);

		//ToDo: Open item selection
		GD.Print(string.Join(",", rngItems.Select(x => x.ResourcePath)));
	}

	public void AddItem(GridItem item)
	{
		ActiveItems.Add(item);
	}

	public void ItemPlaced(GridItem item)
	{
		GameManager.Instance.StartNextRound();
	}

	public void RemoveItem(GridItem item)
	{
		if (ActiveItems.Contains(item))
			ActiveItems.Remove(item);
	}

	public void OnRoundEnd()
	{
		ActiveItems.ForEach(x => x.OnRoundEnd());
	}
}

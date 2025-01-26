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
	[Export]
	public PackedScene ItemSelectorScene;

	private List<GridItem> ActiveItems = new List<GridItem>();

	public void GetRandomItem()
	{
		Dictionary<PackedScene, GridItem> matchTable = new Dictionary<PackedScene, GridItem>();
		AvailableItems.ToList().ForEach(x => matchTable.Add(x,x.Instantiate<GridItem>()));

		//roll random items
		var rngItems = matchTable
			.OrderByDescending(x => GD.Randf() + GetRarityBaseValue(x.Value.Rarity, GameManager.Instance.Stage))
			.Take(ItemAmountToChoose)
			.Select(x => x.Key);

		matchTable.Select(x => x.Value).ToList().ForEach(x => x.QueueFree());

		//open selector
		CardSelector Selector = ItemSelectorScene.Instantiate<CardSelector>();
		AddChild(Selector);
		Selector.SetRandomItems(rngItems);

		float GetRarityBaseValue(ERarity rarity, int stage)
		{
			switch (rarity)
			{
				case ERarity.COMMON: return 0.6f - stage / 10f;
				case ERarity.UNCOMMON: return 0.55f - stage / 20f;
				case ERarity.RARE: return 0.45f;
				case ERarity.EPIC: return 0.30f + stage / 20f;
				case ERarity.LEGENDARY: return 0.10f + stage / 40f;
				default: throw new IndexOutOfRangeException();
			}
		}
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

	public GridItem GetNearestItem(Godot.Vector2 pos, GridItem self)
	{
		return ActiveItems.Where(x => x != self).OrderBy(x => x.GlobalPosition.DistanceTo(pos)).FirstOrDefault();
	}
}

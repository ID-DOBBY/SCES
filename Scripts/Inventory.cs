using Godot;
using System;
//Just setting up foundations 
public partial class Inventory : Node
{
	public struct Item //Foundations for future Not used RN
	{
	public string Name;
	public int Amount;
	public int Value;

		public Item(string name, int amount, int value)
		{
			Name = name;
			Amount = amount;
			Value = value;
		}
	}
	[Export] public int ammo = 50; 
	[Export] public int stone = 0;
		public void AddItem()
	{
		
	}
}

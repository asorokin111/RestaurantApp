using System;

public class FoodItem
{
    public string Name = "Item";
    public float Price = 0.0f;
    public int Amount = 1;

    public FoodItem(string name, float price, int amount)
    {
        Name = name;
        Price = price;
        Amount = amount;
    }

    public void addAmount(int amount)
    {
        Amount += amount;
    }
}

public class FoodItem
{
    public string Name { get; set; } = "Item";
    public float Price { get; set; } = 0.0f;

    public FoodItem(string name, float price)
    {
        Name = name;
        Price = price;
    }
}

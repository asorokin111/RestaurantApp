[System.Serializable]
public class FoodItem
{
    public string Name = "Item";
    public float Price = 0.0f;

    public FoodItem(string name, float price)
    {
        Name = name;
        Price = price;
    }
}

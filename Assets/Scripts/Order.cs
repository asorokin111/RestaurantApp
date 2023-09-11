using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

//Everything related to orders and writing them
public class Order : MonoBehaviour
{
    private UIDocument _uidoc; // For processing the order after a button is pressed
    private float _sum;
    private List<FoodItem> _orderList;
    private List<Button> addButtons;
    private List<Button> removeButtons;
    private Button _confirmButton;
    private Button _lastAdded; // Always set to the last addbutton that was pressed
    private Button _lastRemoved; // Always set to the last removebutton that was pressed
    private Label _costLabel;

    private void Start()
    {
        _orderList = new List<FoodItem>();
        _uidoc = GetComponent<UIDocument>();
        var root = _uidoc.rootVisualElement;
        _costLabel = root.Q<Label>("cost");
        _confirmButton = root.Q<Button>("confirmbtn");
        _confirmButton.clickable.clicked += OnConfirm;
        addButtons = root.Query<Button>("addbtn").ToList();
        removeButtons = root.Query<Button>("removebtn").ToList();
        foreach (var button in addButtons)
        {
            button.clickable.clicked += () => { _lastAdded = button; };
            button.clickable.clicked += OnAddItem;
        }
        foreach (var button in removeButtons)
        {
            button.clickable.clicked += () => { _lastRemoved = button; };
            button.clickable.clicked += OnRemoveItem;
        }
    }

    private void OnDestroy()
    {
        _confirmButton.clickable.clicked -= OnConfirm;
        foreach (var button in addButtons)
        {
            button.clickable.clicked -= OnAddItem;
            button.clickable.clicked -= () => { _lastAdded = button; };
        }
        foreach (var button in removeButtons)
        {
            button.clickable.clicked -= OnRemoveItem;
            button.clickable.clicked -= () => { _lastRemoved = button; };
        }
    }

    public float GetTotal()
    {
        _sum = 0.0f;
        foreach (FoodItem item in _orderList)
        {
            _sum += item.Price;
        }
        return _sum;
    }

    public FoodItem StrToItem(string str)
    {
        string[] strArr = str.Split('\n');
        string name = strArr[0];
        string price = strArr[1].Replace("€", string.Empty);
        float floatPrice = float.Parse(price, CultureInfo.InvariantCulture.NumberFormat);
        return new FoodItem(name, floatPrice);
    }

    public void OnConfirm()
    {
        Debug.Log("Confirmed order");
    }

    public void OnAddItem()
    {
        string itemStr = _lastAdded.hierarchy.parent.Q<Label>().text;
        _orderList.Add(StrToItem(itemStr));
        Debug.Log("Added an item");
        _costLabel.text = "Total cost: " + GetTotal();
    }

    public void OnRemoveItem()
    {
        Debug.Log("Removed an item");
        _costLabel.text = "Total cost: " + GetTotal();
    }
}

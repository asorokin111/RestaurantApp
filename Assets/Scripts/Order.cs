using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

//Everything related to orders and writing them
public class Order : MonoBehaviour
{
    private UIDocument _uidoc; // For processing the order after a button is pressed
    private float _sum;
    private List<FoodItem> _orderList;

    private void Start()
    {
        _orderList = new List<FoodItem>();
        _uidoc = GetComponent<UIDocument>();
        var root = _uidoc.rootVisualElement;
        List<Button> addButtons = root.Query<Button>("addbtn").ToList();
        List<Button> removeButtons = root.Query<Button>("removebtn").ToList();
        foreach (var button in addButtons)
        {
            button.clickable.clicked += OnAddItem;
        }
        foreach (var button in removeButtons)
        {
            button.clickable.clicked += OnRemoveItem;
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

    public void OnAddItem()
    {

    }

    public void OnRemoveItem()
    {
        
    }
}

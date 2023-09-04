using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order : MonoBehaviour
{
    private float _sum;
    private List<FoodItem> _orderList;

    private void Start()
    {
        _orderList = new List<FoodItem>();
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
}

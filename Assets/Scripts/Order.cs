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
    private List<Button> addButtons;
    private List<Button> removeButtons;

    private Button _confirmButton;
    private Button _cancelButton;

    private Label _costLabel;

    private void Start()
    {
        _orderList = new List<FoodItem>();
        _uidoc = GetComponent<UIDocument>();
        var root = _uidoc.rootVisualElement;
        _costLabel = root.Q<Label>("cost");
        _confirmButton = root.Q<Button>("confirmbtn");
        _cancelButton = root.Q<Button>("cancelbtn");
        _confirmButton.clickable.clicked += OnConfirm;
        _cancelButton.clickable.clicked += OnCancel;
        addButtons = root.Query<Button>("addbtn").ToList();
        removeButtons = root.Query<Button>("removebtn").ToList();
        foreach (var button in addButtons)
        {
            button.RegisterCallback<ClickEvent>(ev => OnAddItem(button));
        }
        foreach (var button in removeButtons)
        {
            button.RegisterCallback<ClickEvent>(ev => OnRemoveItem(button));
        }
    }

    private void OnDestroy()
    {
        _confirmButton.clickable.clicked -= OnConfirm;
        foreach (var button in addButtons)
        {
            button.UnregisterCallback<ClickEvent>(ev => OnAddItem(button));
        }
        foreach (var button in removeButtons)
        {
            button.UnregisterCallback<ClickEvent>(ev => OnRemoveItem(button));
        }
    }

    public float GetTotal()
    {
        _sum = 0.0f;
        foreach (FoodItem item in _orderList)
        {
            _sum += item.Price * item.Amount;
        }
        return _sum;
    }

    public FoodItem StrToItem(string str)
    {
        string[] strArr = str.Split('\n');
        string name = strArr[0];
        string price = strArr[1].Replace("€", string.Empty);
        float floatPrice = float.Parse(price, CultureInfo.InvariantCulture.NumberFormat);
        return new FoodItem(name, floatPrice, 1);
    }

    public void WriteOrderToJson()
    {
        string orderPath = Application.persistentDataPath + "/order.json";
        const string totalCostVarName = "total"; // Name of the json variable used for total cost
        const string orderArrayName = "order";
        System.IO.File.WriteAllText(orderPath, string.Empty); // Clear before writing

        if (_orderList.Count <= 0) return; //Probably not the best place for a return but still don't want to deal with "Index out of bounds"

        System.IO.File.AppendAllText(orderPath, "{\"" + totalCostVarName + "\":");
        System.IO.File.AppendAllText(orderPath, GetTotal().ToString(CultureInfo.InvariantCulture.NumberFormat));
        System.IO.File.AppendAllText(orderPath, ",\n\"" + orderArrayName + "\":[\n");

        for (int i = 0; i < _orderList.Count - 1; ++i)
        {
            System.IO.File.AppendAllText(orderPath, JsonUtility.ToJson(_orderList[i]));
            System.IO.File.AppendAllText(orderPath, ",\n");
        }

        // Have to treat the last item of the list differently for correct formatting
        System.IO.File.AppendAllText(orderPath, JsonUtility.ToJson(_orderList[_orderList.Count - 1]));
        System.IO.File.AppendAllText(orderPath, "\n]}");
    }

    public void OnConfirm()
    {
        WriteOrderToJson();
    }

    public void OnCancel()
    {
        _orderList.Clear();
        updateCost();
    }

    public void OnAddItem(Button clickedButton)
    {
        string itemStr = clickedButton.hierarchy.parent.Q<Label>().text;
        var item = StrToItem(itemStr);
        foreach ( var i in _orderList )
        {
            if (i.Name == item.Name) // Looks really awkward
            {
                i.addAmount(1);
                updateCost();
                return;
            }
        }
        _orderList.Add(item);
        updateCost();
    }

    public void OnRemoveItem(Button clickedButton)
    {
        string itemStr = clickedButton.hierarchy.parent.Q<Label>().text;
        var item = StrToItem(itemStr);
        foreach ( var i in _orderList )
        {
            if (i.Name == item.Name)
            {
                i.addAmount(-1);
                if (i.Amount <= 0)
                {
                    _orderList.Remove(i);
                }
                updateCost();
                return;
            }
        }
    }

    public void updateCost()
    {
        _costLabel.text = "Yhteensä: " + GetTotal() + " euroa";
    }
}

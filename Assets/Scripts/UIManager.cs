using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    public void DisableMenu(GameObject menu)
    {
        menu.SetActive(false);
    }
    public void EnableMenu(GameObject menu)
    {
        menu.SetActive(true);
    }
}

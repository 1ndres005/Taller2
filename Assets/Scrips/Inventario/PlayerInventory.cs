using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerInventory : MonoBehaviour
{
    private List<GameObject> Inventory;

    private void Awake()
    {
        Inventory = new List<GameObject>();
    }

    public void AddToInventory(GameObject Objeto)
    {
        Inventory.Add(Objeto);
    }
}


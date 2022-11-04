using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryManager : MonoBehaviour
{
    [SerializeField] List<Item> items;

    [SerializeField] Item Equppied;
    [SerializeField] Item lastEquppied;

    [SerializeField] List<Image> HotbarItemsImages;

    private void Update()
    {
        switchItem();
    }

    void switchItem()
    {
        lastEquppied = Equppied;

    }

    void pickItem()
    {

    }

    void dropItem()
    {

    }
}

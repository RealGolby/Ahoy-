using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryManager : MonoBehaviour
{
    [SerializeField] List<Item> items;

    [SerializeField] Item equppiedItem;
    [SerializeField] Item lastEquppiedItem;

    [SerializeField] GameObject equppiedItemInstance;
    [SerializeField] GameManager lastEquppiedItemInstance;

    [SerializeField] List<Image> HotbarItemsImages;

    [SerializeField] GameObject itemHolder;

    private void Start()
    {
        items[0] = equppiedItem;
    }

    private void Update()
    {
        getInput();
    }

    void getInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && items[0] != null) switchItem(items[0]);
        if (Input.GetKeyDown(KeyCode.Alpha2) && items[1] != null) switchItem(items[1]);
        if (Input.GetKeyDown(KeyCode.Alpha3) && items[2] != null) switchItem(items[2]);
        if (Input.GetKeyDown(KeyCode.Alpha4) && items[3] != null) switchItem(items[3]);
        if (Input.GetKeyDown(KeyCode.Alpha5) && items[4] != null) switchItem(items[4]);
    }

    void switchItem(Item item)
    {
        lastEquppiedItem = equppiedItem;
        equppiedItem = item;
    }

    void pickItem(GameObject item)
    {

    }

    void dropItem()
    {

    }

    GameObject droppedItem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<ItemInstance>(out ItemInstance instance))
        {
            droppedItem = collision.gameObject;
        }
    }
}

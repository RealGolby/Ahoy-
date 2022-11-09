using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventoryManager : MonoBehaviour
{
    [SerializeField] List<Item> hotbarItems;

    [SerializeField] Item equppiedItem;
    [SerializeField] Item lastEquppiedItem;

    [SerializeField] GameObject equppiedItemInstance;
    [SerializeField] GameManager lastEquppiedItemInstance;

    [SerializeField] List<Image> HotbarItemsImages;

    [SerializeField] GameObject itemHolder;

    private void Start()
    {

    }

    private void Update()
    {
        getInput();
    }

    void getInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && hotbarItems[0] != null) switchItem(hotbarItems[0]);
        if (Input.GetKeyDown(KeyCode.Alpha2) && hotbarItems[1] != null) switchItem(hotbarItems[1]);
        if (Input.GetKeyDown(KeyCode.Alpha3) && hotbarItems[2] != null) switchItem(hotbarItems[2]);
        if (Input.GetKeyDown(KeyCode.Alpha4) && hotbarItems[3] != null) switchItem(hotbarItems[3]);
        if (Input.GetKeyDown(KeyCode.Alpha5) && hotbarItems[4] != null) switchItem(hotbarItems[4]);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Icon;

    public GameObject Prefab;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RotaryHeart.Lib.SerializableDictionary;

[Serializable]
public class ItemValuesDictType : SerializableDictionaryBase<string, float> { }
public enum ItemType
{
    Weapon,
    Chest,
    Boots,
    Gloves,
    Helmet,
    Belt,
    Ring,
    Amulet
}

[Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "ARPG/ItemData", order = 1)]
public class ItemObject : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField, TextArea(10,5)] private string flavorText;
    [SerializeField] private ItemType itemType;
    [SerializeField] private GameObject prefab;
    [SerializeField] private ItemValuesDictType values;

    [Header("Inventory")]
    [SerializeField] private Vector2Int invSize;

    [Header("Drop Info")]
    [SerializeField, Range(0,1)] private float rarity;
    [SerializeField] private List<GameObject> dropsFrom;

    public string Name => name;
    public string FlavorText => flavorText;
    public ItemType ItemType => itemType;
    public GameObject Prefab => prefab;
    public ItemValuesDictType Values => values;

    public Vector2Int InvSize => invSize;

    public float Rarity => rarity;
    public List<GameObject> DropsFrom => dropsFrom;


    private void PopulateDatabase()
    {
        var itemObjectGUIDs = AssetDatabase.FindAssets("t:ItemObject", null);
        foreach (var guid in itemObjectGUIDs)
        {
            var objectPath = AssetDatabase.GUIDToAssetPath(guid);
            var objectData = ((GameObject) AssetDatabase.LoadMainAssetAtPath(objectPath)).GetComponent<ItemObject>();
            if (objectData)
            {
                continue;
            }
            Debug.Log(objectPath);
        }
    }
}

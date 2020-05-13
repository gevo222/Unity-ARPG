using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RotaryHeart.Lib.SerializableDictionary;


[System.Serializable]
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

[System.Serializable]
[CreateAssetMenu(fileName = "New Item", menuName = "ARPG/ItemData", order = 1)]
public class ItemObject : ScriptableObject
{
    [SerializeField] private string name;
    [SerializeField, TextArea(10,5)] private string flavorText;
    [SerializeField] private ItemType itemType;
    [SerializeField] private GameObject worldPrefab;
    [SerializeField] private GameObject uiPrefab;
    [SerializeField] private Sprite uiSprite;
    [SerializeField] private ItemValuesDictType values;

    [Header("Inventory")]
    [SerializeField] private Vector2Int invSize;

    [Header("Drop Info")]
    [SerializeField, Range(0,1)] private float rarity;
    [SerializeField] private List<GameObject> dropsFrom;

    public string Name => name;
    public string FlavorText => flavorText;
    public ItemType ItemType => itemType;
    public GameObject WorldPrefab => worldPrefab;
    public GameObject UiPrefab => uiPrefab;
    public Sprite UiSprite => uiSprite;
    public ItemValuesDictType Values => values;

    public Vector2Int InvSize => invSize;

    public float Rarity => rarity;

    private static List<ItemObject> all = null;
    private static Dictionary<string, ItemObject> byPath= null;

    private static void findAll()
    {
        all = new List<ItemObject>{};
        byPath = new Dictionary<string, ItemObject>{};
        var itemObjectGUIDs = AssetDatabase.FindAssets("t:ItemObject", new string[]{"Assets/Resources"});
        foreach (var guid in itemObjectGUIDs)
        {
            // Resources.Load requires the path to be extensionless and relative to Assets/Resources/
            var objectPath = Path.ChangeExtension(AssetDatabase.GUIDToAssetPath(guid), null)
                                 .Remove(0, "Assets/Resources/".Length);
            var objectResource = Resources.Load<ItemObject>(objectPath);
			all.Add(objectResource);
            byPath.Add(objectPath, objectResource);
        }
    }

    public static List<ItemObject> All {
		get { if(all == null) findAll(); return all; }
	}
    public static Dictionary<string, ItemObject> ByPath {
		get { if(byPath == null) findAll(); return byPath; }
	}

	public static ItemObject GetRandomItem(){
		var index = Random.Range(0, All.Count);
		return All[index];
	}
}

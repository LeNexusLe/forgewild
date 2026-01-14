using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemChooseUIController : MonoBehaviour
{
    public static ItemChooseUIController Instance { get; private set; }

    public GameObject popupPrefab;

    [Header("Rarity Frames")]
    public Sprite frameCommon;
    public Sprite frameEpic;
    public Sprite frameLegendary;

    private readonly List<GameObject> activePopups = new();
    private ItemManager currentItemManager;

    void Awake()
    {
        Instance = this;
    }

    public void ShowChoose(List<Item> items, ItemManager manager)
    {
        currentItemManager = manager;

        foreach (Item item in items)
        {
            GameObject popup = Instantiate(popupPrefab, transform);

            PauseManager.instance.PauseGame();

            popup.GetComponentInChildren<TMP_Text>().text = item.itemName;

            popup.transform.Find("ItemIcon")
                .GetComponent<Image>().sprite = item.icon;

            popup.transform.Find("RarityText")
                .GetComponent<TMP_Text>().text = item.rarity.ToString();

            popup.GetComponent<Image>().sprite = GetFrameByRarity(item.rarity);

            Button chooseButton = popup.transform.Find("ChooseButton").GetComponent<Button>();


            chooseButton.onClick.RemoveAllListeners();
            chooseButton.onClick.AddListener(() => ChooseItem(item));

            activePopups.Add(popup);
        }
    }

    void ChooseItem(Item item)
    {
        currentItemManager.AddItem(item);

        PauseManager.instance.UnpauseGame();

        foreach (GameObject popup in activePopups)
            Destroy(popup);

        activePopups.Clear();
    }

    private Sprite GetFrameByRarity(Rarity rarity)
    {
        return rarity switch
        {
            Rarity.Common => frameCommon,
            Rarity.Epic => frameEpic,
            Rarity.Legendary => frameLegendary,
            _ => frameCommon
        };
    }
}

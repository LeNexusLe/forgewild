using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemPickupUIController : MonoBehaviour
{
    public static ItemPickupUIController Instance { get; private set; }

    public GameObject popupPrefab;
    public int maxPopups = 2;
    public float popupDuration = 3f;

    [Header("Rarity Frames")]
    public Sprite frameCommon;
    public Sprite frameEpic;
    public Sprite frameLegendary;

    private readonly Queue<GameObject> activePopups = new();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.Log("Mulityply instance itemui");
            Destroy(gameObject);
        }

        GameObject temp = Instantiate(popupPrefab, transform); // Jakiœ lag przy pierwszym podniesieniu ??
        temp.SetActive(false);
    }


    public void ShowItemPickup(string itemName, Sprite itemIcon, Rarity rarity)
    {
        GameObject newPopup = Instantiate(popupPrefab, transform);

        newPopup.GetComponentInChildren<TMP_Text>().text = itemName;

        Image itemImage = newPopup.transform.Find("ItemIcon")?.GetComponent<Image>();
        if (itemImage)
        {
            itemImage.sprite = itemIcon;
        }

        Image frameImage = newPopup.GetComponent<Image>();
        Debug.Log(frameImage);
        if (frameImage)
        {
            frameImage.sprite = GetFrameByRarity(rarity);
        }

        newPopup.transform.Find("RarityText").GetComponent<TMP_Text>().text = rarity.ToString();

        activePopups.Enqueue(newPopup);
        if (activePopups.Count > maxPopups)
        {
            Destroy(activePopups.Dequeue());
        }

        StartCoroutine(FadeOutAndDestroy(newPopup));
    }

    private IEnumerator FadeOutAndDestroy(GameObject popup)
    {
        yield return new WaitForSeconds(popupDuration);
        if(popup == null) yield break;

        CanvasGroup canvasGroup = popup.GetComponent<CanvasGroup>();
        for(float timePassed = 0f; timePassed < 1f; timePassed += Time.deltaTime)
        {
            if (popup == null) yield break;
            canvasGroup.alpha = 1f - timePassed;
            yield return null;
        }

        Destroy(popup);
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

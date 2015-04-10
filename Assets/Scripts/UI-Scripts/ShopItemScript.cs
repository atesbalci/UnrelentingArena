using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopItemScript : MonoBehaviour {
    public Image image;
    public Text itemName;
    public Button button;
    public Item item { get; set; }

    void Start() {
        Refresh();
    }

    public void Refresh() {
        itemName.text = item.name;
        image.sprite = Resources.Load<Sprite>("Icons/" + item.name);
        button.GetComponentInChildren<Text>().text = item.price + " C";
        button.interactable = !GameManager.instance.playerData.itemSet.items.Contains(item);
        if(!button.interactable)
            button.GetComponentInChildren<Text>().text = "Owned";
        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(() => { Buy(); });
    }

    public void Buy() {
        PlayerData playerData = GameManager.instance.playerData;
        if (playerData.credits >= item.price) {
            playerData.itemSet.items.Add(item);
            playerData.credits -= item.price;
            Refresh();
        }
    }
}

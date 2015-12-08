using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CountdownScript : MonoBehaviour {
    public Sprite[] sprites;

    private Image image;
    private Animator anim;

    void OnEnable() {
        image = GetComponent<Image>();
        anim = GetComponent<Animator>();
        anim.Play("Countdown", -1, 1);
    }

    public void Countdown(int n) {
        image.color = GameManager.instance.playerData.currentPlayer.color;
        image.sprite = sprites[n];
        anim.Play("Countdown", -1, 0);
    }
}

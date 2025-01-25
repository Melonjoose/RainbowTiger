using UnityEngine;
using UnityEngine.UI;

public class SpriteToggleButton : MonoBehaviour
{
    public Sprite spriteUnmuted;
    public Sprite spriteMuted;

    private Image buttonSprite;
    public bool isUsingSpriteUnmuted = true;

    private void Start()
    {
        buttonSprite = GetComponent<Image>();
    }

    public void ToggleSprite()
    {
        if (isUsingSpriteUnmuted)
        {
            buttonSprite.sprite = spriteMuted;
        }
        else
        {
            buttonSprite.sprite = spriteUnmuted;
        }

        isUsingSpriteUnmuted = !isUsingSpriteUnmuted;
    }
}
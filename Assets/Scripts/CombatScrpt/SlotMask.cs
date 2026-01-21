using UnityEngine;

public class SlotMask : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        var c = spriteRenderer.color;
        c.a = 0.6f;
        spriteRenderer.color = c;
    }

    public void ResetAlpha()
    {
        var c = spriteRenderer.color;
        c.a = 0.0f;
        spriteRenderer.color = c;
    }
}

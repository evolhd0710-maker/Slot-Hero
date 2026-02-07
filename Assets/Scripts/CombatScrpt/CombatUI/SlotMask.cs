using UnityEngine;

public class SlotMask : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public int index;
    public bool selected;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        selected = false;
    }

    private void OnMouseDown()
    {
        var c = spriteRenderer.color;
        c.a = 0.6f;
        spriteRenderer.color = c;
        selected = true;
    }

    public void ResetMask()
    {
        var c = spriteRenderer.color;
        c.a = 0.0f;
        spriteRenderer.color = c;
        selected = false;
    }
}

using UnityEngine;
using TMPro;

public class Hp : MonoBehaviour
{
    public Transform unitTransform;
    public Vector3 offset = new Vector3(0, 4.0f, 0);

    private RectTransform rectTransform;
    private Camera mainCamera;
    private TextMeshProUGUI hpText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;
        hpText = GetComponent<TextMeshProUGUI>();   
        UpdateTransform();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateTransform()
    {
        if (unitTransform == null) return;
        Vector3 worldPos = unitTransform.position + offset;
        Vector3 screenPos = mainCamera.WorldToScreenPoint(worldPos);

        if(screenPos.z < 0)
        {
            rectTransform.localScale = Vector3.zero;
        }
        else
        {
            rectTransform.localScale = Vector3.one;
            rectTransform.position = screenPos; 
        }
    }

    public void SetUp(int maxHp, int currentHp, int shield)
    {
        if (hpText == null)
        {
            print("textNull");
            return;
        }
        hpText.text = $"{currentHp} / {maxHp} ({shield})";
    }
}

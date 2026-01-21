using UnityEngine;

public class BootStrapper : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        PlayerDataReader.Instance.Load();
        SlotDataReader.Instance.Load();
        RelicDatabase.instance.Load();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

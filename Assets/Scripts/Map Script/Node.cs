using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Node : MonoBehaviour
{
    public int nodeIndex;
    public Image[] nodeImage;
    public int[] nodeCoordinate = new int[2];
    public MapManager mapManager;
    private void Start()
    {
    }
    public void OnNodeClicked()
    {
        switch (nodeIndex)
        {
            case 0:
                //일반 몬스터
                Debug.Log("0");
                mapManager.currentPosition++;
                break;
            case 1:
                //보스 몬스터
                Debug.Log("1");
                mapManager.currentPosition++;
                break;
            case 2:
                //상점
                Debug.Log("2");
                mapManager.currentPosition++;
                break;
            case 3:
                //체력회복
                Debug.Log("3");
                mapManager.currentPosition++;
                break;
            case 4:
                //랜덤 이벤트
                Debug.Log("4");
                mapManager.currentPosition++;
                break;
        }
    }
}

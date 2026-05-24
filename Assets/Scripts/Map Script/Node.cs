using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using TMPro;
using System.Collections;

public class Node : MonoBehaviour
{
    public int nodeIndex;
    public int[] nodeCoordinate = new int[2];
    public MapManager mapManager;
    public bool isSelectable = false;
    public HashSet<Node> nextNodes = new HashSet<Node>();
    public Button nodeButton;


    private Coroutine pulseCoroutine;
    private Vector3 initialScale;
    [SerializeField] private float targetScaleMultiplier = 1.2f; // 최대 커지는 배율
    [SerializeField] private float pulseSpeed = 5.0f;           // 반복 속도

    private void Awake()
    {
        nodeButton = GetComponent<Button>();
        initialScale = transform.localScale;
        if (initialScale.z == 0) initialScale.z = 1f;
    }


    public void SetHighlight(bool On)
    {
        isSelectable = On;

        // 버튼의 Image 컴포넌트가 있는지 확인 후 색상 변경
        if (nodeButton.targetGraphic != null)
        {
            nodeButton.targetGraphic.color = On ? Color.yellow : Color.white;
        }

        if (On)
        {
            if (pulseCoroutine == null)
            {
                pulseCoroutine = StartCoroutine(PulseAnimation());
            }
        }
        else
        {
            if (pulseCoroutine != null)
            {
                StopCoroutine(pulseCoroutine);
                pulseCoroutine = null;
            }
            // 원래 크기로 즉시 복구 (사라짐 방지)
            transform.localScale = initialScale;
        }
    }

    private IEnumerator PulseAnimation()
    {
        // 시작 시간을 기록하여 일정한 속도로 움직이게 함
        float startTime = Time.time;

        while (isSelectable)
        {
            // (현재시간 - 시작시간)을 사용하여 개별 노드마다 독립적인 타이밍 부여
            float phase = (Time.time - startTime) * pulseSpeed;
            float lerpTime = (Mathf.Sin(phase) + 1f) / 2f; // 0 ~ 1 사이를 부드럽게 반복

            Vector3 targetScale = initialScale * targetScaleMultiplier;

            // Z축은 항상 initialScale.z (보통 1)를 유지하도록 설정
            transform.localScale = new Vector3(
                Mathf.Lerp(initialScale.x, targetScale.x, lerpTime),
                Mathf.Lerp(initialScale.y, targetScale.y, lerpTime),
                initialScale.z
            );

            yield return null;
        }
    }

    public void OnNodeClicked()
    {
        mapManager.OnNodeSelected(this);    
        switch (nodeIndex)
        {
            case 0:
                //일반 몬스터
                Debug.Log("0");
                break;
            case 1:
                //보스 몬스터
                Debug.Log("1");

                break;
            case 2:
                //상점
                Debug.Log("2");
                break;
            case 3:
                //체력회복
                Debug.Log("3");
                break;
            case 4:
                //랜덤 이벤트
                Debug.Log("4");
                break;
        }
    }
}

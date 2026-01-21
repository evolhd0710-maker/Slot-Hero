using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public GameObject nodePrefab;
    public GameObject canvas;
    GameObject[,] nodes = new GameObject[6, 13];
    public GameObject nodeSet;
    public GameObject linePrefab;
    private List<GameObject> lineObjects = new List<GameObject>();
    public GameObject destNode;
    public GameObject lineSet;
    public int currentPosition;
    public GameObject map;
    public int stage;

    private void Start()
    {

        List<LineRenderer> lineArray = new List<LineRenderer>();
        if (stage == 1)
        {
            FirstStageNodeCreate();
        }
        else
        {
            NodeCreate();
        }
        DrawLine();
        currentPosition = 0;
    }
    private void Update()
    {

    }


    // index 일반 전투 스테이지 0 정예 전투 스테이지 1 이벤트 스테이지 2 상점 스테이지 3
    void FirstStageNodeCreate()
    {
        int width = nodes.GetLength(0);
        int height = nodes.GetLength(1);

        List<int> firstRow = new List<int> { 0, 1, 2, 3, 4, 5 };
        for (int i = 0; i < Random.Range(2, 7); i++)
        {
            int randomIndex = Random.Range(0, firstRow.Count);
            int t = firstRow[randomIndex];
            firstRow.RemoveAt(randomIndex);
            CreateNode(t, 0);
        }

        for (int j = 1; j < height; j++)
        {
            HashSet<int> possiblePositions = new HashSet<int>(); // 중복 방지를 위해 HashSet 사용

            //  이전 줄과 대각선 방향까지 연결 가능한 위치 찾기
            for (int i = 0; i < width; i++)
            {
                if (nodes[i, j - 1] != null) // 바로 아래 노드가 있는 경우
                {
                    possiblePositions.Add(i);
                }
                else if (i > 0 && nodes[i - 1, j - 1] != null) // 왼쪽 아래 대각선
                {
                    possiblePositions.Add(i);
                }
                else if (i < width - 1 && nodes[i + 1, j - 1] != null) // 오른쪽 아래 대각선
                {
                    possiblePositions.Add(i);
                }
            }

            //  최소 2개 보장: possiblePositions에서 랜덤하게 선택
            List<int> selectedPositions = new List<int>();
            for (int i = 0; i < Random.Range(2, possiblePositions.Count + 1); i++)
            {
                int randomIndex = Random.Range(0, possiblePositions.Count);
                selectedPositions.Add(possiblePositions.ElementAt(randomIndex));
                possiblePositions.Remove(selectedPositions.Last());
            }

            //  최종적으로 선택된 위치에 노드 생성
            foreach (int i in selectedPositions)
            {
                CreateNode(i, j);
            }
        }
    }

    void NodeCreate()
    {
        int width = nodes.GetLength(0);  // 4
        int height = nodes.GetLength(1); // 5

        //  첫 번째 줄 랜덤한 위치에 3개의 노드 생성
        List<int> firstRowIndices = new List<int> { 0, 1, 2, 3 }; // 가능한 위치 (4x5 기준)
        for (int k = 0; k < 3; k++)
        {
            int randomIndex = Random.Range(0, firstRowIndices.Count);
            int i = firstRowIndices[randomIndex];
            firstRowIndices.RemoveAt(randomIndex);
            CreateNode(i, 0);
        }

        for (int j = 1; j < height; j++)
        {
            HashSet<int> possiblePositions = new HashSet<int>(); // 중복 방지를 위해 HashSet 사용

            //  이전 줄과 대각선 방향까지 연결 가능한 위치 찾기
            for (int i = 0; i < width; i++)
            {
                if (nodes[i, j - 1] != null) // 바로 아래 노드가 있는 경우
                {
                    possiblePositions.Add(i);
                }
                if (i > 0 && nodes[i - 1, j - 1] != null) // 왼쪽 아래 대각선
                {
                    possiblePositions.Add(i);
                }
                if (i < width - 1 && nodes[i + 1, j - 1] != null) // 오른쪽 아래 대각선
                {
                    possiblePositions.Add(i);
                }
            }

            List<int> selectedPositions = new List<int>();

            //  최소 2개 보장: possiblePositions에서 랜덤하게 선택
            while (selectedPositions.Count < 2 && possiblePositions.Count > 0)
            {
                int randomIndex = Random.Range(0, possiblePositions.Count);
                selectedPositions.Add(possiblePositions.ElementAt(randomIndex));
                possiblePositions.Remove(selectedPositions.Last());
            }

            //  만약 selectedPositions에 2개가 안 채워졌다면 강제로 추가
            while (selectedPositions.Count < 2)
            {
                int randomPos = Random.Range(0, width);
                if (!selectedPositions.Contains(randomPos))
                {
                    selectedPositions.Add(randomPos);
                }
            }



            // 추가 랜덤 노드 생성 (10% 확률)
            foreach (int i in possiblePositions)
            {
                if (Random.Range(0, 10) == 0)
                {
                    selectedPositions.Add(i);
                }
            }

            //  최종적으로 선택된 위치에 노드 생성
            foreach (int i in selectedPositions)
            {
                CreateNode(i, j);
            }

            foreach (int i in possiblePositions)
            {
                if (i == 0 && nodes[i + 1, j] == null)
                {
                    CreateNode(i, j);
                }
                if (i > 0 && i < width - 1 && (nodes[i + 1, j] == null || nodes[i - 1, j] == null))
                {
                    CreateNode(i, j);
                }
                if (i == width - 1 && nodes[i - 1, j] == null)
                {
                    CreateNode(i, j);
                }
            }

        }
    }






    void CreateNode(int i, int j)
    {
        nodes[i, j] = Instantiate(nodePrefab, nodeSet.transform) as GameObject;
        nodes[i, j].GetComponent<Node>().mapManager = this;
        nodes[i, j].GetComponentInChildren<Node>().nodeCoordinate[0] = i;
        nodes[i, j].GetComponentInChildren<Node>().nodeCoordinate[1] = j;
        nodes[i, j].transform.position = new Vector2(i * 100 + 100, j * 100 + 50);
    }

    void DrawLine()
    {

        int width = nodes.GetLength(0);  // 가로 크기
        int height = nodes.GetLength(1); // 세로 크기

        for (int j = 0; j < height - 1; j++) // 현재 줄(j)과 다음 줄(j+1)을 연결
        {
            for (int i = 0; i < width; i++) // 현재 줄의 모든 노드 순회
            {
                GameObject startNode = nodes[i, j];

                // 노드 없음
                if (startNode == null)
                    continue; 

                // 1. 바로 다음 줄(j+1)의 노드 연결 확인 (정면)
                if (nodes[i, j + 1] != null)
                {
                    SetLine(startNode.transform, nodes[i, j + 1].transform);
                }

                // 2. 왼쪽 대각선 연결 확인
                if (i > 0) 
                {
                    if (nodes[i - 1, j + 1] != null)
                    {
                        SetLine(startNode.transform, nodes[i - 1, j + 1].transform);
                    }
                }

                // 3. 오른쪽 대각선 연결 확인
                if (i < width - 1) 
                {
                    if (nodes[i + 1, j + 1] != null)
                    {
                        SetLine(startNode.transform, nodes[i + 1, j + 1].transform);
                    }
                }
            }
        }
    }
    /*
    void SetLine(Transform t1, Transform t2)
    {
        GameObject lineObject = Instantiate(linePrefab, lineSet.transform);
        lineObjects.Add(lineObject);

        LineRenderer lineRenderer = lineObject.GetComponent<LineRenderer>();

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, t1.position);
        lineRenderer.SetPosition(1, t2.position);
    }
    */
    void SetLine(Transform startNodeTransform, Transform endNodeTransform)
    {
        // 1. LinePrefab (UI Image 기반)을 Instantiate하고 lineSet (Canvas 자식)의 자식으로 설정
        // lineSet은 현재 Canvas 하위에 있고, Render Mode가 Overlay 또는 Camera라고 가정합니다.
        GameObject lineObject = Instantiate(linePrefab, lineSet.transform);
        lineObjects.Add(lineObject);

        // RectTransform을 가져옵니다.
        RectTransform lineRect = lineObject.GetComponent<RectTransform>();
        if (lineRect == null) return;

        // 2. 노드 위치를 RectTransform 기준으로 가져옵니다.
        // Canvas 좌표계에서 두 점의 위치를 가져옵니다.
        Vector3 startPos = startNodeTransform.localPosition;
        Vector3 endPos = endNodeTransform.localPosition;

        // 3. 거리와 각도 계산
        float distance = Vector3.Distance(startPos, endPos);
        Vector3 direction = (endPos - startPos).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // 4. Line의 RectTransform 설정
        lineRect.pivot = new Vector2(0, 0.5f); // 피봇을 왼쪽 중앙으로 설정
        lineRect.anchorMin = new Vector2(0, 0); // 앵커는 필요에 따라 설정, 여기선 단순화
        lineRect.anchorMax = new Vector2(0, 0);

        // 시작 위치에 Line의 로컬 위치를 설정
        lineRect.localPosition = startPos;

        // 길이와 회전 설정
        lineRect.sizeDelta = new Vector2(distance, 2f); // 길이 설정
        lineRect.localRotation = Quaternion.Euler(0, 0, angle);          // 회전 설정
    }
}

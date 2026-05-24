using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapManager : MonoBehaviour
{
    public GameObject nodePrefab;
    public GameObject destNodePrefab;
    public GameObject destNode;
    public GameObject canvas;
    public GameObject[,] nodes = new GameObject[7, 15];
    public GameObject nodeSet;
    public GameObject linePrefab;
    private List<GameObject> lineObjects = new List<GameObject>();
    public GameObject lineSet;
    public Node currentNode;
    public Node logicalStartNode;
    public GameObject map;
    public int stage;
    public GameObject upperBound, lowerBound;

    private void Start()
    {
        logicalStartNode.mapManager = this;
        currentNode = logicalStartNode;
        List<LineRenderer> lineArray = new List<LineRenderer>();
        MapCreate();
        OnNodeSelected(currentNode);    

    }
    private void Update()
    {
        //마우스가 바운더리 위나 아래로 가면 맵 이동 
        Vector3 mousePos = Input.mousePosition;
        // 마우스 좌표를 월드 좌표로 변환하여 비교
        Vector3 worldMousePos = Camera.main.ScreenToWorldPoint(mousePos);

        if (worldMousePos.y > upperBound.transform.position.y)
        {
            if(nodeSet.transform.localPosition.y > -1500f)
            // 위로 이동
            MoveMap(Vector3.down);
        }
        else if (worldMousePos.y < lowerBound.transform.position.y)
        {
            if (nodeSet.transform.localPosition.y < 0f)
                // 아래로 이동
                MoveMap(Vector3.up);
        }
    }


    // index 일반 전투 스테이지 0 정예 전투 스테이지 1 이벤트 스테이지 2 상점 스테이지 3
    void MapCreate()
    {
        int width = nodes.GetLength(0);
        int height = nodes.GetLength(1);


        List<int> firstRow = new List<int> { 0, 1, 2, 3, 4, 5, 6 };   

        for(int i = 0; i < 6; i++)
        {
            int t = 0;
            //첫번째 줄의 노드를 선택. 이때, 처음 선택하는 두개는 중첩되면 안됨.
            if (i == 0 || i == 1)
            {
                int randomIndex = Random.Range(0, firstRow.Count);
                t = firstRow[randomIndex];  
                firstRow.RemoveAt(randomIndex);
                CreateNode(t, 0);
                nodes[t,0].GetComponent<Node>().isSelectable = true;
            }
            else
            {
                t = Random.Range(0,width);
                CreateNode(t, 0);
                nodes[t, 0].GetComponent<Node>().isSelectable = true;
            }
            logicalStartNode.nextNodes.Add(nodes[t, 0].GetComponent<Node>());
            // Node[t,0] 에서 시작하는 경로 생성
            for(int j = 1; j < height; j++)
            {
                int previousNode = t;
                if (t == 0)
                    t = Random.Range(0, 2);
                else if (t == width - 1)
                    t = Random.Range(width - 2, width);
                else
                    t = Random.Range(t - 1, t + 2);
                CreateNode(t, j);
                SetLine(nodes[previousNode, j - 1].transform, nodes[t, j].transform);
                
            }
        }
        //보스 노드 생성 
        CreateNode(-1, 0);
        for(int i = 0; i < width; i++)
        {
            if (nodes[i, 14] != null)
            {
                SetLine(nodes[i,14].transform, destNode.transform);
            }
        }
    }

   
    void CreateNode(int i, int j)
    {
        if(i == -1)
        {
            destNode = Instantiate(destNodePrefab, nodeSet.transform);
            destNode.GetComponent<Node>().mapManager = this;
            destNode.GetComponentInChildren<Node>().nodeCoordinate[0] = 3;
            destNode.GetComponentInChildren<Node>().nodeCoordinate[1] = 15;
            float xPos = (3 - (nodes.GetLength(0) / 2)) * 75f;
            destNode.transform.localPosition = new Vector3(xPos, 15 * 100, 0);
        }
        else if(nodes[i, j] == null)
        {
            nodes[i, j] = Instantiate(nodePrefab, nodeSet.transform) as GameObject;
            nodes[i, j].GetComponent<Node>().mapManager = this;
            nodes[i, j].GetComponentInChildren<Node>().nodeCoordinate[0] = i;
            nodes[i, j].GetComponentInChildren<Node>().nodeCoordinate[1] = j;
            float xPos = (i - (nodes.GetLength(0) / 2)) * 75f;
            float yPos = j * 100f;

            nodes[i, j].transform.localPosition = new Vector3(xPos, yPos, 0);
        }

    }


    void SetLine(Transform startNodeTransform, Transform endNodeTransform)
    {
        GameObject lineObject = Instantiate(linePrefab, lineSet.transform);
        lineObjects.Add(lineObject);
        RectTransform lineRect = lineObject.GetComponent<RectTransform>();
        if (lineRect == null) return;

        Vector3 startPos = startNodeTransform.localPosition;
        Vector3 endPos = endNodeTransform.localPosition;

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
        
        //다음 노드의 정보 담은 인접리스트 정보를 채워준다. 나중에 이동 가능한 Node highlight하기 위해서 
        startNodeTransform.GetComponent<Node>().nextNodes.Add(endNodeTransform.GetComponent<Node>());
    }

    void MoveMap(Vector3 dir)
    {
            lineSet.transform.Translate(dir);
            nodeSet.transform.Translate(dir);

    }

    public void OnNodeSelected(Node targetNode)
    {
        // 1. 기존 하이라이트 모두 끄기 (현재 가능한 노드들만)
        foreach (Node next in currentNode.nextNodes)
        {
            next.SetHighlight(false);
        }

        // 2. 위치 이동
        currentNode = targetNode;

        // 3. 새 위치에서 갈 수 있는 다음 노드들 하이라이트
        foreach (Node next in currentNode.nextNodes)
        {
            next.SetHighlight(true);
        }

    }
}

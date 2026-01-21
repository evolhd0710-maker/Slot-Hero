using UnityEngine;

[CreateAssetMenu(fileName = "TestEnemyData", menuName = "Scriptable Objects/TestEnemyData")]
public class TestEnemyData : ScriptableObject
{
    //현재는 임의로 입력. 나중에 csv 파일에서 불러오도록 수정할 필요 있음.
    public int enemyID;
    public string enemyName;
    public float baseHealth;
}

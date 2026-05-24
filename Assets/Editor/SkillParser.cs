#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
//스킬 데이터를 CSV에서 파싱하는 코드 
public class SkillCardParser
{
    [MenuItem("Tools/Parse Skill CSV")]
    public static void ParseCSV()
    {
        string csvPath = Path.Combine(Application.dataPath, "Skills/SkillData.csv");
        string targetFolder = "Assets/Skills/SkillAssests";

        if (!File.Exists(csvPath))
        {
            Debug.LogError($"[실패] CSV 파일을 찾을 수 없습니다! 경로를 확인하세요: {csvPath}");
            return;
        }

        string[] lines = File.ReadAllLines(csvPath);
        if (lines.Length <= 1)
        {
            Debug.LogWarning("CSV 파일에 파싱할 데이터 행이 존재하지 않습니다.");
            return;
        }

        string systemFolderPath = Path.Combine(Application.dataPath, "Skills/SkillAssets");
        if (!Directory.Exists(systemFolderPath))
        {
            Directory.CreateDirectory(systemFolderPath);
        }

        int successCount = 0;

        for (int i = 3; i < lines.Length; i++)
        {
            string csvLine = lines[i];
            if (string.IsNullOrWhiteSpace(csvLine)) continue;

            string[] data = csvLine.Split(',');

            SkillSO newSkill = ScriptableObject.CreateInstance<SkillSO>();

            newSkill.skillId = int.Parse(data[1]);
            newSkill.skillName = data[2];

            newSkill.coEff = new float[5];
            for (int j = 0; j < 5; j++)
            {
                newSkill.coEff[j] = float.Parse(data[j + 5])/10000;
            }

            for (int j = 11; j < 15; j++)
            {
                if (j < data.Length && !string.IsNullOrWhiteSpace(data[j]))
                {
                    string[] effectToken = data[j].Split(':');

                    if (effectToken.Length < 4) continue;

                    string effectTargetType = effectToken[0].Trim();
                    string effectName = effectToken[1].Trim();
                    string effectNumType = effectToken[2].Trim();
                    int effectChance = int.Parse(effectToken[3]); 

                    string effectPath = $"Assets/Skills/Effects/{effectName}Effect.asset";
                    SkillEffect blueprint = AssetDatabase.LoadAssetAtPath<SkillEffect>(effectPath);

                    if (blueprint != null)
                    {
                        SkillSO.EffectContainer container = new SkillSO.EffectContainer();
                        container.effectBlueprint = blueprint;
                        container.effectName = effectName;
                        
                        System.Enum.TryParse<EffectTargetType>(effectTargetType, true, out container.targetType);
                        System.Enum.TryParse<VariableType>(effectNumType, true, out container.flexAmount.valueType);

                        newSkill.effects.Add(container);
                    }
                    else
                    {
                        Debug.LogWarning($"{effectName} 리소스 {effectPath} 에 없음 {newSkill.skillName}");
                    }
                }
            }


            string savePath = $"Assets/Skills/SkillAssets/Skill_{newSkill.skillId}.asset";
            AssetDatabase.CreateAsset(newSkill, savePath);

            EditorUtility.SetDirty(newSkill);
            successCount++;
        }


        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"{successCount} skill loaded");
    }
}
#endif
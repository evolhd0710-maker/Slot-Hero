#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
//스킬에 포함된 Effect 를 파싱하는 코드

public class EffectParser
{
    [MenuItem("Tools/Parse Effect CSV")]
    public static void ParseCSV()
    {
        string csvPath = Path.Combine(Application.dataPath, "Skills/EffectData.csv");
        string targetFolder = "Assets/Skills/Effects";

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

        if (!Directory.Exists(targetFolder))
        {
            Directory.CreateDirectory(targetFolder);
        }

        int successCount = 0;

        for(int i = 3;  i < lines.Length; i++)
        {
            string csvLine = lines[i];

            if (string.IsNullOrWhiteSpace(csvLine)) continue;

            string[] data = csvLine.Split(',');

            if (data.Length < 11 || string.IsNullOrWhiteSpace(data[1])) continue;

            EffectClassType parsedEffectClassType;
            if (!System.Enum.TryParse<EffectClassType>(data[3].Trim(), true, out parsedEffectClassType))
            {
                Debug.LogWarning($"[스킵] {i + 1}번째 행의 효과 종류가 올바르지 않습니다: {data[3]}");
                continue;
            }


            EffectSO newEffect = null;

            switch(parsedEffectClassType)
            {
                case EffectClassType.InstantDamage : 
                    newEffect = ScriptableObject.CreateInstance<InstantDamageEffect>();
                    break;
                case EffectClassType.DotDamage :
                    newEffect = ScriptableObject.CreateInstance<DotDamageEffect>();
                    break;
                case EffectClassType.StatBuff :
                    newEffect = ScriptableObject.CreateInstance<StatBuffEffect>();
                    break;
            }

            if (newEffect == null)
            {
                Debug.LogWarning($"[스킵] {i + 1}번째 행: {parsedEffectClassType}에 대응하는 실제 C# 클래스가 생성되지 않았습니다.");
                continue;
            }

            newEffect.effectId = int.Parse(data[0]);
            newEffect.effectCode = data[1];
            System.Enum.TryParse<EffectClassType>(data[3],true, out newEffect.effectClassType);
            System.Enum.TryParse<DurationType>(data[4],true,out newEffect.durationType);
            System.Enum.TryParse<TickTiming>(data[5], true, out newEffect.tickTiming);
            System.Enum.TryParse<StackType>(data[6], true, out newEffect.stackType);
            newEffect.maxStacks = int.Parse(data[7]);
            System.Enum.TryParse<DecayType>(data[8], true, out newEffect.decayType);
            newEffect.decayValue = int.Parse(data[9]);
            System.Enum.TryParse<DecayTiming>(data[10], true, out newEffect.decayTiming);

            string savePath = $"{targetFolder}/Effect_{newEffect.effectCode}.asset";
            AssetDatabase.CreateAsset(newEffect, savePath);
            EditorUtility.SetDirty(newEffect);
            successCount++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"{successCount} Effect loaded");
    }
}
#endif
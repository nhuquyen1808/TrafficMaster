using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace DevDuck
{
    [Serializable]
    public class LevelData
    {
        public int level;
        public string type;
        public List<Vector3> positions = new List<Vector3>();
        public List<string> directions = new List<string>();
    }
    public class SaveData : MonoBehaviour
    {
       [SerializeField] private int currentLevel;
        public List<Car> carsSaved = new List<Car>();
        
        public List<Vector3> positionsLoaded = new List<Vector3>();
        public List<string> directionsLoaded = new List<string>();
        public void SaveToJson()
        {
            LevelData data = new LevelData();
            data.level = currentLevel;
            data.positions = positionsLoaded;
            data.directions = directionsLoaded;
            string json = JsonUtility.ToJson(data);
            //  File.WriteAllText(Application.persistentDataPath + "/save.json", json);
            File.WriteAllText($"Assets/Resources/Levels/DataLevel_{data.level}.json", json);
        }
        public void UpdateJson()
        {
            positionsLoaded.Clear();
            for (int i = 0; i < carsSaved.Count; i++)
            {
                if (carsSaved[i].gameObject.activeSelf)
                {
                    positionsLoaded.Add(carsSaved[i].transform.position);
                }
            }
        }

        public LevelData LoadFromJson()
        {
            LevelData result = new LevelData();
            string data = "";
#if UNITY_EDITOR
            data = File.ReadAllText($"Assets/Resources/Levels/DataLevel_{currentLevel}.json");
#else
            data = File.ReadAllText($"Assets/Resources/Levels/DataLevel_{currentLevel}.json");
#endif
            result = JsonUtility.FromJson<LevelData>(data);
            positionsLoaded = result.positions;
            return result;
        }
        public List<Vector3> LoadLevelPositionGame16(int level)
        {
            LevelData result = new LevelData();
            string data = "";
#if UNITY_EDITOR
                data = File.ReadAllText("Assets/Resources/Art/game16/LevelData_16/DataLevel_5.json");
#else
          
                data = Resources.Load<TextAsset>($"Art/game16/LevelData_16/DataLevel_5").ToString();
#endif
            result = JsonUtility.FromJson<LevelData>(data);
            positionsLoaded = result.positions;
            return positionsLoaded;
        }
    }
    
#if UNITY_EDITOR
    [CustomEditor(typeof(SaveData), true)]
    public class SetMapEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            SaveData saveData = (SaveData)target;
            if (GUILayout.Button("Update Data"))
            {
                saveData.UpdateJson();
            }
            if (GUILayout.Button("Save Data"))
            {
                saveData.SaveToJson();
            }
            if (GUILayout.Button("Read Data"))
            {
                LevelData data = saveData.LoadFromJson();
                Debug.Log("3");
            }
        }
    }
#endif
}

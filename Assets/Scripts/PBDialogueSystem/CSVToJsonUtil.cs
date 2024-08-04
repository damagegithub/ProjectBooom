using System.Collections.Generic;
using System.Xml;
using UnityEngine;  
using Newtonsoft.Json;
using Formatting = Newtonsoft.Json.Formatting;

namespace PBDialogueSystem
{
    public class CSVToJsonUtil: MonoBehaviour
    {
        public static List<T> GetJsonData<T>(string csvFilePath)
        {
            TextAsset csvFile = Resources.Load<TextAsset>(csvFilePath);
            List<T> T_instances = new List<T>();
            if (csvFile != null)
            {
                // 将CSV内容转化为JSON
                string json = ConvertCSVToJson(csvFile.text);

                // 将JSON转化为类
                T_instances = JsonConvert.DeserializeObject<List<T>>(json);
            }
            else
            {
                Debug.LogError("CSV file not found!");
            }

            return T_instances;
        }
        
        private static string ConvertCSVToJson(string csv)
        {
            string[] lines = csv.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length <= 1) return null;

            string[] headers = lines[0].Split(new char[] { ',' }, System.StringSplitOptions.RemoveEmptyEntries);
            List<Dictionary<string, string>> rows = new List<Dictionary<string, string>>();

            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(new char[] { ',' }, System.StringSplitOptions.None);
                Dictionary<string, string> row = new Dictionary<string, string>();

                for (int j = 0; j < headers.Length; j++)
                {
                    row[headers[j]] = values[j];
                }

                rows.Add(row);
            }
            return JsonConvert.SerializeObject(rows, Formatting.Indented);
        }
    }
}
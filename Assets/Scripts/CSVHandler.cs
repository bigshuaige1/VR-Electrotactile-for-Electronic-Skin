using System;
using System.IO;
using System.Text;
using UnityEngine;
using System.Collections.Generic;

static class CSVHandler
{
    public static void WriteCSV(string filePath, List<List<string>> data, bool append = false)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath, append, Encoding.UTF8))
            {
                foreach (var row in data)
                {
                    string line = string.Join(",", row);
                    writer.WriteLine(line);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error writing CSV file: " + e.Message);
            throw e;
        }
    }

    public static List<string[]> ReadCSV(string filePath)
    {
        List<string[]> rows = new List<string[]>();

        try
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    // 将每行数据按逗号分割并存入数组
                    string[] values = line.Split(',');
                    rows.Add(values);
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error reading CSV file: " + e.Message);
        }

        return rows;
    }
}

﻿using System.Diagnostics;

namespace UI
{
    public class EnvLoader
    {
        public static void Load(string filePath)
        {
            Debug.WriteLine("fewipnsf8uesb8fso8fbsifb7es7ibf8isbf8isvbfs8ifebse8if");

            if (!File.Exists(filePath))
                return;

            foreach (var line in File.ReadAllLines(filePath))
            {
                var parts = line.Split(
                    '=',
                    StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length != 2)
                    continue;

                Environment.SetEnvironmentVariable(parts[0], parts[1]);
            }
        }
    }
}

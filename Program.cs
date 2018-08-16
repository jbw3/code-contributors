﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace code_contributors
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, long> contributorCounts = new Dictionary<string, long>();

            string line = Console.ReadLine();
            while (line != null)
            {
                int idx = 1;
                while (idx < line.Length && !(line[idx - 1] == ' ' && line[idx] == '('))
                {
                    ++idx;
                }
                ++idx;
                int nameStartIdx = idx;

                idx += 4;
                while (idx < line.Length && !(line[idx - 4] == ' ' && char.IsDigit(line[idx - 3]) && char.IsDigit(line[idx - 2]) && char.IsDigit(line[idx - 1]) && char.IsDigit(line[idx])))
                {
                    ++idx;
                }
                int nameEndIdx = idx - 4;

                if (nameStartIdx < line.Length && nameEndIdx > nameStartIdx)
                {
                    string name = line.Substring(nameStartIdx, nameEndIdx - nameStartIdx).TrimEnd();

                    try
                    {
                        ++contributorCounts[name];
                    }
                    catch (KeyNotFoundException)
                    {
                        contributorCounts[name] = 1;
                    }
                }

                line = Console.ReadLine();
            }

            long totalCount = 0;
            foreach (KeyValuePair<string, long> pair in contributorCounts.OrderBy(p => p.Key))
            {
                Console.WriteLine("{0}: {1}", pair.Key, pair.Value);
                totalCount += pair.Value;
            }
            Console.WriteLine("Total: {0}", totalCount);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;

namespace code_contributors
{
    class Program
    {
        static Dictionary<string, ulong> ReadFiles()
        {
            Dictionary<string, ulong> contributorCounts = new Dictionary<string, ulong>();

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

            return contributorCounts;
        }

        static Dictionary<string, ulong> CombineAliases(Dictionary<string, ulong> contributorCounts, Dictionary<string, List<string>> aliases)
        {
            foreach (KeyValuePair<string, List<string>> pair in aliases)
            {
                ulong count;
                try
                {
                    count = contributorCounts[pair.Key];
                }
                catch (KeyNotFoundException)
                {
                    count = 0;
                }

                foreach (string alias in pair.Value)
                {
                    try
                    {
                        count += contributorCounts[alias];
                        contributorCounts.Remove(alias);
                    }
                    catch (KeyNotFoundException)
                    {
                    }
                }

                contributorCounts[pair.Key] = count;
            }

            return contributorCounts;
        }

        static void Main(string[] args)
        {
            Dictionary<string, ulong> contributorCounts = ReadFiles();

            Dictionary<string, List<string>> aliases = new Dictionary<string, List<string>>
            {
            };

            contributorCounts = CombineAliases(contributorCounts, aliases);

            int maxNameLen = "Total".Length;
            int maxCountLen = 0;
            ulong totalCount = 0;
            foreach (KeyValuePair<string, ulong> pair in contributorCounts)
            {
                maxNameLen = Math.Max(maxNameLen, pair.Key.Length);
                maxCountLen = Math.Max(maxCountLen, pair.Value.ToString().Length);
                totalCount += pair.Value;
            }
            maxCountLen = Math.Max(maxCountLen, totalCount.ToString().Length);

            foreach (KeyValuePair<string, ulong> pair in contributorCounts.OrderBy(p => p.Value).Reverse())
            {
                string name = pair.Key.PadRight(maxNameLen);
                string count = pair.Value.ToString().PadLeft(maxCountLen);
                double percentage = pair.Value / (double)totalCount;
                Console.WriteLine("{0} {1} {2,6:0.0%}", name, count, percentage);
            }
            Console.WriteLine("{0} {1} 100.0%", "Total".PadRight(maxNameLen), totalCount.ToString().PadLeft(maxCountLen));
        }
    }
}

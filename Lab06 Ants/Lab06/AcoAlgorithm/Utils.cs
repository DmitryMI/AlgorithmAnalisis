using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab06.AcoAlgorithm
{
    class Utils
    {
        public static List<int> Intersect(ICollection<int> collectionA, ICollection<int> collectionB)
        {
            List<int> result = new List<int>();

            for (int i = 0; i < collectionA.Count; i++)
            {
                int element = collectionA.ElementAt(i);
                if(!collectionB.Contains(element))
                    result.Add(element);
            }

            return result;
        }

        public static string PathToString(ICollection<int> path)
        {
            string result = "";
            foreach (int city in path)
            {
                result += city + " ";
            }
            return result;
        }

        public static string ParamsToTable(ICollection<AcoProcessor.AcoParameters> paramList, ICollection<double> results)
        {
            // Draw labels
            string result = "\nalpha\tbeta\tro\ttMax\tResult\n";

            for (int i = 0; i < paramList.Count; i++)
            {
                AcoProcessor.AcoParameters param = paramList.ElementAt(i);
                result += param.alpha + "\t" + param.beta + "\t" + param.p + "\t" + param.tMax + "\t";
                if (results != null)
                    result += results.ElementAt(i).ToString("F");
                result += '\n';
            }

            return result;
        }
    }
}

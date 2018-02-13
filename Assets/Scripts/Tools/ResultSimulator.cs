using com.tangelogames.extensions.model.vo;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Assets.Scripts.Tools
{
    class ResultSimulator
    {
        static SpinResultVO sr;

        public static SpinResultVO randomResult()
        {
            sr = new SpinResultVO();

            randomSymbols();
            generateLine();

            return sr;

        }

        static void generateLine()
        {
            

            
            LineWinResult line = new LineWinResult();
            line.wonAmount = Random.Range(0, 100);
            line.lineNumber = Random.Range(0, 9);
            line.streak = Random.Range(2, 5);
            for (int i = 0; i < line.streak; i++)
            {
                line.symbolIndexes += Random.Range(0,3) + i * 3  + ",";
            }
            sr.lines.Add(line);
        }
        static void randomSymbols( )
        {
            List<int> symbol_list = new List<int>();
            for (int i = 0; i < 15; i++)
            {
                symbol_list.Add(Random.Range(0, 11));
            }
            sr.symbolsList = symbol_list.ToArray();
        }

    }
}

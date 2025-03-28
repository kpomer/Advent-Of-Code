using System;
using System.Collections;

namespace AdventOfCode
{
    public class Program
    {
        static void Main(string[] args)
        {

            List<long[]> machineRules = ParseFileData(); //input from PuzzleInput1.txt

            long sumTokens = 0;
            int r = 0;
            while(r<machineRules.Count){
                long[] eq1 = machineRules[r];
                r+=1;
                long[] eq2 = machineRules[r];

                // //Part 2
                eq1[2] = eq1[2] + 10000000000000;
                eq2[2] = eq2[2] + 10000000000000;
                //
                
                long a = 0;
                long b = 0;

                if (SolveValues(eq1, eq2, ref a, ref b)){
                    sumTokens += 3*a + b;
                }

                r += 1;
            }

            Console.WriteLine($"Minimum Tokens: {sumTokens}");
            
        }


        static bool SolveValues(long[] pEq1, long[] pEq2, ref long pReturnA, ref long pReturnB){

            Double[] newEq1 = new Double[3];
            Double[] newEq2 = new Double[3];

            if(pEq1[2] > pEq2[2]){
                newEq1[0] = pEq1[0] * pEq2[1];
                newEq1[1] = pEq1[1] * pEq2[1];
                newEq1[2] = pEq1[2] * pEq2[1];

                newEq2[0] = pEq2[0] * pEq1[1];
                newEq2[1] = pEq2[1] * pEq1[1];
                newEq2[2] = pEq2[2] * pEq1[1];
            }
            else{
                newEq1[0] = pEq2[0] * pEq1[1];
                newEq1[1] = pEq2[1] * pEq1[1];
                newEq1[2] = pEq2[2] * pEq1[1];

                newEq2[0] = pEq1[0] * pEq2[1];
                newEq2[1] = pEq1[1] * pEq2[1];
                newEq2[2] = pEq1[2] * pEq2[1];
            }

            Double valA = (newEq1[2] - newEq2[2])/(newEq1[0] - newEq2[0]);
            if(valA % 1 == 0){
                //Whole Number!
                pReturnA = Convert.ToInt64(valA);
                
                Double valB = (pEq2[2] - (pEq2[0]*pReturnA))/pEq2[1];
                if(valB % 1 == 0){
                    //Whole Number!
                    pReturnB = Convert.ToInt64(valB);
                }
                else{
                    return false;
                }
            }

            return true;
        }


        static List<long[]> ParseFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string fileName = "PuzzleInput1.txt";

            string[] lines = File.ReadAllLines(string.Concat(fileDataDir, "/", fileName));

            List<long[]> machineRules = new List<long[]>();
            for(int l = 0; l<lines.Length; l++){
                if(lines[l].Length > 9 && lines[l].Substring(0, 9) == "Button A:"){
                    long[] m1 = new long[3];
                    long[] m2 = new long[3];

                    string[] lineSplit = lines[l].Split(' ');
                    m1[0] = long.Parse(lineSplit[2].Replace("X+", "").Replace(",",""));
                    m2[0] = long.Parse(lineSplit[3].Replace("Y+", "").Replace(",",""));

                    l+=1;
                    lineSplit = lines[l].Split(' ');
                    m1[1] = long.Parse(lineSplit[2].Replace("X+", "").Replace(",",""));
                    m2[1] = long.Parse(lineSplit[3].Replace("Y+", "").Replace(",",""));

                    l+=1;
                    lineSplit = lines[l].Split(' ');
                    m1[2] = long.Parse(lineSplit[1].Replace("X=", "").Replace(",",""));
                    m2[2] = long.Parse(lineSplit[2].Replace("Y=", "").Replace(",",""));
                    
                    machineRules.Add(m1);
                    machineRules.Add(m2);

                }

            }

            return machineRules;

        }

    }
}
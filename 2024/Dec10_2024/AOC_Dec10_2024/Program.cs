using System;
using System.Collections;
using System.Configuration.Assemblies;

namespace AdventOfCode
{
    public class Program
    {
        public static int[,] FileData = new int[0,0];
        public static List<string> Trails = new List<string>();
        public static int verticalCount = 0;
        public static int horizontalCount = 0;
        static void Main(string[] args)
        {

            FileData = GetFileData(); //int[,] coordinates of all values from PuzzleInput1.txt

            for(int v = 0; v < verticalCount; v++){
                for(int h = 0; h < horizontalCount; h++){
                    if(FileData[v,h] == 0){
                        //Possible Trailhead
                        string newTrailString = "";
                        FindNextStep(v, h, FileData[v,h], newTrailString);
                    }
                }
            }

            int countScores = 0; //number of trails starting from one point that end at a DIFFERENT point
            HashSet<string> uniqueTrails = new HashSet<string>();
            foreach(string trail in Trails){
                string[] trailPoints = trail.Split(';');
                string tStartEnd = String.Concat(trailPoints[0], ";", trailPoints[9]);
                if(uniqueTrails.Contains(tStartEnd) == false){
                    uniqueTrails.Add(tStartEnd);
                    countScores += 1;
                }
            }

            Console.WriteLine($"Total Scores {countScores}");
            Console.WriteLine($"Total Ratings {Trails.Count}"); //Total number of trails
            
        }

        static void FindNextStep(int pCurV, int pCurH, int pCurHeight, string pNewTrailString){

            pNewTrailString = String.Concat(pNewTrailString, $"[{pCurV},{pCurH}];");

            if(pCurHeight == 9){
                //Full trail found!
                pNewTrailString = pNewTrailString.Remove(pNewTrailString.Length-1); //remove last ';'
                Trails.Add(pNewTrailString);
                return;
            }

            //Find next steps that meet criteria (coordinate is on map AND value is pCurStepVal + 1)
            List<int[]> nextSteps = NextStepCoordinates(pCurV, pCurH, pCurHeight);
            foreach(int[] step in nextSteps){
                int nextV = step[0];
                int nextH = step[1];
                FindNextStep(nextV, nextH, FileData[nextV,nextH], pNewTrailString);
            }
        }

        static List<int[]> NextStepCoordinates(int pCurV, int pCurH, int pCurHeight){
            List<int[]> nextSteps = new List<int[]>();

            int nextV = 0;
            int nextH = 0;
            
            //Check Up
            nextV = pCurV - 1;
            nextH = pCurH;
            if(ValidNextStep(nextV, nextH, pCurHeight)){
                nextSteps.Add([nextV,nextH]);
            }

            //Check Right
            nextV = pCurV;
            nextH = pCurH + 1;
            if(ValidNextStep(nextV, nextH, pCurHeight)){
                nextSteps.Add([nextV,nextH]);
            }

            //Check Down
            nextV = pCurV + 1;
            nextH = pCurH;
            if(ValidNextStep(nextV, nextH, pCurHeight)){
                nextSteps.Add([nextV,nextH]);
            }
            
            //Check Left
            nextV = pCurV;
            nextH = pCurH - 1;
            if(ValidNextStep(nextV, nextH, pCurHeight)){
                nextSteps.Add([nextV,nextH]);
            }

            return nextSteps;
        }

        static bool ValidNextStep(int pNextV, int pNextH, int pCurHeight){

            return (pNextV >= 0 && pNextV < verticalCount && pNextH >= 0 && pNextH < horizontalCount && FileData[pNextV,pNextH] == pCurHeight + 1);
        }

        static int[,] GetFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string fileName = "PuzzleInput1.txt";

            string[] fileStringArr = File.ReadAllLines(string.Concat(fileDataDir, "/", fileName)); 

            verticalCount = fileStringArr.Count<string>();
            horizontalCount = fileStringArr[0].ToString().Length;

            int[,] heights = new int[verticalCount,horizontalCount];

            for(int v = 0; v < verticalCount; v++){
                string line = fileStringArr[v].ToString();
                char[] lineStrArr = line.ToCharArray();

                for(int h = 0; h < line.Length; h++){
                    heights[v,h] = int.Parse(lineStrArr[h].ToString());
                }
            }

            return heights;

        }

    }
}
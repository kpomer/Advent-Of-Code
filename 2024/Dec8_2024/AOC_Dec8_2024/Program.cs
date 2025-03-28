using System;
using System.Collections;
using System.IO.Pipes;
using System.Reflection.Emit;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks.Dataflow;

namespace AdventOfCode
{

    public class Program
    {
        public static char[,] FileData = new char[0,0];
        public static int verticalCount = 0;
        public static int horizontalCount = 0;

        static void Main(string[] args)
        {

            FileData = GetFileData();
            long countAntinodes = 0;
            for(int v = 0; v < verticalCount; v++){
                for(int h = 0; h < horizontalCount; h++){
                    if(CheckAntinodes(v, h)){
                        countAntinodes += 1;
                    }
                }
            }

            Console.WriteLine($"Antinode count: {countAntinodes}");

        }

        static bool CheckAntinodes(int pVert, int pHoriz){
            
            int cV = 0; //change in Vertical
            int cH = 0; //change in Horizontal
            for(int v = 0; v < verticalCount; v++){
                for(int h = 0; h < horizontalCount; h++){

                    if((v != pVert || h != pHoriz) && (FileData[v, h] != '.')){
                        //Not the same point, point is not blank
                        cV = v - pVert;
                        cH = h - pHoriz;
                        List<int[]> otherANodeCoor = GetOtherANodePairs(cV, cH);

                        foreach(int[] possibleANodeVH in otherANodeCoor){

                            int v2 = pVert + possibleANodeVH[0];
                            int h2 = pHoriz + possibleANodeVH[1]; 

                            if((v2 >= 0) && (v2 < verticalCount) && (h2 >= 0) && (h2 < horizontalCount)){
                                if(FileData[v,h] == FileData[v2,h2]){
                                    return true;
                                }
                            }
                        }
                    }

                }
            }

            return false;
            
        }

        static List<int[]> GetOtherANodePairs(int pChgV, int pChgH){
            List<int[]> aNodePairs = new List<int[]>();

            //### Part 1 ######################################
            // if((pChgV % 2 == 0) && (pChgH % 2 == 0)){
            //     //Add Half Values
            //     aNodePairs.Add([-1*pChgV/2, -1*pChgH/2]);
            // }

            // //Add Double Values
            // aNodePairs.Add([pChgV*2, pChgH*2]);
            //##################################################


            // //### Part 2 ######################################
            
            //Determine Smallest Incrememt using Greatest Common Factor
            int chgV_MIN = pChgV;
            int chgH_MIN = pChgH;
            int gcf = GCF(chgV_MIN, chgH_MIN);
            if(gcf > 1){
                chgV_MIN = chgV_MIN/gcf;
                chgH_MIN = chgH_MIN/gcf;
            }

            //Add Positive Values
            int chgV = chgV_MIN;
            int chgH = chgH_MIN;
            while((Math.Abs(chgV) < verticalCount) && (Math.Abs(chgH) < horizontalCount)){
                if((chgV != pChgV) || (chgH != pChgH)){
                    aNodePairs.Add([chgV, chgH]);
                }
                chgV += chgV_MIN;
                chgH += chgH_MIN;
            }

            //Add Negative Values
            chgV = chgV_MIN*-1;
            chgH = chgH_MIN*-1;
            while((Math.Abs(chgV) < verticalCount) && (Math.Abs(chgH) < horizontalCount)){
                if((chgV != pChgV) || (chgH != pChgH)){
                    aNodePairs.Add([chgV, chgH]);
                }
                chgV += chgV_MIN;
                chgH += chgH_MIN;
            }
            //##################################################

            return aNodePairs;

        }

        static int GCF(int i1, int i2){
            //Determine Greatest Common Factor
            i1 = Math.Abs(i1);
            i2 = Math.Abs(i2);

            int start = Math.Min(i1, i2);

            for(int d = start; d > 0; d--){
                if((i1 % d == 0) && (i2 % d == 0)){
                    return d;
                }
            }

            return 0;
        }



        static char[,] GetFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string fileName = "PuzzleInput1.txt";

            string[] fileStringArr = File.ReadAllLines(string.Concat(fileDataDir, "/", fileName)); 

            verticalCount = fileStringArr.Count<string>();
            horizontalCount = fileStringArr[0].ToString().Length;
            char[,] characters = new char[verticalCount,horizontalCount];

            for(int v = 0; v < verticalCount; v++){
                string line = fileStringArr[v].ToString();
                char[] lineStrArr = line.ToCharArray();

                for(int h = 0; h < line.Length; h++){
                    characters[v,h] = lineStrArr[h];
                }
            }

            return characters;

        }

    }
}
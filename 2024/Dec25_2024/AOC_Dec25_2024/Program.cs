using System;
using System.Collections;

namespace AdventOfCode
{
    public class Program
    {
        static List<char[,]> Keys = new List<char[,]>();
        static Dictionary<string,int[]> KeyPins = new Dictionary<string, int[]>();
        static List<char[,]> Locks = new List<char[,]>();
        static Dictionary<string,int[]> LockPins = new Dictionary<string, int[]>();
        static int VerticalMax = 0;
        static int HorizontalMax = 0;
        static void Main(string[] args)
        {

            SetFileData();
            
            //keys
            for(int keyIndex = 0; keyIndex < Keys.Count; keyIndex++){
                SetPinHeights($"k{keyIndex}", Keys[keyIndex], KeyPins);
            }

            //locks
            for(int lockIndex = 0; lockIndex < Locks.Count; lockIndex++){
                SetPinHeights($"l{lockIndex}", Locks[lockIndex], LockPins);
            }

            //Part 1
            long totalFitCombos = GetTotalFitCombos();
            Console.WriteLine($"Part 1: Total Fitting Combinations = {totalFitCombos}");

        }

        static long GetTotalFitCombos(){
            long totalCombos = 0;

            foreach(string k in KeyPins.Keys){
                foreach(string l in LockPins.Keys){
                    for(int p = 0; p<HorizontalMax; p++){
                        if(KeyPins[k][p] + LockPins[l][p] > VerticalMax - 2){
                            //lock doesn't fit
                            break;
                        }
                        else if(p+1 == HorizontalMax){
                            totalCombos += 1;
                        }
                    }
                }
            }

            return totalCombos;

        }

        static void SetPinHeights(string pDictKey, char[,] pItem, Dictionary<string,int[]> pDictToSet){
            //Get Height of Pins in each column (Lock or Key)
            int[] pinHeights = new int[HorizontalMax];

            for(int h = 0; h < HorizontalMax; h++){
                int pins = -1; //Start at -1 to skip TOP or BOTTOM line
                for(int v = 0; v < VerticalMax; v++){
                    if(pItem[v,h] == '#'){
                        pins += 1;
                    }
                }
                pinHeights[h] = pins;
            }

            pDictToSet[pDictKey] = pinHeights;
    
        }


        static void SetFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string fileName = "PuzzleInput1.txt";

            foreach(string s in File.ReadAllText(string.Concat(fileDataDir, "/", fileName)).Split("\n\n")){
                //LOCK or KEY
                string[] input = s.Split("\n");
                VerticalMax = input.Length;
                HorizontalMax = input[0].Length;
                char[,] gridInput = new char[VerticalMax,HorizontalMax];

                for(int v = 0; v < VerticalMax; v++){
                    for(int h = 0; h < HorizontalMax; h++){
                        gridInput[v,h] = input[v][h];
                    }
                }

                if(input[0] == "#####" && input[VerticalMax-1] == "....."){
                    Locks.Add(gridInput);
                }
                else if(input[0] == "....." && input[VerticalMax-1] == "#####"){
                    Keys.Add(gridInput);
                }
                else{
                    throw new Exception("Invalid Input Grid");
                }
            }

        }

    }
}
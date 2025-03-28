using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Program
    {
        static string[] Towels = new string[0];
        static List<string> Designs = new List<string>();
        // static Dictionary<string, string[]> MemoList = new Dictionary<string, string[]>();
        static void Main(string[] args)
        {

            SetFileData(); //Set objects from PuzzleInput1.txt

            //Loop through designs
            int possibleDesigns = 0;
            long totalDesignCombos = 0;

            foreach(string design in Designs){
                long[] arrDesTowelCount = new long[design.Length];

                for(int i = design.Length-1; i >= 0; i--){

                    long[] arrPartialDesign = new long[design.Length - i];
                    for(int j = 0; j < arrPartialDesign.Length; j++){
                        arrPartialDesign[j] = arrDesTowelCount[arrDesTowelCount.Length - arrPartialDesign.Length + j];
                    }

                    // //Part 1
                    // arrDesTowelCount[i] = GetMinimumTowels(design.Substring(i), arrPartialDesign);

                    // //Part 2
                    arrDesTowelCount[i] = GetTotalCombos(design.Substring(i), arrPartialDesign);
                }

                // //Part 1
                // if (arrDesTowelCount[0] != -1){
                //     possibleDesigns += 1;
                // }

                // //Part 2
                totalDesignCombos += arrDesTowelCount[0];

            }

            // //Part 1
            //Console.WriteLine($"Possible Designs: {possibleDesigns}");

            // //Part 2
            Console.WriteLine($"Total Combos: {totalDesignCombos}");

        }

        static int GetMinimumTowels(string pSubDesign, int[] pArrCounts){
            
            int towelCount = -1;

            foreach(string t in Towels){
                string regPattern_Sub = new Regex($"^{t}.*").ToString();
                string regPattern_Full = new Regex($"^{t}$").ToString();

                if(Regex.IsMatch(pSubDesign, regPattern_Full)){
                    //Perfect match to end of string
                    return 1;
                }
                else if(Regex.IsMatch(pSubDesign, regPattern_Sub)){
                    int regPatternLength = regPattern_Sub.Length - 3;
                    //Matches Part of string
                    if(pArrCounts[regPatternLength] != -1){
                        int tc = pArrCounts[regPatternLength] + 1;
                        if(towelCount == -1 || tc < towelCount){
                            towelCount = tc;
                        }
                    }
                }

            }

            return towelCount;
        }

        static long GetTotalCombos(string pSubDesign, long[] pArrCounts){
            
            long comboCount = 0;

            foreach(string t in Towels){
                string regPattern_Sub = new Regex($"^{t}.*").ToString();
                string regPattern_Full = new Regex($"^{t}$").ToString();

                if(Regex.IsMatch(pSubDesign, regPattern_Full)){
                    //Perfect match to end of string
                    comboCount += 1;
                }
                else if(Regex.IsMatch(pSubDesign, regPattern_Sub)){
                    int regPatternLength = regPattern_Sub.Length - 3;
                    //Matches Part of string

                    comboCount += pArrCounts[regPatternLength];
                }

            }

            return comboCount;
        }

        // static bool DesignPossible(string pInputDesign, string[] pTowelVals){

        //     foreach(string t in Towels){
        //         string regPattern_Sub = new Regex($"^{t}.*").ToString();
        //         string regPattern_Full = new Regex($"^{t}$").ToString();
        //         if(MemoStrings.Contains(pInputDesign)){
        //             //Previously Confirmed
        //             return true;
        //         }
        //         else if(Regex.IsMatch(pInputDesign, regPattern_Full)){
        //             //Full Match
        //             MemoStrings.Add(pInputDesign);
        //             return true;
        //         }
        //         else if(Regex.IsMatch(pInputDesign, regPattern_Sub)){
        //             //Substring Matches - continue searching
        //             if(DesignPossible(pInputDesign.Substring(t.Length), AddToStrArr(t, pTowelVals))){
        //                 MemoStrings.Add(pInputDesign);
        //                 return true;
        //             }
        //         }
        //     }
            
        //     return false;
        // }

        // static string[] AddToStrArr(string pNewStr, string[] pInStrArr){
        //     string[] newArr = new string[pInStrArr.Length + 1];
        //     pInStrArr.CopyTo(newArr, 0);
        //     newArr[pInStrArr.Length] = pNewStr;

        //     return newArr;
        // }


        static void SetFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string fileName = "PuzzleInput1.txt";

            string[] puzzleInputLines = File.ReadAllLines(string.Concat(fileDataDir, "/", fileName));

            //Get Towels
            string[] towelList = puzzleInputLines[0].Split(", ");
            Towels = new string[towelList.Length];
            towelList.CopyTo(Towels, 0);

            //Get Designs
            for(int s = 2; s < puzzleInputLines.Length; s++){
                Designs.Add(puzzleInputLines[s]);
            }
        }

    }
}
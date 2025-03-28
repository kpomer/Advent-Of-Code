using System;
using System.Collections;
using System.Configuration.Assemblies;
using System.Runtime.CompilerServices;

namespace AdventOfCode
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Parse Data into two objects
            //  filePageOrderRules[,] - stores rules
            //  filePageData[] - stores lines of data

            long[,] filePageOrderRules = new long[0,0];
            List <long[]> filePageData = new List<long[]>();
            SetParsedFileObjects(ref filePageOrderRules, ref filePageData);

            long sumMiddles = 0;
            bool correctOrder = false;
            List <long[]> incorrectOrderUpdates = new List<long[]>();
            foreach (long[] updateRow in filePageData)
            {

                long middleValue2 = 0;
                correctOrder = false;
                CheckUpdateOrder(updateRow, filePageOrderRules, ref correctOrder, ref middleValue2);
                if (correctOrder){
                    sumMiddles += middleValue2;
                }
                else{
                    incorrectOrderUpdates.Add(updateRow);
                }

            }

            Console.WriteLine($"Summed Middle Values = {sumMiddles}");

            long middleValue = 0;
            long sumUnsortedMiddles = 0;
            foreach(long[] unsortedUpdateRow in incorrectOrderUpdates){
                middleValue = 0;
                sortUpdateRecords(unsortedUpdateRow, ref middleValue, filePageOrderRules);
                sumUnsortedMiddles += middleValue;
            }
            
            Console.WriteLine($"Summed Unsorted Middle Values = {sumUnsortedMiddles}");

        }

        static void sortUpdateRecords(long[] pUnsortedUpdateRow, ref long pMiddleValue, long[,] pFilePageOrderRules){
            List <long> unsortedUpdateValues = pUnsortedUpdateRow.ToList<long>();
            long[] sortedUpdateRow = new long[pUnsortedUpdateRow.Length];
            for(int s = 0; s < sortedUpdateRow.Length; s++){
                List <long> unsortedUpdateValues2 = new List<long>(unsortedUpdateValues);
                for(int r = 0; r < pFilePageOrderRules.Length/2; r++){
                    if(unsortedUpdateValues2.Contains(pFilePageOrderRules[r,1])){
                        long afterValue = pFilePageOrderRules[r,1];
                        if(unsortedUpdateValues2.Contains(pFilePageOrderRules[r,0])){
                            unsortedUpdateValues2.Remove(afterValue);
                        }
                    }
                    if(unsortedUpdateValues2.Count<long>() == 1){
                        sortedUpdateRow[s] = unsortedUpdateValues2[0];
                        unsortedUpdateValues.Remove(unsortedUpdateValues2[0]);
                        break;
                    }
                }
            }

            pMiddleValue = sortedUpdateRow[sortedUpdateRow.Length/2];

        }


        static void CheckUpdateOrder(long[] pUpdateRow, long[,] pFilePageOrderRules, ref bool pCorrectOrder, ref long pMiddleValue){

            for(int i = 0; i < pUpdateRow.Length; i++){
                if(pageFollowsRules(pUpdateRow[i], i, pUpdateRow, pFilePageOrderRules) == false){
                    pCorrectOrder = false;
                    return;
                }
            }

            //All pages followed rules, return MiddleValue
            pCorrectOrder = true;
            pMiddleValue = pUpdateRow[pUpdateRow.Length/2];

        }

        static bool pageFollowsRules(long pPage, int pPageIndex, long[] pUpdateRow, long[,] pFilePageOrderRules){
            
            for(int i = 0; i < pFilePageOrderRules.Length/2; i++){
                if(pFilePageOrderRules[i, 0] == pPage){
                    //checkAfter - confirm does not exist BEFORE
                    long afterPageRule = pFilePageOrderRules[i, 1];
                    for(int a = 0; a < pPageIndex; a++){
                        if(pUpdateRow[a] == afterPageRule){
                            return false;
                        }
                    }
                }
                else if(pFilePageOrderRules[i, 1] == pPage){
                    //checkBefore - confirm does not exist AFTER
                    long beforePageRule = pFilePageOrderRules[i, 0];
                    for(int b = pPageIndex + 1; b < pPageIndex; b++){
                        if(pUpdateRow[b] == pUpdateRow.Length){
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        static void SetParsedFileObjects(ref long[,] pfilePageOrderRules, ref List <long[]> pfilePageData){
            
            string[] fileData1_String = GetFileData("PuzzleInput1.txt"); //String Array of all lines from PuzzleInput1.txt
            pfilePageOrderRules = new long[fileData1_String.Count<string>(),2];
            for(int i = 0; i < fileData1_String.Count<string>(); i++){ 

                string fileLine1 = fileData1_String[i];
                pfilePageOrderRules[i,0] = long.Parse(fileLine1.Split("|")[0]);
                pfilePageOrderRules[i,1] = long.Parse(fileLine1.Split("|")[1]);
            }

            string[] fileData2_String = GetFileData("PuzzleInput2.txt"); //String Array of all lines from PuzzleInput2.txt
            pfilePageData = new List<long[]>(fileData2_String.Count<string>());
            for(int i = 0; i < fileData2_String.Count<string>(); i++){ 

                string fileLine2 = fileData2_String[i];
                string[] fileLinesSplit = fileLine2.Split(",");
                long[] fileLineValues = new long[fileLinesSplit.Count<string>()];
                for(int j = 0; j < fileLineValues.Count<long>(); j++){ 
                    fileLineValues[j] = long.Parse(fileLinesSplit[j]);
                    }
                    pfilePageData.Add(fileLineValues);
            }
        }

        static string[] GetFileData(string fileName){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();

            return File.ReadAllLines(string.Concat(fileDataDir, "/", fileName));

        }

    }
}
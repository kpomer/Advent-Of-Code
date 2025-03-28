using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Data;
using System.Runtime.InteropServices;

namespace AdventOfCode
{

    enum levelStatus
    {
        Increasing,
        Decreasing,
        None
    }

    public class Program
    {
        static void Main(string[] args)
        {

            string[] fileData = GetFileData(); //String Array of all lines from PuzzleInput1.txt
     
            long safeLevels = 0;
            foreach (string levelLine in fileData){
                if(checkLevelSafety(levelLine)){
                    safeLevels++;
                }
                
                else{
                    //Allow 1 buffer - check strings without 1 entry
                    List<string> listLevelEntriesTotal = levelLine.Split(" ").ToList<String>();
                    for(int l = 0; l < listLevelEntriesTotal.Count<string>(); l++){
                        if(checkLevelSafetyBuf1(new List<string>(listLevelEntriesTotal), l)){
                            safeLevels++;
                            break;
                        }
                    }

                }
                

            }

            Console.WriteLine($"Safe Entries: {safeLevels}");
        }

            

        static string createLevelFromArray(List<string> lstArrayLevel){

            string level = "";
            for(int i = 0; i < lstArrayLevel.Count<string>(); i++){
                if(i != 0){
                    level += " ";
                }
                level += lstArrayLevel[i];
            }

            return level;

        }

        static bool checkLevelSafetyBuf1(List<string> levelList, int indexRemove){
            levelList.RemoveAt(indexRemove);

            return checkLevelSafety(createLevelFromArray(levelList));

        }

        static bool checkLevelSafety(string level){

            long entryChange = 0;
            levelStatus status = levelStatus.None;

            string[] levelEntries = level.Split(" ");
                status = levelStatus.None;
                for(int i = 0; i < levelEntries.Count<string>(); i++){
                    if(i == levelEntries.Count<string>() - 1){
                        return true;
                    }
                    
                    entryChange = long.Parse(levelEntries[i+1]) - long.Parse(levelEntries[i]);
                    if(status == levelStatus.None){
                        if(entryChange > 0){
                            status = levelStatus.Increasing;
                        }
                        else if(entryChange < 0){
                            status = levelStatus.Decreasing;
                        }
                    }

                    if(status == levelStatus.Increasing && entryChange >= 1 && entryChange <= 3){
                        //nothing
                    }
                    else if(status == levelStatus.Decreasing && entryChange >= -3 && entryChange <= -1){
                        //nothing
                    }
                    else{
                        return false;
                        
                    }

                }

                return true;
        }


        static string[] GetFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string fileName = "PuzzleInput1.txt";

            return File.ReadAllLines(string.Concat(fileDataDir, "/", fileName));

        }

    }
}
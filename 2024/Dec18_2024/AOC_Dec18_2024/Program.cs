using System;
using System.Collections;

namespace AdventOfCode
{
    public class Program
    {
        public static int MemorySize = 71;
        public static char[,] Memory = new char[MemorySize,MemorySize];
        public static List<int[]> ByteData = new List<int[]>();
        static List<PathCoordinate> UnexploredCoordinates = new List<PathCoordinate>();
        static List<PathCoordinate> ExploredCoordinates = new List<PathCoordinate>();

        public class PathCoordinate
        {
            public int[] Coordinates { get; set; }
            public int[] LastCoordinates { get; set; }
            public char CharVal { get; set; }
            public long RequiredPoints { get; set; }
            public PathCoordinate(int[] pCoord, int[] pLastCoord, char pCharVal, long pReqPoints)
            {
                Coordinates = pCoord;
                LastCoordinates = pLastCoord;
                CharVal = pCharVal;
                RequiredPoints = pReqPoints;
            }
        }

        // // PART 1
        // static void Main(string[] args)
        // {

        //     SetFileData(); //String Array of all lines from PuzzleInput1.txt

        //     PrintMemory();
        //     for(int n = 0; n < 1024; n++){
        //         Memory[ByteData[n][1],ByteData[n][0]] = '#';
        //     }

        //     PopulateCoordinateLists();
        //     PrintMemory();

        //     while(UnexploredCoordinates.Count > 0 || ExploredCoordinates.FindIndex(a => a.Coordinates[0] == MemorySize-1 && a.Coordinates[1] == MemorySize-1) == -1){

        //         UnexploredCoordinates.Sort((x, y) => x.RequiredPoints.CompareTo(y.RequiredPoints));
        //         CheckAdjacentValues(UnexploredCoordinates[0]);

        //         ExploredCoordinates.Add(UnexploredCoordinates[0]);
        //         UnexploredCoordinates.Remove(UnexploredCoordinates[0]);

        //         if(ExploredCoordinates.FindIndex(a => a.Coordinates[0] == MemorySize-1 && a.Coordinates[1] == MemorySize-1) != -1){
        //             break;
        //         }
        //     }

        //     int exitIndex = ExploredCoordinates.FindIndex(a => a.Coordinates[0] == MemorySize-1 && a.Coordinates[1] == MemorySize-1);
        //     if(exitIndex != -1){
        //         Console.WriteLine($"Minimum Steps: {ExploredCoordinates[exitIndex].RequiredPoints}");
        //     }
        //     else{
        //         Console.WriteLine($"No Exit");
        //     }
        // }


        // PART 2
        static void Main(string[] args)
        {

            SetFileData(); //String Array of all lines from PuzzleInput1.txt

            //PrintMemory();
            for(int n = 0; n < ByteData.Count; n++){
                Memory[ByteData[n][1],ByteData[n][0]] = '#';
                if(n>2750){
                    //Only checking after 2750 to speed up the process
                    if(GetExitCount() < 0){
                        Console.WriteLine($"Blocking Byte: {ByteData[n+1][0]},{ByteData[n+1][1]}");
                        break;
                    }
                }
                
            }

            //PrintMemory();

        }

        static long GetExitCount(){
            //Perform Dijkstra's Algorithm
            PopulateCoordinateLists();
            while(UnexploredCoordinates.Count > 0 && ExploredCoordinates.FindIndex(a => a.Coordinates[0] == MemorySize-1 && a.Coordinates[1] == MemorySize-1) == -1){

                UnexploredCoordinates.Sort((x, y) => x.RequiredPoints.CompareTo(y.RequiredPoints));
                CheckAdjacentValues(UnexploredCoordinates[0]);

                ExploredCoordinates.Add(UnexploredCoordinates[0]);
                UnexploredCoordinates.Remove(UnexploredCoordinates[0]);

                if(ExploredCoordinates.FindIndex(a => a.Coordinates[0] == MemorySize-1 && a.Coordinates[1] == MemorySize-1) != -1){
                    break;
                }
            }

            int exitIndex = ExploredCoordinates.FindIndex(a => a.Coordinates[0] == MemorySize-1 && a.Coordinates[1] == MemorySize-1);
            if(exitIndex != -1){
                return ExploredCoordinates[exitIndex].RequiredPoints;
            }

            return -1; //no exit
        }

        static void CheckAdjacentValues(PathCoordinate pCurrentCoord){
            
            //Check UP: (-1,0)
            SetPoints(pCurrentCoord, [-1,0], pCurrentCoord.RequiredPoints + 1);

            //Check RIGHT: (0,1)
            SetPoints(pCurrentCoord, [0,1], pCurrentCoord.RequiredPoints + 1);

            //Check DOWN: (1,0)
            SetPoints(pCurrentCoord, [1,0], pCurrentCoord.RequiredPoints + 1);

            //Check LEFT : (0,-1)
            SetPoints(pCurrentCoord, [0,-1], pCurrentCoord.RequiredPoints + 1);
            
        }

        static void SetPoints(PathCoordinate pCurrentCoord, int[] pDirection, long pPointsNeeded){
            int[] nextCoordinate = [pCurrentCoord.Coordinates[0] + pDirection[0],pCurrentCoord.Coordinates[1] + pDirection[1]];

            if(nextCoordinate[0] < 0 || nextCoordinate[0] >= MemorySize || nextCoordinate[1] < 0 || nextCoordinate[1] >= MemorySize || Memory[nextCoordinate[0],nextCoordinate[1]] == '#'){
                return; //invalid space
            }


            int index = UnexploredCoordinates.FindIndex(a => (a.Coordinates[0] == nextCoordinate[0]) && (a.Coordinates[1] == nextCoordinate[1]));
            if(index == -1){
                if(ExploredCoordinates.FindIndex(a => (a.Coordinates[0] == nextCoordinate[0]) && (a.Coordinates[1] == nextCoordinate[1])) != -1){
                    return;
                }
                else{
                    throw new Exception($"Index -1 for coordinate: [{nextCoordinate[0]},{nextCoordinate[1]}]");
                }

            }
            else if(UnexploredCoordinates[index].RequiredPoints > pPointsNeeded){
                UnexploredCoordinates[index].RequiredPoints = pPointsNeeded;
                UnexploredCoordinates[index].LastCoordinates = pCurrentCoord.Coordinates;
            }
            
        }


        static void PrintMemory(){
            string memoryLine = "";
            for(int v = 0; v < MemorySize; v++){
                for(int h = 0; h < MemorySize; h++){
                    memoryLine = string.Concat(memoryLine, Memory[v,h]);
                }
                memoryLine = string.Concat(memoryLine, "\n");
            }
            Console.WriteLine(memoryLine);
        }

        static void PopulateCoordinateLists(){
            //reset values
            UnexploredCoordinates = new List<PathCoordinate>();
            ExploredCoordinates = new List<PathCoordinate>();

            for(int v = 0; v < MemorySize; v++){
                for(int h = 0; h < MemorySize; h++){
                    if(v==0 && h==0){
                        UnexploredCoordinates.Add(new PathCoordinate([v,h], [-1,-1], Memory[v,h], 0));
                    }
                    else if(Memory[v,h] != '#'){
                        UnexploredCoordinates.Add(new PathCoordinate([v,h], [-1,-1], Memory[v,h], long.MaxValue));
                    }
                }
            }

        }


        static void SetFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string fileName = "PuzzleInput1.txt";

            string[] fileStringArr = File.ReadAllLines(string.Concat(fileDataDir, "/", fileName)); 

            foreach(string s in fileStringArr){
                ByteData.Add([int.Parse(s.Split(",")[0]),int.Parse(s.Split(",")[1])]);
            }

            for(int v = 0; v < MemorySize; v++){
                for(int h = 0; h < MemorySize; h++){
                    Memory[v,h] = '.';                    
                }
            }

        }

    }
}
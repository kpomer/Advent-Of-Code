using System;
using System.Collections;

namespace AdventOfCode
{
    public class Program
    {
        static char[,] MapCoordinates = new char[0,0];
        static int VerticalMax = 0;
        static int HorizontalMax = 0;
        static long NoCheatTime = 0;
        static long AllowedCheat = 20;
        static long Saved100Plus_Count = 0;
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
        static void Main(string[] args)
        {

            int[] currentPosition = SetFileData(); //String Array of all lines from PuzzleInput1.txt

            //Use Dijkstra's Algorithm to Explore all Coordinates
            while(UnexploredCoordinates.Count > 0){
                UnexploredCoordinates.Sort((x, y) => x.RequiredPoints.CompareTo(y.RequiredPoints));
                CheckAdjacentValues(UnexploredCoordinates[0]);

                ExploredCoordinates.Add(UnexploredCoordinates[0]);
                UnexploredCoordinates.Remove(UnexploredCoordinates[0]);
            }

            int endIndex = ExploredCoordinates.FindIndex(a => a.CharVal == 'S');
            NoCheatTime = ExploredCoordinates[endIndex].RequiredPoints;
            Console.WriteLine($"Min Required Points (no cheats): {NoCheatTime}");

            ExploredCoordinates.Sort((x, y) => -1*x.RequiredPoints.CompareTo(y.RequiredPoints));
            long counter = 0;
            foreach(PathCoordinate ec in ExploredCoordinates){
                HashSet<string> PerformedCheats = new HashSet<string>();

                for(long dx = -1*AllowedCheat; dx <= AllowedCheat; dx++){
                    for(long dy = -1*AllowedCheat; dy <= AllowedCheat; dy++){
                        long cheatAmount = Math.Abs(dx) + Math.Abs(dy);
                        if(cheatAmount <= AllowedCheat){
                            int newX = ec.Coordinates[0] + Convert.ToInt32(dx);
                            int newY = ec.Coordinates[1] + Convert.ToInt32(dy);

                            string cheatString = $"[{ec.Coordinates[0]},{ec.Coordinates[1]}] - [{newX},{newY}]";
                            if(PerformedCheats.Contains(cheatString) == false){
                                int nextIndex = ExploredCoordinates.FindIndex(a => (a.Coordinates[0] == newX) && (a.Coordinates[1] == newY));
                                if(nextIndex != -1){
                                    PathCoordinate nextCoordiate = ExploredCoordinates[nextIndex];
                                    long newTime = NoCheatTime - ec.RequiredPoints + cheatAmount + nextCoordiate.RequiredPoints;
                                    if(NoCheatTime - newTime >= 100){
                                        Saved100Plus_Count += 1;
                                    }
                                }
                                PerformedCheats.Add(cheatString);
                            }
                        }
                    }
                }


                if(counter % 100 == 0){
                    Console.WriteLine($"Counter: {counter}");
                }
                counter+= 1;
                //MoveCheat(ec, ec.Coordinates, 0, PerformedCheats);
            }

            Console.WriteLine($"Saved 100+: {Saved100Plus_Count}");

        }

        static void MoveCheat(PathCoordinate pStartCoord, int[] currentCoordinate, int pMoveCount, HashSet<string> pPerformedCheats){

            //check Valid Coordinate
            if(currentCoordinate[0] < 0 || currentCoordinate[0] >= VerticalMax || currentCoordinate[1] < 0 || currentCoordinate[1] >= HorizontalMax){
                return;
            }

            if(pMoveCount > 0){
                // Check Saved Time
                int curIndex = ExploredCoordinates.FindIndex(a => (a.Coordinates[0] == currentCoordinate[0]) && (a.Coordinates[1] == currentCoordinate[1]));
                if(curIndex != -1){
                    PathCoordinate curPoint = ExploredCoordinates[curIndex];

                    if(pPerformedCheats.Contains($"[{pStartCoord.Coordinates[0]},{pStartCoord.Coordinates[1]}] - [{curPoint.Coordinates[0]},{curPoint.Coordinates[1]}]") == false){
                        long newTime = NoCheatTime - pStartCoord.RequiredPoints + pMoveCount + curPoint.RequiredPoints;
                        if(NoCheatTime - newTime >= 100){
                            Saved100Plus_Count += 1;
                        }
                        pPerformedCheats.Add($"[{pStartCoord.Coordinates[0]},{pStartCoord.Coordinates[1]}] - [{curPoint.Coordinates[0]},{curPoint.Coordinates[1]}]");
                    }

                }

            }

            int[] nextCoordinate;
            if(pMoveCount < AllowedCheat){
                //Check Adjacent Points
                
                //UP (-1,0)
                nextCoordinate = [currentCoordinate[0] - 1, currentCoordinate[1]];
                MoveCheat(pStartCoord, nextCoordinate, pMoveCount + 1, pPerformedCheats);

                //RIGHT (0,1)
                nextCoordinate = [currentCoordinate[0], currentCoordinate[1] + 1];
                MoveCheat(pStartCoord, nextCoordinate, pMoveCount + 1, pPerformedCheats);

                //DOWN (1,0)
                nextCoordinate = [currentCoordinate[0] + 1, currentCoordinate[1]];
                MoveCheat(pStartCoord, nextCoordinate, pMoveCount + 1, pPerformedCheats);

                //LEFT (0,-1)
                nextCoordinate = [currentCoordinate[0], currentCoordinate[1] - 1];
                MoveCheat(pStartCoord, nextCoordinate, pMoveCount + 1, pPerformedCheats);
            }
        }

        static long CheckSavedPicoseconds(PathCoordinate pCurCoord, int[] pDirection, int pCheatTime){

            long totalTime = NoCheatTime - pCurCoord.RequiredPoints;
            int[] newCoordinates = [pCurCoord.Coordinates[0] + pCheatTime*pDirection[0],pCurCoord.Coordinates[1] + pCheatTime*pDirection[1]];
            
            int newIndex = ExploredCoordinates.FindIndex(a => (a.Coordinates[0] == newCoordinates[0]) && (a.Coordinates[1] == newCoordinates[1]));

            if(newIndex == -1){
                return long.MinValue;
            }
            else{
                PathCoordinate newCoord = ExploredCoordinates[newIndex];
                totalTime = totalTime + pCheatTime + newCoord.RequiredPoints;
            }

            return NoCheatTime - totalTime;

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

            if(nextCoordinate[0] < 0 || nextCoordinate[0] >= VerticalMax || nextCoordinate[1] < 0 || nextCoordinate[1] >= HorizontalMax || MapCoordinates[nextCoordinate[0],nextCoordinate[1]] == '#'){
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

        static int[] SetFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string mapFileName = "PuzzleInput1.txt";
            int[] startPosition = [0,0];

            //map
            string[] mapLines = File.ReadAllLines(string.Concat(fileDataDir, "/", mapFileName));
            VerticalMax = mapLines.Length;
            HorizontalMax = mapLines[0].Length;

            //map Parsing
            MapCoordinates = new char[mapLines.Length,mapLines[0].Length];
            for(int v = 0; v < mapLines.Length; v++){
                for(int h = 0; h < mapLines[0].Length; h++){
                    char c = mapLines[v][h];
                    MapCoordinates[v,h] =c;
                    if(c == 'E'){
                        startPosition = [v,h];
                        UnexploredCoordinates.Add(new PathCoordinate([v,h], [v,h-1], c, 0)); //indicates start direction is EAST
                    }
                    else if(c != '#'){
                        UnexploredCoordinates.Add(new PathCoordinate([v,h], [-1,-1], c, long.MaxValue));
                    }
                }
            }      

            return startPosition;

        }

    }
}
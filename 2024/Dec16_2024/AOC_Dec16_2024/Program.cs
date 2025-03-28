using System;
using System.Collections;
using System.Configuration.Assemblies;
using System.Drawing;
using System.Formats.Asn1;
using System.Net;
using System.Runtime.InteropServices.Marshalling;

namespace AdventOfCode
{
    public class Program
    {
        //static int[] CurrentPoint = new int[2];
        //static int[] Direction = new int[2];
        //static long Points = 0;
        //static List<long> pathPoints = new List<long>();
        static long bestPathPoints = 0;
        static List<int[]> AllBestSpots = new List<int[]>();
        //static Dictionary<int[],char> GridMap_Entries = new Dictionary<int[],char>(); //{coordinate, value}
        static char[,] MapCoordinates = new char[0,0];
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

            int[] currentPoint = SetFileData();
            int[] direction = [0,1]; //start facing EAST

            // while(ExploredCoordinates.FindIndex(a => a.CharVal == 'E') == -1){
            while(UnexploredCoordinates.Count > 0){
                UnexploredCoordinates.Sort((x, y) => x.RequiredPoints.CompareTo(y.RequiredPoints));
                CheckAdjacentValues(UnexploredCoordinates[0]);

                ExploredCoordinates.Add(UnexploredCoordinates[0]);
                UnexploredCoordinates.Remove(UnexploredCoordinates[0]);
            }

            int indexEnd = ExploredCoordinates.FindIndex(a => a.CharVal == 'E');
            bestPathPoints = ExploredCoordinates[indexEnd].RequiredPoints;
            
            Console.WriteLine($"Minimum Points: {bestPathPoints}");

            //Part 2 - backtrack
            currentPoint = ExploredCoordinates[indexEnd].Coordinates;

            MoveBack(currentPoint, [-1,0], bestPathPoints, [currentPoint]);
            MoveBack(currentPoint, [0,1], bestPathPoints, [currentPoint]);
            MoveBack(currentPoint, [1,0], bestPathPoints, [currentPoint]);
            MoveBack(currentPoint, [0,-1], bestPathPoints, [currentPoint]);

            Console.WriteLine($"Total Path Spots: {AllBestSpots.Count}");

        }

        static void CheckAdjacentValues(PathCoordinate pCurrentCoord){

            int[] currentDirection = [pCurrentCoord.Coordinates[0] - pCurrentCoord.LastCoordinates[0],pCurrentCoord.Coordinates[1] - pCurrentCoord.LastCoordinates[1]];            
            
            //Check Straight
            SetPoints(pCurrentCoord, currentDirection, pCurrentCoord.RequiredPoints + 1);

            //Check Left
            SetPoints(pCurrentCoord, Turn90("LEFT", currentDirection), pCurrentCoord.RequiredPoints + 1001);

            //Check Right
            SetPoints(pCurrentCoord, Turn90("RIGHT", currentDirection), pCurrentCoord.RequiredPoints + 1001);    

        }

        static void SetPoints(PathCoordinate pCurrentCoord, int[] pDirection, long pPointsNeeded){
            int[] nextCoordinate = [pCurrentCoord.Coordinates[0] + pDirection[0],pCurrentCoord.Coordinates[1] + pDirection[1]];
            if(MapCoordinates[nextCoordinate[0],nextCoordinate[1]] != '#'){
                //TODO Handle 'E'
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
        }

        static void MoveBack(int[] pCurPoint, int[] direction, long pPoints, List<int[]> pathCoords){
            pCurPoint = [pCurPoint[0] + direction[0], pCurPoint[1] + direction[1]];
            pPoints -= 1;

            int coordinateIndex = ExploredCoordinates.FindIndex(a => (a.Coordinates[0] == pCurPoint[0]) && (a.Coordinates[1] == pCurPoint[1]));
            
            PathCoordinate current;
            if(coordinateIndex == -1){
                return; //invalid path
            }
            else{
                current = ExploredCoordinates[coordinateIndex];
                if(current.CharVal == 'S' && (direction[0] != 0 || direction[0] != -1)){
                    pPoints -= 1000; //turn needed to face correct direction
                }
            }

            if(current.RequiredPoints <= pPoints){
                pathCoords.Add(current.Coordinates);
            }
            else{
                return;
            }


            if(current.CharVal == 'S'){
                //Add to list
                foreach(int[] pathSpot in pathCoords){
                    if(AllBestSpots.IndexOf(pathSpot) == -1){
                        AllBestSpots.Add(pathSpot);
                    }
                }

                //Add to list
            }
            else if(current.CharVal == '.'){

                //try STRAIGHT
                MoveBack(pCurPoint, direction, pPoints, pathCoords);

                //try LEFT
                MoveBack(pCurPoint, Turn90("LEFT", direction), pPoints - 1000, pathCoords);

                //try RIGHT
                MoveBack(pCurPoint, Turn90("RIGHT", direction), pPoints - 1000, pathCoords);
            }
            else{
                return;
            }
        }


        static int[] Turn90(string pDirection, int[] pInitDir){

            List<int[]> dirList = new List<int[]>();

            // //Add directions clockwise
            dirList.Add([-1,0]); //UP
            dirList.Add([0,1]); //RIGHT
            dirList.Add([1,0]); //DOWN
            dirList.Add([0,-1]); //LEFT

            int dirIndex = dirList.FindIndex(array => array.SequenceEqual(pInitDir));

            int newDirectionIndex = 0;
            switch(pDirection) 
                {
                case "LEFT":
                    newDirectionIndex = dirIndex - 1;
                    break;
                case "RIGHT":
                    newDirectionIndex = dirIndex + 1;
                    break;
                default:
                    throw new Exception($"Invalid Direction {pDirection}");
                }

                if(newDirectionIndex < 0){
                    newDirectionIndex += dirList.Count;
                }

                return dirList[newDirectionIndex % dirList.Count];
        }


        static int[] SetFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string mapFileName = "PuzzleInput1.txt";
            int[] startPosition = [0,0];

            //map
            string[] mapLines = File.ReadAllLines(string.Concat(fileDataDir, "/", mapFileName));

            //map Parsing
            MapCoordinates = new char[mapLines.Length,mapLines[0].Length];
            for(int v = 0; v < mapLines.Length; v++){
                for(int h = 0; h < mapLines[0].Length; h++){
                    char c = mapLines[v][h];
                    MapCoordinates[v,h] =c;
                    if(c == 'S'){
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
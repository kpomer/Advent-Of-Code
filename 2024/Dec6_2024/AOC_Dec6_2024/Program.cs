using System;
using System.Collections;

namespace AdventOfCode
{
    public class Program
    {

        public static char[,] mapCoordinates = new char[0,0];
        public static long maxHorizontal = 0;
        public static long maxVertical = 0;

        public static HashSet<string> corners = new HashSet<string>(); //corner turns - when repeatd we are in a loop
        public static long[] NextStep = [-1, 0]; //startingDirection: UP


        static void Main(string[] args)
        {

            long[] currentPosition = GetMapCoordinates(); //Long[,] of all coordinates and values from PuzzleInput1.txt
            List<long[]> loopBarrierPosition = new List<long[]>();

            for(int v = 0; v < maxVertical; v++){
                for(int h = 0; h < maxHorizontal; h++){
                    currentPosition = GetMapCoordinates(); //Reset values (starting position and coordinate layout)
                    corners = new HashSet<string>(); //Reset corners HashSet
                    NextStep = [-1, 0]; //Reset NextStep long[]
                    bool skipCoordinate = false;
                    if(mapCoordinates[v,h] == '.'){
                        //set barrier at [v,h]
                        mapCoordinates[v,h] = '#';
                    }
                    else{
                        //Already barrier or starting position
                        skipCoordinate = true;
                    }
                    
                    if(!skipCoordinate){
                        
                        //HashSet<string> distinctPositions = new HashSet<string>();
                        //long countDistinctPositions = 0;
                        long[] nextPosition = [0,0];

                        //AddDistinctPosition(ref distinctPositions, ref countDistinctPositions, currentPosition);
                        //Console.WriteLine($"Start Position [{currentPosition[0]},{currentPosition[1]}] | Total Distinct Positions: {countDistinctPositions}");
                        while (true){
                            nextPosition = GetNextPosition(currentPosition);
                            
                            if(nextPosition[0] < 0 || nextPosition[0] >= maxVertical || nextPosition[1] < 0 || nextPosition[1] >= maxHorizontal){
                                //Edge of map reached
                                //Console.WriteLine("Reached End of Map! - no loops");
                                break;
                            }
                            else if(mapCoordinates[nextPosition[0],nextPosition[1]] == '#'){
                                //barrier reached, turn 90 degrees
                                if(turn90Degrees(currentPosition)){
                                    //loop hit
                                    loopBarrierPosition.Add([v,h]);
                                    //Console.WriteLine($"Loop Found - barrier position [{v},{h}]");        
                                    break;
                                }
                                
                            }
                            else{
                                //move next!
                                currentPosition[0] = nextPosition[0];
                                currentPosition[1] = nextPosition[1];
                                //AddDistinctPosition(ref distinctPositions, ref countDistinctPositions, currentPosition);
                                //Console.WriteLine($"New Position [{currentPosition[0]},{currentPosition[1]}] | Total Distinct Positions: {countDistinctPositions}");
                                //Console.WriteLine($"New Position [{currentPosition[0]},{currentPosition[1]}] | Total Corners Hit: {corners.Count}");
                            }

                        }

                        //Console.WriteLine($"Finished!  Current Postion [{currentPosition[0]},{currentPosition[1]}] | Total Distinct Positions: {countDistinctPositions}");                             
                    }     
                }
            }

            Console.WriteLine($"Total Loop Barrier Positions: {loopBarrierPosition.Count()}");
        }

        static void AddDistinctPosition(ref HashSet<string> pDistinctPositions, ref long pCountDistinctPos, long[] pCurrentPosition){

            string curPositionString = $"[{pCurrentPosition[0]},{pCurrentPosition[1]}]";
            if(pDistinctPositions.Contains(curPositionString)){
                //do Nothing
                //Console.WriteLine("Already been here");
            }
            else{
                pDistinctPositions.Add(curPositionString);
                pCountDistinctPos += 1;
            }
        }

        static long[] GetNextPosition(long[] pCurrentPosition){
            long[] nextPosition = new long[2];
            nextPosition[0] = pCurrentPosition[0] + NextStep[0];
            nextPosition[1] = pCurrentPosition[1] + NextStep[1];

            return nextPosition;
        }

        static bool turn90Degrees(long[] pCurrentPosition){
            string nextDirection = "";
            switch(NextStep) 
                {
                case [-1, 0]: //UP
                    NextStep = [0,1]; //Turn RIGHT
                    nextDirection = "RIGHT";
                    break;
               case [0,1]: //RIGHT
                    NextStep = [1,0]; //Turn DOWN
                    nextDirection = "DOWN";
                    break;
               case [1,0]: //DOWN
                    NextStep = [0,-1]; //Turn LEFT
                    nextDirection = "LEFT";
                    break;
               case [0,-1]: //LEFT
                    NextStep = [-1, 0]; //Turn UP
                    nextDirection = "UP";
                    break;
                default:
                    throw new Exception($"Invalid direction [{NextStep[0]},{NextStep[1]}]");
                }

                string cornerString = $"[{pCurrentPosition[0]},{pCurrentPosition[1]}]_{nextDirection}";
                //Console.WriteLine($"Reached Barrier - corner: {cornerString}");
                if(corners.Contains(cornerString)){
                    //LOOP REACHED!
                    return true;
                }
                else{
                    corners.Add(cornerString);
                }
                return false;

                //Console.WriteLine($"Reached Barrier - turn {nextDirection}");
        }





        static long[] GetMapCoordinates(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string fileName = "PuzzleInput1.txt";

            string[] fileStringArr = File.ReadAllLines(string.Concat(fileDataDir, "/", fileName)); 

            maxHorizontal = fileStringArr[0].Length;
            maxVertical = fileStringArr.Length;
            mapCoordinates = new char[maxVertical,maxHorizontal];

            long[] startPosition = new long[2];
            for(int l = 0; l < fileStringArr.Length; l++){
                char[] lineCharArr = fileStringArr[l].ToCharArray();

                for(int c = 0; c < lineCharArr.Length; c++){
                    mapCoordinates[l, c] = lineCharArr[c];
                    if(lineCharArr[c] == '^'){
                        //set StartingPosition
                        startPosition[0] = l;
                        startPosition[1] = c;
                    }
                }

            }

            return startPosition;

        }

    }
}
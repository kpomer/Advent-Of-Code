using System;
using System.Collections;
using System.Collections.Immutable;
using System.Runtime.ExceptionServices;
using System.Runtime.Intrinsics.X86;

namespace AdventOfCode
{
    public class Program
    {
        
        static Dictionary<string,char> GridMap = new Dictionary<string, char>(); //{coordinate, value}
        static List<int[]> Movements = new List<int[]>();
        static int VerticalMax = 0;
        static int HorizontalMax = 0;

        static void Main(string[] args)
        {

            int[] currentPosition = SetFileData(); //Set objects from PuzzleInput1.txt & PuzzleInput2.txt
            
            //PrintCurrentState(); //Print Starting Positions

            //Make Moves
            int counter = 1;
            foreach(int[] movement in Movements){
                // //Part 1
                // if(MakeMove(currentPosition, movement, '@')){
                //     currentPosition = [currentPosition[0] + movement[0],currentPosition[1] + movement[1]];
                // }

                //Part 2
                if(MakeMove2([currentPosition], movement)){
                    currentPosition = [currentPosition[0] + movement[0],currentPosition[1] + movement[1]];
                }
                //Console.WriteLine($"Counter: {counter}");
                //PrintCurrentState();
                counter += 1;
            }

            //Calculate GPS Sum
            long gpsSum = 0;
            foreach(string coordinate in GridMap.Keys){
                //PART 1
                // if(GridMap[coordinate] == 'O'){
                //     //Point contains a box
                //     int[] arrCoordinate = StrToArr(coordinate);
                //     gpsSum += 100*arrCoordinate[0] + arrCoordinate[1];
                // }

                //PART 2
                if(GridMap[coordinate] == '['){
                    //Point contains a box
                    int[] arrCoordinate = StrToArr(coordinate);
                    gpsSum += 100*arrCoordinate[0] + arrCoordinate[1];
                }
            }

            Console.WriteLine($"GPS Sum: {gpsSum}");

        }

        static bool MakeMove(int[] pCurrentPosition, int[] pMovement, char value){
            int[] nextSpace = [pCurrentPosition[0] + pMovement[0],pCurrentPosition[1] + pMovement[1]];
            if(nextSpace[0] < 0 || nextSpace[0] >= VerticalMax || nextSpace[1] < 0 || nextSpace[1] >= HorizontalMax || GridMap[ArrToStr(nextSpace)] == '#'){
                //Barrier - no move
                return false;
            }
            else if(GridMap[ArrToStr(nextSpace)] == '.'){
                //Free space - move
                GridMap[ArrToStr(nextSpace)] = value;
                GridMap[ArrToStr(pCurrentPosition)] = '.';
                return true;
            }
            else if(GridMap[ArrToStr(nextSpace)] == 'O'){
                //Reached box - attempt to push
                if(MakeMove(nextSpace, pMovement, GridMap[ArrToStr(nextSpace)])){
                    //Boxes could be pushed
                    GridMap[ArrToStr(nextSpace)] = value;
                    GridMap[ArrToStr(pCurrentPosition)] = '.';
                    return true;
                }
                else{
                    //Barrier reached
                    return false;
                }
            }
            else{
                throw new Exception($"Invalid Value: {GridMap[ArrToStr(nextSpace)]}");
            }
            
        }

        static bool MakeMove2(int[][] pCurrentPositions, int[] pMovement){
            int[][] nextSpaces = new int[0][]; //TODO Add Length?
            if(ArrToStr(pMovement) == "[0,-1]" || ArrToStr(pMovement) == "[0,1]"){
                nextSpaces = new int[pCurrentPositions.Length][];
                for(int cs = 0; cs < pCurrentPositions.Length; cs++){
                    nextSpaces[cs] = [pCurrentPositions[cs][0] + pMovement[0],pCurrentPositions[cs][1] + pMovement[1]];
                }
            }
            else if(ArrToStr(pMovement) == "[-1,0]" || ArrToStr(pMovement) == "[1,0]"){
                List<string> nextSpacesExtended = new List<string>();
                    for(int cs = 0; cs < pCurrentPositions.Length; cs++){
                        if(GridMap[ArrToStr(pCurrentPositions[cs])] == '.'){
                            continue;
                        }
                        int[] ns1, ns2;
                        string ns1_str, ns2_str;
                        ns1 = [pCurrentPositions[cs][0] + pMovement[0],pCurrentPositions[cs][1] + pMovement[1]];
                        ns1_str = ArrToStr(ns1);

                        if(GridMap[ns1_str] == ']'){
                            //check for '[' and add
                            ns2 = [pCurrentPositions[cs][0] + pMovement[0],pCurrentPositions[cs][1] + pMovement[1]-1];
                            ns2_str = ArrToStr(ns2);
                            if(nextSpacesExtended.Contains(ns2_str) == false){
                                nextSpacesExtended.Add(ns2_str);
                            }
                            //check for ']' and add
                            if(nextSpacesExtended.Contains(ns1_str) == false){
                                nextSpacesExtended.Add(ns1_str);
                            }
                        }


                        else if(GridMap[ns1_str] == '['){
                            //check for '[' and add
                            if(nextSpacesExtended.Contains(ns1_str) == false){
                                nextSpacesExtended.Add(ns1_str);
                            }
                            //check for ']' and add
                            ns2 = [pCurrentPositions[cs][0] + pMovement[0],pCurrentPositions[cs][1] + pMovement[1]+1];
                            ns2_str = ArrToStr(ns2);
                            if(nextSpacesExtended.Contains(ns2_str) == false){
                                nextSpacesExtended.Add(ns2_str);
                            }
                        }

                        else if(nextSpacesExtended.Contains(ns1_str) == false){
                                nextSpacesExtended.Add(ns1_str);
                        }
                }

                nextSpaces = new int[nextSpacesExtended.Count][];
                for(int ns = 0; ns < nextSpacesExtended.Count; ns++){
                    nextSpaces[ns] = StrToArr(nextSpacesExtended[ns]);
                }
            }


            //if any barriers exist, NO MOVE
            bool boxExists = false;
            for(int n = 0; n < nextSpaces.Length; n++){
                if(GridMap[ArrToStr(nextSpaces[n])] == '#'){
                    return false;
                }
                else if(GridMap[ArrToStr(nextSpaces[n])] == '[' || GridMap[ArrToStr(nextSpaces[n])] == ']'){
                    boxExists = true;
                }
                else if(GridMap[ArrToStr(nextSpaces[n])] != '.'){
                    throw new Exception($"Invalid Value: {GridMap[ArrToStr(nextSpaces[n])]}");
                }
            }


            // foreach(int[] nextSpace in nextSpaces){
                
            //     if(GridMap[ArrToStr(nextSpace)] == '#'){
            //         return false;
            //     }
            //     else if(GridMap[ArrToStr(nextSpace)] == '[' || GridMap[ArrToStr(nextSpace)] == ']'){
            //         boxExists = true;
            //     }
            //     else if(GridMap[ArrToStr(nextSpace)] != '.'){
            //         throw new Exception($"Invalid Value: {GridMap[ArrToStr(nextSpace)]}");
            //     }

            // }

            if(boxExists){

                if(MakeMove2(nextSpaces, pMovement) == false){
                    //Future barrier exists - no move
                    return false;
                }
            }

            HashSet<string> strCurPositions = new HashSet<string>();
            for(int cp = 0; cp < pCurrentPositions.Length; cp++){
                strCurPositions.Add(ArrToStr(pCurrentPositions[cp]));
            }
            foreach(int[] nextSpace in nextSpaces){
                int[] prevSpace = [nextSpace[0]-pMovement[0],nextSpace[1]-pMovement[1]];
                if(strCurPositions.Contains(ArrToStr(prevSpace))){
                    GridMap[ArrToStr(nextSpace)] = GridMap[ArrToStr(prevSpace)];
                }
                else{
                    GridMap[ArrToStr(nextSpace)] = '.';
                }
            }

            foreach(int[] currentSpace in pCurrentPositions){
                GridMap[ArrToStr(currentSpace)] = '.';
            }

            return true;
            
        }

        static int[] StrToArr(string input){
            string[] inputStrArr = input.Replace("[","").Replace("]","").Split(",");
            return [int.Parse(inputStrArr[0]),int.Parse(inputStrArr[1])];
        }

        static string ArrToStr(int[] input){
            return string.Concat($"[{input[0]},{input[1]}]");
        }

        static void PrintCurrentState(){
            string line = "";

            for(int v = 0; v < VerticalMax; v++){
                line = "";
                for(int h = 0; h < HorizontalMax; h++){
                    line = string.Concat(line, GridMap[ArrToStr([v,h])]);
                }
                Console.WriteLine(line);
            }
            Console.WriteLine("\n\n\n");
        }


        static int[] SetFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string mapFileName = "PuzzleInput1.txt";
            string movementFileName = "PuzzleInput2.txt";
            int[] startPosition = [0,0];

            //map
            string[] mapLines = File.ReadAllLines(string.Concat(fileDataDir, "/", mapFileName));

            // //map Parsing (part 1)
            // VerticalMax = mapLines.Length;
            // HorizontalMax = mapLines[0].Length;
            // for(int v = 0; v < VerticalMax; v++){
            //     for(int h = 0; h < HorizontalMax; h++){
            //         char c = mapLines[v][h];
            //         GridMap.Add(ArrToStr([v,h]),c);
            //         if(c=='@'){
            //             startPosition = [v,h];
            //         }
            //     }
            // }


            //map Parsing (part 2) - twice as wide
            VerticalMax = mapLines.Length;
            HorizontalMax = mapLines[0].Length*2;
            for(int v = 0; v < VerticalMax; v++){
                for(int h = 0; h < HorizontalMax/2; h++){
                    char c = mapLines[v][h];

                    switch(c){
                        case '#': //Add wide Barrier
                        case '.': //Add wide Free space
                            GridMap.Add(ArrToStr([v,h*2]),c);
                            GridMap.Add(ArrToStr([v,h*2+1]),c);
                            break;
                        case 'O': //Add wide Box
                            GridMap.Add(ArrToStr([v,h*2]),'[');
                            GridMap.Add(ArrToStr([v,h*2+1]),']');
                            break;
                        case '@': //Add robot with Free space
                            GridMap.Add(ArrToStr([v,h*2]),c);
                            GridMap.Add(ArrToStr([v,h*2+1]),'.');
                            startPosition = [v,h*2]; //Mark start position
                            break;
                        default:
                            throw new Exception($"Invalid Character: '{c}'");
                    }
                }
            }


            //movements
            string[] movements = File.ReadAllLines(string.Concat(fileDataDir, "/", movementFileName));
            // VerticalMax = mapLines.Length;
            // HorizontalMax = mapLines[0].Length;
            for(int v = 0; v < movements.Length; v++){
                for(int h = 0; h < movements[0].Length; h++){
                    switch(movements[v][h]){
                        case '<':
                            //LEFT
                            Movements.Add([0,-1]);
                            break;
                        case '^':
                            //UP
                            Movements.Add([-1,0]);
                            break;
                        case '>':
                            //RIGHT
                            Movements.Add([0,1]);
                            break;
                        case 'v':
                            //LEFT
                            Movements.Add([1,0]);
                            break;
                        default:
                            throw new Exception($"Invalid Direction: '{movements[v][h]}'");
                    }
                }

            }

            return startPosition;

        }
    }
}
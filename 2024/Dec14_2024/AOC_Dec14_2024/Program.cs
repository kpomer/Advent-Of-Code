using System;
using System.Collections;

namespace AdventOfCode
{
    public class Program
    {
        static void Main(string[] args)
        {

            List<int[]> fileData = GetFileData(); //String Array of all lines from PuzzleInput1.txt

            List<int[]> varianceLevels = new List<int[]>();

            // int[] quadCounts = new int[4];
            List<string> coordinates = new List<string>();
            int count = 0;
            while(count < 7000){
            // quadCounts = [0,0,0,0];
            coordinates = new List<string>(); //Clear
            int s = 1; //seconds
            int xLength = 101;
            int yLength = 103;
            foreach(int[] robot in fileData){
                //Initial Value - off the map
                int newX = robot[0] + (s*robot[2]);
                int newY = robot[1] + (s*robot[3]);
                
                //Fit to map (wrap around)
                newX = newX % xLength;
                if(newX < 0){
                    newX = newX + xLength;
                }

                newY = newY % yLength;
                if(newY < 0){
                    newY = newY + yLength;
                }

                robot[0] = newX;
                robot[1] = newY;

                coordinates.Add(String.Concat($"[{newX},{newY}]"));
                //Console.WriteLine($"X: {newX}, Y: {newY}");
                //Determine Quadrant

                
                // if(newX < xLength/2){
                //     if(newY < yLength/2){
                //         //Quad 1 (top left)
                //         quadCounts[0] = quadCounts[0] + 1;
                //     }
                //     else if(newY > yLength/2){
                //         //Quad 2 (bottom left)
                //         quadCounts[1] = quadCounts[1] + 1;
                //     }
                // }
                // else if(newX > xLength/2){
                //     if(newY < yLength/2){
                //         //Quad 3 (top right)
                //         quadCounts[2] = quadCounts[2] + 1;
                //     }
                //     else if(newY > yLength/2){
                //         //Quad 4 (bottom right)
                //         quadCounts[3] = quadCounts[3] + 1;
                //     }
                // }
                
            }

            if(count > 6580 && count < 6590){
                //CORRECT ANSWER 6587

                Console.WriteLine(count);
                PrintImage(coordinates, xLength, yLength);
            }
            // int varLevel = CheckVarianceLevel(coordinates, xLength, yLength); //max count where adjacent points exist in a single column
            // varianceLevels.Add([count, varLevel]);

            count += 1;
            if (count > 7000){
                long test = 0;
            }

            }

            // int countMax = 0;
            // foreach(int[] item in varianceLevels){
            //     if(item[1] > countMax){
            //         countMax = item[0];
            //     }
            // }

            // Console.WriteLine($"CountMax: {countMax}");

            // int safetyFactor = quadCounts[0] * quadCounts[1] * quadCounts[2] * quadCounts[3];

            // Console.WriteLine($"Safety Score: {safetyFactor}");
            
        }

        static int CheckVarianceLevel(List<string> pCoordinates, int xMax, int yMax){

            int columnLength = 0;
            int maxColumnLength = 0;
            for(int x = 0; x<xMax; x++){
                for(int y = 0; y<yMax; y++){
                    if(pCoordinates.Contains(string.Concat($"[{x},{y}]"))){
                        columnLength += 1;
                        if(columnLength > maxColumnLength){
                            maxColumnLength = columnLength;
                        }
                    }
                    else{
                        columnLength = 0;
                    }

                }
            }

            return maxColumnLength;

        }

        static void PrintImage(List<string> pCoordinates, int xMax, int yMax){
            
            string line = "";
            for(int y = 0; y<yMax; y++){
                line = "";
                for(int x = 0; x<xMax;x++){
                    if(pCoordinates.Contains(string.Concat($"[{x},{y}]"))){
                        line = string.Concat(line, ".");
                    }
                    else{
                        line = string.Concat(line, " ");
                    }
                }
                Console.WriteLine(line);
                Console.WriteLine("\n");
            }

        }


        static List<int[]> GetFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string fileName = "PuzzleInput1.txt";

            string[] robots = File.ReadAllLines(string.Concat(fileDataDir, "/", fileName));
            List<int[]> rPosVel = new List<int[]>();

            foreach(string r in robots){
                // string rValues = r.Replace("p=","").Replace("v=","").Replace("",",");
                string rValues = r.Replace("p=","");
                rValues = rValues.Replace("v=","");
                rValues = rValues.Replace(" ",",");

                string[] rValuesSplit = rValues.Split(",");

                rPosVel.Add([int.Parse(rValuesSplit[0]),int.Parse(rValuesSplit[1]),int.Parse(rValuesSplit[2]),int.Parse(rValuesSplit[3])]);
                //[x-Position, y-Position, x-Velocity, y-Velocity]

            }

            return rPosVel;

        }

    }
}
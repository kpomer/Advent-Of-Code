using System;
using System.Collections;

namespace AdventOfCode
{
    public class Program
    {


        public static char[,] FileData = new char[0,0];
        public static int verticalCount = 0;
        public static int horizontalCount = 0;
         
        static void Main(string[] args)
        {

            FileData = GetFileData(); //String Array of all lines from PuzzleInput1.txt
            
            // char[] searchString = ['X', 'M', 'A', 'S'];
            char[] searchString = ['M', 'A', 'S'];
            //string searchStringRev = "SAMX";
            long count = 0;

            for(int v = 0; v < verticalCount; v++){
                for(int h = 0; h < horizontalCount; h++){
                    // count += searchVertical(searchString, v, h);
                    // count += searchHorizontal(searchString, v, h);
                    // count += searchDiagonale(searchString, v, h);
                    if((xMasDiag(searchString, v, h, true) || xMasDiag(reverseCharArr(searchString), v, h, true)) && (xMasDiag(searchString, v, h+2, false) || xMasDiag(reverseCharArr(searchString), v, h+2, false))){
                        count += 1;
                    }
                }
            }

            Console.WriteLine($"Count: {count}");
        }

        static bool xMasDiag(char[] searchString, int vertical, int horizontal, bool topLeftbottomRight){
            //start Top Left to Bottom Right

            for(int i = 0; i < searchString.Length; i++){
                
                if(vertical >= 0 && vertical < verticalCount && horizontal >= 0 && horizontal < horizontalCount && FileData[vertical,horizontal] == searchString[i]){
                    //MATCH!
                }
                else{
                    break;
                }
                
                if(topLeftbottomRight){
                    horizontal += 1;
                    vertical += 1;
                }
                else{
                    horizontal -= 1;
                    vertical += 1;
                }

                if(i == searchString.Length - 1){
                    return true;
                }

            }

            return false;

        }


        // static long searchVertical(char[] searchString, int startVertical, int startHorizontal){
        //     //Search vertical
        //     long count = 0;

        //     //top to bottom
        //     int vertical = startVertical;
        //     int horizontal = startHorizontal;
        //     for(int i = 0; i < searchString.Length; i++){
                
        //         if(vertical >= 0 && vertical < verticalCount && horizontal >= 0 && horizontal < horizontalCount && FileData[vertical,horizontal] == searchString[i]){
        //             //MATCH!
        //         }
        //         else{
        //             break;
        //         }

        //         vertical += 1;
        //         if(i == searchString.Length - 1){
        //             count += 1;
        //         }

        //     }

        //     //bottom to top
        //     vertical = startVertical;
        //     horizontal = startHorizontal;
        //     for(int i = 0; i < searchString.Length; i++){
                
        //         if(vertical >= 0 && vertical < verticalCount && horizontal >= 0 && horizontal < horizontalCount && FileData[vertical,horizontal] == searchString[i]){
        //             //MATCH!
        //         }
        //         else{
        //             break;
        //         }

        //         vertical -= 1;
        //         if(i == searchString.Length - 1){
        //             count += 1;
        //         }

        //     }

        //     return count;
        // }

        // static long searchHorizontal(char[] searchString, int startVertical, int startHorizontal){
        //     //Search horizontal
        //     long count = 0;

        //     //left to right
        //     int vertical = startVertical;
        //     int horizontal = startHorizontal;
        //     for(int i = 0; i < searchString.Length; i++){
                
        //         if(vertical >= 0 && vertical < verticalCount && horizontal >= 0 && horizontal < horizontalCount && FileData[vertical,horizontal] == searchString[i]){
        //             //MATCH!
        //         }
        //         else{
        //             break;
        //         }

        //         horizontal += 1;
        //         if(i == searchString.Length - 1){
        //             count += 1;
        //         }

        //     }

        //     //right to left
        //     vertical = startVertical;
        //     horizontal = startHorizontal;
        //     for(int i = 0; i < searchString.Length; i++){
                
        //         if(vertical >= 0 && vertical < verticalCount && horizontal >= 0 && horizontal < horizontalCount && FileData[vertical,horizontal] == searchString[i]){
        //             //MATCH!
        //         }
        //         else{
        //             break;
        //         }

        //         horizontal -= 1;
        //         if(i == searchString.Length - 1){
        //             count += 1;
        //         }

        //     }

        //     return count;
        // }

        // static long searchDiagonale(char[] searchString, int startVertical, int startHorizontal){
        //     //Search diagonale
        //     long count = 0;

        //     //top left to bottom right
        //     int vertical = startVertical;
        //     int horizontal = startHorizontal;
        //     for(int i = 0; i < searchString.Length; i++){
                
        //         if(vertical >= 0 && vertical < verticalCount && horizontal >= 0 && horizontal < horizontalCount && FileData[vertical,horizontal] == searchString[i]){
        //             //MATCH!
        //         }
        //         else{
        //             break;
        //         }

        //         horizontal += 1;
        //         vertical += 1;
        //         if(i == searchString.Length - 1){
        //             count += 1;
        //         }

        //     }

        //     //top right to bottom left
        //     vertical = startVertical;
        //     horizontal = startHorizontal;
        //     for(int i = 0; i < searchString.Length; i++){
                
        //         if(vertical >= 0 && vertical < verticalCount && horizontal >= 0 && horizontal < horizontalCount && FileData[vertical,horizontal] == searchString[i]){
        //             //MATCH!
        //         }
        //         else{
        //             break;
        //         }

        //         horizontal -= 1;
        //         vertical += 1;
        //         if(i == searchString.Length - 1){
        //             count += 1;
        //         }

        //     }

        //     //bottom left to top right
        //     vertical = startVertical;
        //     horizontal = startHorizontal;
        //     for(int i = 0; i < searchString.Length; i++){
                
        //         if(vertical >= 0 && vertical < verticalCount && horizontal >= 0 && horizontal < horizontalCount && FileData[vertical,horizontal] == searchString[i]){
        //             //MATCH!
        //         }
        //         else{
        //             break;
        //         }

        //         horizontal -= 1;
        //         vertical -= 1;
        //         if(i == searchString.Length - 1){
        //             count += 1;
        //         }

        //     }

        //     //bottom right to top left
        //     vertical = startVertical;
        //     horizontal = startHorizontal;
        //     for(int i = 0; i < searchString.Length; i++){
                
        //         if(vertical >= 0 && vertical < verticalCount && horizontal >= 0 && horizontal < horizontalCount && FileData[vertical,horizontal] == searchString[i]){
        //             //MATCH!
        //         }
        //         else{
        //             break;
        //         }

        //         horizontal += 1;
        //         vertical -= 1;
        //         if(i == searchString.Length - 1){
        //             count += 1;
        //         }

        //     }

        //     return count;
        // }

        static char[] reverseCharArr(char[] inputString){
            char[] reversedString = new char[inputString.Length];

            for(int i = 0; i < inputString.Length; i++){
                reversedString[i] = inputString[inputString.Length - 1 - i];
            }

            return reversedString;

        }


        static char[,] GetFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string fileName = "PuzzleInput1.txt";

            string[] fileStringArr = File.ReadAllLines(string.Concat(fileDataDir, "/", fileName)); 

            verticalCount = fileStringArr.Count<string>();
            horizontalCount = fileStringArr[0].ToString().Length;
            char[,] characters = new char[verticalCount,horizontalCount];

            for(int v = 0; v < verticalCount; v++){
                string line = fileStringArr[v].ToString();
                char[] lineStrArr = line.ToCharArray();

                for(int h = 0; h < line.Length; h++){
                    characters[v,h] = lineStrArr[h];
                }
            }

            return characters;

        }

    }
}
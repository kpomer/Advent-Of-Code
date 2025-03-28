using System;
using System.Collections;

namespace AdventOfCode
{
    public class Program
    {
        static void Main(string[] args)
        {

            string[] fileData = GetFileData(); //String Array of all lines from PuzzleInput1.txt
     
            //TODO Add Program Logic Here 

        }


        static string[] GetFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string fileName = "PuzzleInput1.txt";

            return File.ReadAllLines(string.Concat(fileDataDir, "/", fileName));

        }

    }
}
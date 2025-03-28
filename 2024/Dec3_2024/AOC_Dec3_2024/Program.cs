using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace AdventOfCode
{
    public class Program
    {
        static void Main(string[] args)
        {

            string fileData = GetFileData(); //String Array of all lines from PuzzleInput1.txt
            string regexDoDont = @"mul\(\d+,\d+\)|do\(\)|don't\(\)";
            //string mulRegex = @"mul\(\d+,\d+\)"; 
            long summedMatches = 0;
            
            MatchCollection matches = Regex.Matches(fileData, regexDoDont);
            bool flag = true;
            foreach (Match m in matches){
                if(m.ToString() == "do()"){
                    flag = true;
                }
                else if(m.ToString() == "don't()"){
                    flag = false;
                }
                else if(flag == true){
                    summedMatches += multiplyMatch(m.ToString());
                }
                
            }

            Console.WriteLine($"Summed products: {summedMatches}");
            

        }

        static long multiplyMatch(string matchText){

            long m1 = long.Parse(Regex.Match(matchText, @"(mul\()(.*)(,)").Groups[2].ToString());
            long m2 = long.Parse(Regex.Match(matchText, @"(,)(.*)(\))").Groups[2].ToString());

            return m1*m2;
        }

 


        static string GetFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string fileName = "PuzzleInput1.txt";

            return File.ReadAllText(string.Concat(fileDataDir, "/", fileName));

        }

    }
}
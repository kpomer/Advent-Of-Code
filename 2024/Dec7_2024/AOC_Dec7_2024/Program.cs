using System;
using System.Collections;

namespace AdventOfCode
{
    public class Program
    {
        
        public static char[] mathOpsList; //All possible mathOps

        static void Main(string[] args)
        {

            //mathOpsList = ['+','*']; //Initialize All MathOps options (Part 1)
            mathOpsList = ['+','*','|']; //Initialize All MathOps options (Part 2)
            //mathOpsList = ['0','1']; //Initialize All MathOps options

            string[] fileData = GetFileData(); //String Array of all lines from PuzzleInput1.txt
            long sumCalibrations = 0;
            foreach(string eqLine in fileData){
                List<char[]> lstOpOptions = GetAllEqOps(eqLine);
                
                // Console.WriteLine(eqLine);
                // Console.WriteLine($"Contains {eqLine.Split(' ').Count<string>() - 2} spaces, which gives {Math.Pow((mathOpsList.Count<char>()), (eqLine.Split(' ').Count<string>() - 2))} options");
                // for(int i = 0; i < lstOpOptions.Count<char[]>(); i++){
                //     string strValues = new string(lstOpOptions[i]);
                //     Console.WriteLine($"{i+1} - {strValues}");
                // }
                
                foreach(char[] opOptions in lstOpOptions){
                    long calibrationVal = 0;
                    if(CheckValidOperators(eqLine, opOptions, ref calibrationVal)){
                        sumCalibrations += calibrationVal;
                        break;
                    }
                }

            }

            Console.WriteLine($"Summed Valid Calibrations: {sumCalibrations}");
        
        }

        static bool CheckValidOperators(string pEqLine, char[] pOpOptions, ref long pCalibrationVal){
            string[] pEqValues = pEqLine.Split(' ');

            //PART 2 - incorrect solution
            //ConcatOperator(ref pEqValues, ref pOpOptions);

            pCalibrationVal = long.Parse(pEqValues[0].Replace(":",""));
            long calcVal = 0;

            for(int i = 1; i < pEqValues.Length; i++){
                if(i == 1){
                    calcVal = long.Parse(pEqValues[i]);
                }
                else{
                    switch (pOpOptions[i-2]){
                        case '+':
                            calcVal = calcVal + long.Parse(pEqValues[i]);
                            break;
                        case '*':
                            calcVal = calcVal * long.Parse(pEqValues[i]);
                            break;
                        case '|':
                            calcVal = long.Parse($"{calcVal}{pEqValues[i]}");
                            break;
                        default:
                            throw new Exception($"Invalid value: {pOpOptions[i-1]}");
                    }

                }
            }

            return pCalibrationVal == calcVal; //return whether they match
        }

        static void ConcatOperator(ref string[] pEqValues, ref char[] pOpOptions){

            List<char> ConcatOpOptions = new List<char>();
            string fullEquation = "";

            for(int i = 0; i < pEqValues.Length; i++){
                if(i == 0){
                    //Calibration
                    fullEquation = String.Concat(fullEquation, pEqValues[i], " ");
                }
                else if(i == 1){
                    //1st Value
                    fullEquation = String.Concat(fullEquation, pEqValues[i]);
                }
                else{
                    //Operator and Other Values
                    fullEquation = String.Concat(fullEquation, pOpOptions[i-2], pEqValues[i]);
                    if(pOpOptions[i-2] != '|'){
                        //Add non-| values to new list
                        ConcatOpOptions.Add(pOpOptions[i-2]);
                    }
                }
            }

            //Replace concat values, and remove other operators
            fullEquation = fullEquation.Replace("+", " "); //space
            fullEquation = fullEquation.Replace("*", " "); //space
            fullEquation = fullEquation.Replace("|", ""); //No space
            pEqValues = fullEquation.Split(' ');

            //Char array with removed | values
            pOpOptions = ConcatOpOptions.ToArray<char>();

        }

        static List<char[]> GetAllEqOps(string pEqLine){
            List<char[]> lstEqOpsCombinations = new List<char[]>();

            int numSpaces = pEqLine.Split(' ').Count<string>() - 2;
            int numOpTypes = mathOpsList.Count<char>();

            for(int i = 0; i < Math.Pow(numOpTypes, numSpaces); i++)
            {
                char[] cEqOpsCombination = new char[numSpaces];
                int val = i;
                for(int j = 1; j <= numSpaces; j++){
                    cEqOpsCombination[numSpaces-j] = mathOpsList[val % numOpTypes];
                    val = val / numOpTypes;
                }
                // string strVal = new string(cEqOpsCombination);
                // Console.WriteLine($"{i} - {strVal}");
                lstEqOpsCombinations.Add(cEqOpsCombination);
            }
            
            return lstEqOpsCombinations;
        }


        static string[] GetFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string fileName = "PuzzleInput1.txt";

            return File.ReadAllLines(string.Concat(fileDataDir, "/", fileName));

        }

    }
}
using System;
using System.Collections;
using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode
{
    public class Program
    {

        static long RegA = 0;
        static long RegB = 0;
        static long RegC = 0;
        static long InstructionPointer = 0;
        static string OutputVal = "";
        static List<long> ProgramInput = new List<long>();

        //PART 1
        static void Main(string[] args)
        {

            SetFileData(); //Set values from PuzzleInput1.txt

            // //PART 1
            // InstructionPointer = 0;
            // bool moveNext = true;
            // while(InstructionPointer < ProgramInput.Count){
            //     moveNext = PerformOp(ProgramInput[Convert.ToInt32(InstructionPointer)], ProgramInput[Convert.ToInt32(InstructionPointer)+1]);
                
            //     if(moveNext == true){
            //         //some operations do NOT move next
            //         InstructionPointer += 2;
            //     }
            // }
            // Console.WriteLine($"Program Output: {OutputVal}");

            //PART 2
            int[] octVals = [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]; //octals
            int bitCount = 1;
            string expectedOutput = GetExpectedOutput(bitCount);

            while(bitCount < 17){
                RunProgram(octVals, bitCount);
                expectedOutput = GetExpectedOutput(bitCount);

                if(expectedOutput == OutputVal){
                    if(bitCount == 16){
                        break;
                    }
                    else{
                        bitCount += 1;
                    }
                }
                else if(octVals[bitCount - 1] < 7){
                    octVals[bitCount - 1] += 1;
                }
                else{
                    //dead end
                    octVals[bitCount - 1] = 0;
                    bitCount -= 1;
                    octVals[bitCount - 1] += 1;
                }

            }

            string octalString = "";
            for(int i = 0; i < bitCount; i++){
                octalString = String.Concat(octalString, octVals[i]);
            }
            Console.WriteLine($"Octal Value: {octalString}");
            Console.WriteLine($"Decimal Value: {Convert.ToInt64(octalString, 8)}");

        }

        // static void Main(string[] args)
        // {

        //     SetFileData(); //needed to setup initial logic
             
        //     int[] octVals = [0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0]; //octals
        //     int bitCount = 1;
        //     string expectedOutput = GetExpectedOutput(bitCount);

        //     while(bitCount < 17){
        //         RunProgram(octVals, bitCount);
        //         expectedOutput = GetExpectedOutput(bitCount);

        //         Console.WriteLine($"BitCount: {bitCount}");
        //         Console.WriteLine($"Expected: {expectedOutput}");
        //         Console.WriteLine($"Actual: {OutputVal}");
        //         Console.WriteLine($"\n\n\n");

        //         if(expectedOutput == OutputVal){
        //             if(bitCount == 16){
        //                 break;
        //             }
        //             else{
        //                 bitCount += 1;
        //             }
        //         }
        //         else if(octVals[bitCount - 1] < 7){
        //             octVals[bitCount - 1] += 1;
        //         }
        //         else{
        //             //dead end
        //             octVals[bitCount - 1] = 0;
        //             bitCount -= 1;
        //             octVals[bitCount - 1] += 1;
        //         }

        //     }

        //     string octalString = "";
        //     for(int i = 0; i < bitCount; i++){
        //         octalString = String.Concat(octalString, octVals[i]);
        //     }
        //     Console.WriteLine($"Octal Value: {octalString}");
        //     Console.WriteLine($"Decimal Value: {Convert.ToInt64(octalString, 8)}");

        // }
        

        

        static string RunProgram(int[] pRegA_InputOct, int pBitCount){
            string octalString = "";
            for(int i = 0; i < pBitCount; i++){
                octalString = String.Concat(octalString, pRegA_InputOct[i]);
            }
            long regAInput = Convert.ToInt64(octalString, 8);

            ProgramInput = new List<long>(); //reset
            OutputVal = ""; //reset
            SetFileDataPart2(regAInput); //Set values from PuzzleInput1.txt

            InstructionPointer = 0;
            bool moveNext = true;
            while(InstructionPointer < ProgramInput.Count){
                moveNext = PerformOp(ProgramInput[Convert.ToInt32(InstructionPointer)], ProgramInput[Convert.ToInt32(InstructionPointer)+1]);
                
                if(moveNext == true){
                    //some operations do NOT move next
                    InstructionPointer += 2;
                }
            }
            return OutputVal;
        }

        static string GetExpectedOutput(int bitCount){
            string expectedOutput = "";
            for(int o = ProgramInput.Count - bitCount; o < ProgramInput.Count; o++){
                if(expectedOutput != ""){
                    expectedOutput = String.Concat(expectedOutput, ",");
                }
                expectedOutput = String.Concat(expectedOutput, ProgramInput[o]);
            }

            return expectedOutput;
        }

        static void PrintDetails(){
            Console.WriteLine($"RegA: {RegA}\nRegB: {RegB}\nRegC: {RegC}\n\n\n");
        }

        static bool PerformOp(long pOpCode, long pOperand){

            bool moveNext = true;

            switch (pOpCode)
            {
                case 0:
                    //Operation 'adv'
                    Double num = RegA;
                    Double den = Math.Pow(2, GetComboOperand(pOperand));
                    RegA = Convert.ToInt64(Math.Round(num/den, 0, MidpointRounding.ToZero));
                    break;
                case 1:
                    //Operation 'bxl'
                    RegB = XOR(RegB, pOperand);
                    break;
                case 2:
                    //Operation 'bst'
                    RegB = GetComboOperand(pOperand) % 8;
                    break;
                case 3:
                    //Operation 'jnz'
                    if(RegA != 0){
                        InstructionPointer = pOperand;
                        moveNext = false;
                    }
                    break;
                case 4:
                    //Operation 'bxc'
                    RegB = XOR(RegB, RegC);
                    break;
                case 5:
                    //Operation 'out'
                    AddOutput(GetComboOperand(pOperand) % 8);
                    break;
                case 6:
                    //Operation 'bdv'
                    Double numB = RegA;
                    Double denB = Math.Pow(2, GetComboOperand(pOperand));
                    RegB = Convert.ToInt64(Math.Round(numB/denB, 0, MidpointRounding.ToZero));
                    break;
                case 7:
                    //Operation 'cdv'
                    Double numC = RegA;
                    Double denC = Math.Pow(2, GetComboOperand(pOperand));
                    RegC = Convert.ToInt64(Math.Round(numC/denC, 0, MidpointRounding.ToZero));
                    break;
                default:
                    throw new Exception($"Invalid Operation Code: {pOpCode}");
            }

            return moveNext;

        }


        static int[] IntToBits(long pInputInt, int pLength){
            //6 = (1)*2^2 + (1)*2^1 + (0)*2^2
            int[] bits = new int[pLength];

            for(int j = 1; j <= bits.Length; j++){
                bits[bits.Length - j] = Convert.ToInt32(pInputInt % 2);
                pInputInt = pInputInt / 2;
            }

            return bits;
        }

        static long BitsToInt(int[] pInputBits){
            Double value = 0;
            //6 = (1)*2^2 + (1)*2^1 + (0)*2^2
            for(int j = 0; j < pInputBits.Length; j++){
                value += pInputBits[j] * Math.Pow(2, pInputBits.Length-j-1);
            }

            return Convert.ToInt64(value);
        }

        static string BitsToString(int[] pInputBits){
            string outputString = "";
            foreach(int b in pInputBits){
                outputString = string.Concat(outputString, b.ToString());
            }
            return outputString;
        }

        static long XOR(long i1, long i2){
            long maxNum = long.Max(i1, i2);
            int numBits = 1;
            if(maxNum > 0){
                numBits = Convert.ToInt32(Math.Round(Math.Log(maxNum, 2) + 1,0,MidpointRounding.ToZero));
            }

            int[] i1_Bits = IntToBits(i1, numBits);
            int[] i2_Bits = IntToBits(i2, numBits);

            int[] xorValue = new int[numBits];
            for(int i = 0; i < numBits; i++){
                if(i1_Bits[i] == i2_Bits[i]){
                    xorValue[i] = 0;
                }
                else{
                    xorValue[i] = 1;
                }
            }
            return BitsToInt(xorValue);
        }

        static void AddOutput(long value){
            if(OutputVal.Length != 0){
                OutputVal = String.Concat(OutputVal, ",");
            }

            OutputVal = String.Concat(OutputVal, value.ToString());
        }

        static long GetComboOperand(long pOperand){

            switch (pOperand)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    //Literal Value
                    return pOperand;
                case 4:
                    //Register A
                    return RegA;
                case 5:
                    //Register B
                    return RegB;
                case 6:
                    //Register C
                    return RegC;
                default:
                    throw new Exception($"Invalid Operand: {pOperand}");
            }

        }


        static void SetFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string fileName = "PuzzleInput1.txt";

            string[] inputStrings = File.ReadAllLines(string.Concat(fileDataDir, "/", fileName));

            foreach(string l in inputStrings){
                string[] lSplit = l.Split(" ");
                if(lSplit.Length >= 2){
                    if(lSplit[1] == "A:"){
                        RegA = long.Parse(lSplit[2]); //decimal
                        //RegA = Convert.ToInt64(lSplit[2], 2); //binary

                    }
                    else if(lSplit[1] == "B:"){
                        RegB = long.Parse(lSplit[2]);
                    }
                    else if(lSplit[1] == "C:"){
                        RegC = long.Parse(lSplit[2]);
                    }
                    else if(lSplit[0] == "Program:"){
                        foreach(string pi in lSplit[1].Split(",")){
                            ProgramInput.Add(long.Parse(pi));
                        }
                    }
                }
            }

        }

        static void SetFileDataPart2(long pRegA){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string fileName = "PuzzleInput1.txt";

            string[] inputStrings = File.ReadAllLines(string.Concat(fileDataDir, "/", fileName));

            foreach(string l in inputStrings){
                string[] lSplit = l.Split(" ");
                if(lSplit.Length >= 2){
                    if(lSplit[1] == "A:"){
                        //Input from Parameter
                        RegA = pRegA;
                    }
                    else if(lSplit[1] == "B:"){
                        RegB = long.Parse(lSplit[2]);
                    }
                    else if(lSplit[1] == "C:"){
                        RegC = long.Parse(lSplit[2]);
                    }
                    else if(lSplit[0] == "Program:"){
                        foreach(string pi in lSplit[1].Split(",")){
                            ProgramInput.Add(long.Parse(pi));
                        }
                    }
                }
            }

        }

    }
}
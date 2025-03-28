using System;
using System.Collections;
using System.Security;

namespace AdventOfCode
{
    public class Program
    {
        static Dictionary<string,bool> WireValues = new Dictionary<string, bool>(); //wire: value
        static string[][] GateInstructions = new string[0][]; //[ [intputWire1],[gate],[inputWire2],[outputWire] ]
        static HashSet<string[]> ExecutedInstructions = new HashSet<string[]>();
        static void Main(string[] args)
        {

            SetFileData();

            Console.WriteLine($"x = {GetDecimalBitValue('x')}");
            Console.WriteLine($"y = {GetDecimalBitValue('y')}");
     
            string[][] activeInstructions = GetActiveInstructions();
            while(activeInstructions.Length != 0){

                foreach(string[] inputOutput in activeInstructions){
                    ExecuteGateLogic(inputOutput);
                    ExecutedInstructions.Add(inputOutput);
                }                

                activeInstructions = GetActiveInstructions();
            }
            
            Int64 zValue = GetDecimalBitValue('z');
            Console.WriteLine($"\nPart 1: z = {zValue}");

            //Part 2
            List<string[]> gateList = [.. GateInstructions];
            List<string> swapList = new List<string>();

            foreach(string[] i in GateInstructions){
                if((i[0] == "x00" || i[2] == "x00") && (i[0] == "y00" || i[2] == "y00")){
                    //skip validation on 1st
                    continue;
                }
                else if(i[3] == "z45"){
                    //skip validation on last
                    continue; 
                }
                else if((i[0][0] == 'x' || i[0][0] == 'y') && (i[2][0] == 'x' || i[2][0] == 'y')){
                    //Start
                    if(i[1] == "AND" && gateList.FindIndex(a => (a[0] == i[3] || a[2] == i[3]) && a[1] == "OR") == -1){
                        //OR does not follow AND - INVALID
                        swapList.Add(i[3]);
                    }
                    else if(i[1] == "XOR" && gateList.FindIndex(a => (a[0] == i[3] || a[2] == i[3]) && a[1] == "AND") == -1){
                        //AND does not follow XOR - INVALID
                        swapList.Add(i[3]);
                    }
                    else if(i[0][0] == i[2][0]){
                        //x and y must be used separately
                        swapList.Add(i[3]);
                    }
                    else if(i[1] == "OR"){
                        swapList.Add(i[3]);
                    }
                }
                else if(i[3][0] == 'z' && i[1] != "XOR"){
                    //End without XOR - INVALID
                    swapList.Add(i[3]);
                }
                else if(i[3][0] != 'z' && i[1] == "XOR"){
                    //MIDDLE with XOR - INVALID
                    swapList.Add(i[3]);
                }

            }
            swapList.Sort();
            string swapString = "";
            foreach(string s in swapList){
                if(swapString != ""){
                    swapString = swapString + ",";
                }
                swapString = swapString + s;
            }

            Console.WriteLine($"Part 2: {swapString}");

        }

        static string[][] GetActiveInstructions(){
            List<string[]> activeInstructionList = new List<string[]>();

            foreach(string[] i in GateInstructions){
                if(!ExecutedInstructions.Contains(i) && WireValues.ContainsKey(i[0]) && WireValues.ContainsKey(i[2])){
                    activeInstructionList.Add(i);
                }
            }

            return activeInstructionList.ToArray();

        }

        static void ExecuteGateLogic(string[] pInputOutput){

            switch(pInputOutput[1]){
                case "AND":
                    WireValues[pInputOutput[3]] = WireValues[pInputOutput[0]] & WireValues[pInputOutput[2]];
                    break;
                case "OR":
                    WireValues[pInputOutput[3]] = WireValues[pInputOutput[0]] | WireValues[pInputOutput[2]];
                    break;
                case "XOR":
                    WireValues[pInputOutput[3]] = WireValues[pInputOutput[0]] ^ WireValues[pInputOutput[2]];
                    break;
                default:
                    throw new Exception($"Invalid Gate: {pInputOutput[1]}");
            }

        }

        static void PrintComponentsBackwards(List<string> components){

            List<string[]> gateList = [.. GateInstructions];

            if(components.FindIndex(a => a!="AND" && a!="OR" && a!="XOR" && a!="[" && a!="]" && a[0]!='x' && a[0]!='y') == -1){
                //print final values
                string printComponents = "";
                foreach(string t in components){
                    printComponents = String.Concat(printComponents, " ", t);
                }
                Console.WriteLine(printComponents);
                return;
            }
            else{
                // //print values
                if(components.Count == 1){
                    Console.WriteLine(components[0]);
                }

                //break into smaller
                List<string> components2 = new List<string>(components);
                components = new List<string>();
                foreach(string item in components2){
                    //get components
                    int index = gateList.FindIndex(a => a[3] == item);
                    if(index != -1){
                        components.Add("[");
                        components.Add(gateList[index][0]);
                        components.Add(gateList[index][1]);
                        components.Add(gateList[index][2]);
                        components.Add("]");
                    }
                    else{
                        components.Add(item);
                    }
                }

                List<string> componentParameter = new List<string>(components);

                PrintComponentsBackwards(componentParameter);
            }

        }


        static Int64 GetDecimalBitValue(char pBitPrefix){

            List<string> wires = new List<string>();

            foreach(string w in WireValues.Keys){
                if(w[0] == pBitPrefix){
                    wires.Add(w);
                }
            }

            wires.Sort((y, x) => x.CompareTo(y));

            string binaryString = "";
            foreach(string w in wires){
                switch(WireValues[w]){
                    case true:
                        binaryString = binaryString + "1";
                        break;
                    case false:
                        binaryString = binaryString + "0";
                        break;
                    default:
                        throw new Exception($"Invalid Value: {WireValues[w]}");
                }
            }

            return Convert.ToInt64(binaryString, 2);
        }


        static void SetFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string fileName = "PuzzleInput1.txt";

            string[] fileSections = File.ReadAllText(string.Concat(fileDataDir, "/", fileName)).Split("\n\n");

            //Set Wires with Values
            foreach(string w in fileSections[0].Split("\n")){
                bool wireVal = false;
                switch(w.Split(": ")[1]){
                    case "0":
                        wireVal = false;
                        break;
                    case "1":
                        wireVal = true;
                        break;
                    default:
                        throw new Exception($"Invalid wire value: {w.Split(": ")[1]}");
                }
                WireValues[w.Split(": ")[0]] = wireVal;
            }

            //Set Gate Instructions
            long instructionCount = fileSections[1].Split("\n").Length;
            GateInstructions = new string[instructionCount][];
            for(int i = 0; i < instructionCount; i++){
                string[] formattedInstruction = fileSections[1].Split("\n")[i].Replace(" -> ", " ").Split(" ");
                GateInstructions[i] = formattedInstruction;
            }

        }

    }
}
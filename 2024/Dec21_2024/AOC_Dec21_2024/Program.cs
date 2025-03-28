using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace AdventOfCode
{
    public class Program
    {

        static long LayerCount = 4;
        static Dictionary<char,int[]> Keypad_Nums = new Dictionary<char, int[]>(); //numbers
        static Dictionary<char,int[]> Keypad_Arrows = new Dictionary<char, int[]>(); //arrows
        static Dictionary<char[],string[]> KeyPaths = new Dictionary<char[], string[]>();
        static Dictionary<string,long> Cached_GetLength = new Dictionary<string, long>();
        static Dictionary<int[], string> Directions = new Dictionary<int[], string>
        {
            { [0,-1], "<" },
            { [-1,0], "^" },
            { [0,1], ">" },
            { [1,0], "v" },
        };
        
        static void Main(string[] args)
        {

            string[] fileData = GetFileData(); //String Array of all lines from PuzzleInput1.txt

            long part1 = 0;
            long part2 = 0;
            foreach(string code in fileData){
                long minLength = long.MaxValue;
                long minLength2 = long.MaxValue;
                foreach(string path1 in GetCodePaths(Keypad_Nums, code)){
                    minLength = Math.Min(minLength, GetLength(path1, 2));
                    minLength2 = Math.Min(minLength2, GetLength(path1, 25));

                    // foreach(string path2 in GetCodePaths(Keypad_Arrows, path1)){
                    //     foreach(string path3 in GetCodePaths(Keypad_Arrows, path2)){
                    //         minLength = Math.Min(path3.Length, minLength);
                    //     }
                    // }
                }
                part1 += long.Parse(code.Substring(0, code.Length-1)) * minLength;
                part2 += long.Parse(code.Substring(0, code.Length-1)) * minLength2;
            }

            Console.WriteLine($"Part 1 Length: {part1}");
            Console.WriteLine($"Part 1 Length: {part2}");


        }

        static long GetLength(string path, long robots){
            string inputString = $"{path}_{robots}";

            if(Cached_GetLength.ContainsKey(inputString) == false){
                if(robots == 0){
                    Cached_GetLength[inputString] = path.Length;
                }
                else{
                    long length = 0;
                    char[] toVal = path.ToCharArray();
                    char[] fromVal = new char[toVal.Length];
                    fromVal[0] = 'A'; //Always start from A
                    for(int i = 0; i < toVal.Length - 1; i++){
                        fromVal[i+1] = toVal[i];
                    }

                    for(int v = 0; v < fromVal.Length; v++){
                        long minLength = long.MaxValue;
                        foreach(string p in GetBestPaths(fromVal[v], toVal[v], Keypad_Arrows)){
                            minLength = Math.Min(minLength, GetLength(p, robots - 1));
                        }
                        length += minLength;
                    }
                    
                    Cached_GetLength[inputString] = length;

                }
            }

            return Cached_GetLength[inputString];
        }

        static string[] GetCodePaths(Dictionary<char,int[]> Keypad, string code){
            //Setup char arrays to iterate through
            char[] toVal = code.ToCharArray();
            char[] fromVal = new char[toVal.Length];
            fromVal[0] = 'A'; //Always start from A
            for(int i = 0; i < toVal.Length - 1; i++){
                fromVal[i+1] = toVal[i];
            }

            string[][] paths = new string[fromVal.Length][];

            for(int v = 0; v < fromVal.Length; v++){
                paths[v] = GetBestPaths(fromVal[v], toVal[v], Keypad);
            }

            List<string> allPaths = new List<string>();

            foreach(string p in paths[0]){
                GetAllCombos(paths, 1, p, ref allPaths);
            }

            return allPaths.ToArray();
            
        }

        static void GetAllCombos(string[][] paths, int level, string pathString, ref List<string> pAllPaths){
            
            if(paths.Length > level){
                foreach(string p in paths[level]){
                    GetAllCombos(paths, level + 1, pathString+p, ref pAllPaths);
                }
            }
            else{
                pAllPaths.Add(pathString);
            }       

        }





        static string[] GetFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string fileName = "PuzzleInput1.txt";

            //Setup Keypads and create Dict of all shortest paths between arrows
            ParseKeypad(ref Keypad_Nums, "789\n456\n123\n 0A");
            ParseKeypad(ref Keypad_Arrows, " ^A\n<v>");

            foreach(char c1 in Keypad_Nums.Keys){
                foreach(char c2 in Keypad_Nums.Keys){
                    if(c1 != ' ' && c2 != ' '){
                        KeyPaths.Add([c1,c2],GetBestPaths(c1, c2, Keypad_Nums));
                    }
                }
            }

            foreach(char c1 in Keypad_Arrows.Keys){
                foreach(char c2 in Keypad_Arrows.Keys){
                    if(c1 != ' ' && c2 != ' '){
                        KeyPaths.Add([c1,c2],GetBestPaths(c1, c2, Keypad_Arrows));
                    }
                }
            }

            return File.ReadAllLines(string.Concat(fileDataDir, "/", fileName));

        }

        static void ParseKeypad(ref Dictionary<char,int[]> pKeypad, string pKeyValues){
            
            string[] keyLines = pKeyValues.Split("\n");
            for(int v = 0; v < keyLines.Length; v++){
                char[] keyLineChars = keyLines[v].ToCharArray();
                for(int h = 0; h < keyLineChars.Length; h++){
                    pKeypad[keyLineChars[h]] = [v,h];
                }
            }

        }

        static string[] GetBestPaths(char c1, char c2, Dictionary<char,int[]> Keypad){

            int[] c2_Coordinates = Keypad[c2];

            List<string> shortestPaths = new List<string>();

            Queue<int[]> queue = new Queue<int[]>();
            Dictionary<int[],string> queuePath = new Dictionary<int[], string>();
            List<int[]> unvisited = new List<int[]>(Keypad.Values);
            unvisited.Remove(Keypad[' ']); //don't store empty space

            //BFS
            int shortestLength = 100; //placeholder
            queue.Enqueue(Keypad[c1]);
            queuePath[Keypad[c1]] = "";
            unvisited.Remove(Keypad[c1]);

            while(queue.Count != 0 && queuePath[queue.Peek()].Length < shortestLength){
                int[] current = queue.Dequeue();
                if(c2_Coordinates[0] == current[0] && c2_Coordinates[1] == current[1]){
                    shortestPaths.Add(queuePath[current] + "A");
                    shortestLength = queuePath[current].Length + 1;
                    continue;
                }

                foreach(int[] dir in Directions.Keys){
                    int[] adjNode = [current[0] + dir[0], current[1] + dir[1]];
                    if(unvisited.FindIndex(a => (a[0] == adjNode[0]) && (a[1] == adjNode[1])) != -1){
                        queue.Enqueue(adjNode);
                        queuePath[adjNode] = queuePath[current] + Directions[dir];
                        unvisited.Remove(adjNode);
                    }
                }

            }

            return shortestPaths.ToArray();
        }

    }
}
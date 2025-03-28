using System;
using System.Collections;

namespace MyNamespace
{
    public class Program
    {
        static void Main(string[] args)
        {

            string[] fileData = GetFileData();
            
            List<long> locList1 = new List<long>();
            List<long> locList2 = new List<long>();

            foreach (string line in fileData){

                string[] lineSplit = line.Split("   ");
                
                locList1.Add(long.Parse(lineSplit[0]));
                locList2.Add(long.Parse(lineSplit[1]));

            }

            locList1.Sort();
            locList2.Sort();

            /* Dec1 Part 1

            long count = 0;

            for(int i = 0; i < locList1.Count(); i++){
                count += Math.Abs(locList1[i] - locList2[i]);
            }

            Console.WriteLine($"Abs Sum = {count}");
            */


            long similarity = 0;
            
            for(int i = 0; i < locList1.Count(); i++){

                long val = locList1[i];
                long valCount = GetEntryCount(locList2, val);

                similarity += val * valCount;

            }

            Console.WriteLine($"Similarity = {similarity}");
     
        }


        static string[] GetFileData(){
            string fileDataDir = Directory.GetCurrentDirectory().ToString(); //File.ReadAllLines(Directory)
            string fileName = "PuzzleInput_Dec1_2024.txt";

            return File.ReadAllLines(string.Concat(fileDataDir, "/", fileName));

        }


        static int GetEntryCount(List<long> plocList, long l){

            int count = 0;
            int index = 0;

            while (index != -1 && index <= (plocList.Count - 1)){
                
                index = plocList.IndexOf(l, index);
                if(index != -1){
                    count += 1;
                    index += 1;
                }
            }

            return count;
        }

    }
}
using System;
using System.Collections;

namespace AdventOfCode
{
    public class Program
    {

        static HashSet<string> SequencePreviouslyUsed = new HashSet<string>();
        static Dictionary<string,long> SequenceBananaTotal = new Dictionary<string, long>();

        static void Main(string[] args)
        {

            long[] initSecNums = GetFileData(); //Long Array of all lines from PuzzleInput1.txt
            long[,] last4Seq = new long[4,2]; //long array to store last 4 sequence
            
            long totalAfter2k = 0;
            long monkeyCount = 0;
            foreach(long i in initSecNums){
                last4Seq = new long[4,2];
                long sn = i;
                for(int c = 0; c < 2000; c++){
                    sn = GetNextSecretNumber(sn);
                    
                    UpdateLast4Seq(ref last4Seq, sn);
                    if(c >= 3){
                        AddSequenceBananas(monkeyCount, last4Seq);
                    }
                }
                totalAfter2k += sn;
                monkeyCount += 1;
            }

            long maxBananas = 0;
            string maxSequence = "";
            foreach(string seq in SequenceBananaTotal.Keys){
                if(SequenceBananaTotal[seq] > maxBananas){
                    maxSequence = seq;
                    maxBananas = SequenceBananaTotal[seq];
                }
            }

            Console.WriteLine($"Part 1: {totalAfter2k}");
            Console.WriteLine($"Part 2: {maxBananas}");

        }


        static void UpdateLast4Seq(ref long[,] pLast4Seq, long pNewSN){

            //[SeqChange, newVal]
            
            pLast4Seq[0,0] = pLast4Seq[1,0];
            pLast4Seq[0,1] = pLast4Seq[1,1];
            pLast4Seq[1,0] = pLast4Seq[2,0];
            pLast4Seq[1,1] = pLast4Seq[2,1];
            pLast4Seq[2,0] = pLast4Seq[3,0];
            pLast4Seq[2,1] = pLast4Seq[3,1];


            long newSN_LastDigit = pNewSN % 10;
            pLast4Seq[3,0] = newSN_LastDigit - pLast4Seq[2,1];
            pLast4Seq[3,1] = newSN_LastDigit;

        }

        static void AddSequenceBananas(long pMonkeyNum, long[,] pLast4Seq){
            string uniqueKeyVal = $"{pLast4Seq[0,0]},{pLast4Seq[1,0]},{pLast4Seq[2,0]},{pLast4Seq[3,0]}";
            string monkeyUniqueKeyVal = $"{pMonkeyNum}: {uniqueKeyVal}";
            if(SequencePreviouslyUsed.Contains(monkeyUniqueKeyVal)){
                return; //Only first time finding sequence matters
            }

            if(SequenceBananaTotal.ContainsKey(uniqueKeyVal)){
                SequenceBananaTotal[uniqueKeyVal] = SequenceBananaTotal[uniqueKeyVal] + (pLast4Seq[3,1] % 10);
            }
            else{
                SequenceBananaTotal[uniqueKeyVal] = pLast4Seq[3,1];
            }

            SequencePreviouslyUsed.Add(monkeyUniqueKeyVal);

        }


        static long GetNextSecretNumber(long pInSecNum){

            long sn1 = pInSecNum * 64; //muliply by 64
            sn1 = pInSecNum ^ sn1; //Bitwise XOR - MIX
            sn1 = sn1 % 16777216; //mod 16777216 - PRUNE

            long sn2 = sn1 / 32; //divide by 32
            sn2 = sn2 ^ sn1; //Bitwise XOR - MIX
            sn2 = sn2 % 16777216; //mod 16777216 - PRUNE

            long sn3 = sn2 * 2048; //multiply 2048
            sn3 = sn3 ^ sn2; //Bitwise XOR - MIX
            sn3 = sn3 % 16777216; //mod 16777216 - PRUNE

            return sn3;

            //1000000                   - 64 (2^6)
            //100000                    - 32 (2^5)
            //100000000000              - 2048 (2^11)
            //1000000000000000000000000 - 16777216 (2^24)


            //                 1111011 - 123
            //           1111011000000 - 123*64 = 7872 (left shift 6)
            //           1111010111011 - bitwise xor = 7867
            //           1111010111011 - mod ^25 = 7867

            //                11110101 - 7867/32=245 (right shift 5)
            //           1111001001110 - bitwise xor = 7758
            //           1111001001110 - mod ^25 = 7758

            //111100100111000000000000 - 7758*2048 = 15888384 (left shift 11)
            //111100100110111001001110 - bitwise xor = 15887950
            //111100100110111001001110 - mod ^25 = 15887950

        }


        static long[] GetFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string fileName = "PuzzleInput1.txt";

            string[] lines = File.ReadAllLines(string.Concat(fileDataDir, "/", fileName));
            long[] longValues = new long[lines.Length];

            for(int l = 0; l<lines.Length; l++){
                longValues[l] = long.Parse(lines[l]);
            }

            return longValues;
        }

    }
}
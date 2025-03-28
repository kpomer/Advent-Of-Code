using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace AdventOfCode
{
    public class Program

    {

        static Dictionary<long,long> ValueSets = new Dictionary<long, long>(); //{value, count}

        static void Main(string[] args)
        {


            GetFileData(); //Set Dictionary values from PuzzleInput1.txt

            long stoneCount = 0;
            foreach(long key in ValueSets.Keys){
                stoneCount += ValueSets[key];
            }

            //Blinks
            for(int b = 0; b < 75; b++){
                ValueSets = blinkChange(ValueSets);
            }

            //Print Final Count
            stoneCount = 0;
            foreach(long key in ValueSets.Keys){
                stoneCount += ValueSets[key];
            }
            Console.WriteLine($"End Count: {stoneCount}");

        }

        static Dictionary<long,long> blinkChange(Dictionary<long,long> pIncomingValues){
            Dictionary<long,long> outgoingSet = new Dictionary<long, long>();

            foreach(long key in ValueSets.Keys){
                if(key == 0){
                    //0 values turn into 1
                    AddDictEntry(ref outgoingSet, 1, pIncomingValues[key]);
                }
                else if(key.ToString().Length % 2 == 0){
                    //split into 2
                    char[] numChars = key.ToString().ToCharArray();
                    string ns1 = "";
                    string ns2 = "";

                    for(int i = 0; i < numChars.Length/2; i++){
                        ns1 = string.Concat(ns1, numChars[i]);
                    }

                    for(int i = numChars.Length/2; i < numChars.Length; i++){
                        ns2 = string.Concat(ns2, numChars[i]);
                    }

                    AddDictEntry(ref outgoingSet, long.Parse(ns1), pIncomingValues[key]);
                    AddDictEntry(ref outgoingSet, long.Parse(ns2), pIncomingValues[key]);
                }
                else{
                    AddDictEntry(ref outgoingSet, key*2024, pIncomingValues[key]);
                }

            }

            return outgoingSet;


        }


        static void AddDictEntry(ref Dictionary<long,long> pDict, long pKey, long pVal){

                if(pDict.ContainsKey(pKey)){
                    //increment Count
                    long newVal = pDict[pKey] + pVal;
                    pDict[pKey] = newVal;
                }
                else{
                    pDict.Add(pKey, pVal);
                }
        }

        static void GetFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string fileName = "PuzzleInput1.txt";

            string textInput = File.ReadAllText(string.Concat(fileDataDir, "/", fileName));

            foreach(string val in textInput.Split(" ")){
                long valNum = long.Parse(val);

                AddDictEntry(ref ValueSets, valNum, 1);
            }

        }

    }
}
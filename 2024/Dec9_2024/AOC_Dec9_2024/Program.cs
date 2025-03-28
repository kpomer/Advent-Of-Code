using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace AdventOfCode
{
    public class Program
    {
        static char[] FileDataChars = new char[0];
        static List<int> FileBlocks = new List<int>();
        static List<int> FileFreeSpace = new List<int>();

        static void Main(string[] args)
        {

            FileDataChars = GetFileData(); //Char Array of all characters in string from PuzzleInput1.txt

            SetFileDataLists();

            List<int> newFile = PopulateFileUncompressed();
            
            // //REMOVE
            // string debugResponse = "";
            // foreach(int v in newFile){
            //     if(v == -1){
            //         debugResponse = string.Concat(debugResponse, ".");
            //     }
            //     else{
            //         debugResponse = string.Concat(debugResponse, v.ToString());
            //     }
            // }
            // Console.WriteLine(debugResponse);
            // //REMOVE

            //CompressFile(ref newFile);
            CompressFileByBlock(ref newFile);
            long checkSum = GetCheckSum(newFile);

            Console.WriteLine($"CheckSum: {checkSum}");
           
        }

        public static long GetCheckSum(List<int> pCompressedFile){
            long checkSum = 0;

            for(int i = 0; i < pCompressedFile.Count(); i++){
                if(pCompressedFile[i] != -1){
                    //reached spaces
                    checkSum += i*pCompressedFile[i];
                }
                
            }

            return checkSum;
        }

        public static void CompressFile(ref List<int> pNewFile){

            while(FirstFreeSpace(pNewFile) < LastBlock(pNewFile)){
                pNewFile[FirstFreeSpace(pNewFile)] = pNewFile[LastBlock(pNewFile)];
                pNewFile[LastBlock(pNewFile)] = -1;
                
                // //REMOVE
                // string debugResponse = "";
                // foreach(int v in pNewFile){
                //     if(v == -1){
                //         debugResponse = string.Concat(debugResponse, ".");
                //     }
                //     else{
                //         debugResponse = string.Concat(debugResponse, v.ToString());
                //     }
                // }
                // Console.WriteLine(debugResponse);
                // //REMOVE
            }
        }

        public static void CompressFileByBlock(ref List<int> pNewFile){
            int blockSize = 0;
            for(int b = FileBlocks.Count() - 1; b >= 0; b--){
                blockSize = FileBlocks[b];
                int indexBlockStart = IndexBlockStart(pNewFile, b);
                int indexFirstSpaceFit = FirstSpaceFitIndex(pNewFile, indexBlockStart, blockSize);

                //Replace spaces with block
                int s = indexFirstSpaceFit;
                if(s != -1){
                    for(int c = 0; c < blockSize; c++){
                        pNewFile[s] = b;
                        s += 1;
                    }


                    //Replace block with spaces
                    s = indexBlockStart;
                    for(int c = 0; c < blockSize; c++){
                        pNewFile[s] = -1;
                        s += 1;
                    }
                }


                // //REMOVE
                // string debugResponse = "";
                // foreach(int v in pNewFile){
                //     if(v == -1){
                //         debugResponse = string.Concat(debugResponse, ".");
                //     }
                //     else{
                //         debugResponse = string.Concat(debugResponse, v.ToString());
                //     }
                // }
                // Console.WriteLine(debugResponse);
                // //REMOVE
                
            }

        }

        public static int FirstFreeSpace(List<int> pFileIntArray){
            
            for(int f = 0; f < pFileIntArray.Count; f++){
                if(pFileIntArray[f] == -1){
                    return f;
                }
            }

            return -1;
        }

        public static int LastBlock(List<int> pFileIntArray){
            
            for(int b = pFileIntArray.Count - 1; b >= 0; b--){
                if(pFileIntArray[b] != -1){
                    return b;
                }
            }

            return -1;
        }

        public static int FirstSpaceFitIndex(List<int> pNewFile, int pIndexBlockStart, long pBlockSize){
            
            int index = 0;
            int spaceSize = 0;

            while (index < pIndexBlockStart){
                if(pNewFile[index] == -1){
                    //continued space
                    spaceSize += 1;
                }
                else{
                    spaceSize = 0;
                }

                if(spaceSize == pBlockSize){
                    //space is big enough
                    return (index-spaceSize+1);
                }

                index += 1;
            }

            return -1; //No fitting space found
        }

        public static int IndexBlockStart(List<int> pNewFile, int pBlockVal){

            for(int i = 0; i < pNewFile.Count(); i++){
                if(pNewFile[i] == pBlockVal){
                    return i; //index where blockVal starts
                }
            }

            return -1; //no matching block val
        }


        public static List<int> PopulateFileUncompressed(){
            List<int> uncompressedFile = new List<int>();

            for(int i = 0; i < FileBlocks.Count(); i++){
                //Add Blocks
                for(int b = 0; b < FileBlocks[i]; b++){
                    uncompressedFile.Add(i);
                }

                //Add Spaces
                if(FileFreeSpace.Count() > i){
                    for(int s = 0; s < FileFreeSpace[i]; s++){
                        uncompressedFile.Add(-1);
                    }
                }
            }

            return uncompressedFile;
        }

        public static void SetFileDataLists(){

            FileBlocks = new List<int>();
            FileFreeSpace = new List<int>();

            for(int c = 0; c < FileDataChars.Length; c++){
                int value = int.Parse(FileDataChars[c].ToString());
                switch (c % 2){
                        case 0: //even
                            FileBlocks.Add(value);
                            break;
                        case 1: //odd
                            FileFreeSpace.Add(value);
                            break;
                        default:
                            throw new Exception($"Invalid value: {(c % 2)}");
                    }
            }
        }


        static char[] GetFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string fileName = "PuzzleInput1.txt";
            
            string fileData = File.ReadAllText(string.Concat(fileDataDir, "/", fileName));
            return fileData.ToCharArray();

        }

    }
}
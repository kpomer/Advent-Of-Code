using System;
using System.Collections;
using System.Dynamic;
using System.Net.Http.Headers;
using System.Runtime.Intrinsics.X86;
using System.Xml;

namespace AdventOfCode
{
    public class Program
    {
        static List<HashSet<string>> AllMaximalCliques = new List<HashSet<string>>();
        static Dictionary<string,HashSet<string>> LinkedComputers = new Dictionary<string, HashSet<string>>(); //{Computer, Set of linked nodes}

        static void Main(string[] args)
        {

            List<string[]> networkPairs = GetFileData(); //List of String[] of all lines from PuzzleInput1.txt

            foreach(string[] pair in networkPairs){
                if(LinkedComputers.ContainsKey(pair[0])){
                    LinkedComputers[pair[0]].Add(pair[1]);
                }
                else{
                    LinkedComputers.Add(pair[0],[pair[1]]);
                }

                if(LinkedComputers.ContainsKey(pair[1])){
                    LinkedComputers[pair[1]].Add(pair[0]);
                }
                else{
                    LinkedComputers.Add(pair[1],[pair[0]]);
                }
            }



            // //Part 1
            HashSet<string> Set3Group = new HashSet<string>();
            foreach(string val1 in LinkedComputers.Keys){
                if(val1[0] == 't'){
                    foreach(string val2 in LinkedComputers[val1]){
                        foreach(string val3 in LinkedComputers[val2]){
                            if(LinkedComputers[val1].Contains(val3)){
                                List<string> set3List = [val1, val2, val3];
                                //sort and add to HashSet
                                set3List.Sort();
                                string setString = $"{set3List[0]},{set3List[1]},{set3List[2]}";
                                if(Set3Group.Contains(setString) == false){
                                    Set3Group.Add(setString);
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine($"Part 1: {Set3Group.Count()}");


            // //Part 2 - Bron-Kerbosch

            // R (Reported Clique): The set of vertices that currently form a clique.
            // P (Potential Vertices): The set of vertices that could potentially be added to R to extend the clique.
            // X (Excluded Vertices): The set of vertices that cannot be added to R because they would break the clique property.


            HashSet<string> potential = [.. LinkedComputers.Keys];
            Bron_Kerbosch(potential, new HashSet<string>(), new HashSet<string>());

            //Get Max Clique
            AllMaximalCliques.Sort((y, x) => x.Count.CompareTo(y.Count)); //sort descending length
            List<string> maxClique = AllMaximalCliques[0].ToList<string>();

            maxClique.Sort((x, y) => x.CompareTo(y));
            string password = "";
            foreach(string c in maxClique){
                if(password != ""){
                    password = password + ",";
                }

                password = password + c;
            }

            Console.WriteLine($"Part 2: {password}");

        }

        static void Bron_Kerbosch(HashSet<string> p, HashSet<string> x, HashSet<string> r){
            if(p.Count == 0 && x.Count == 0){
                //Max Clique Found
                AllMaximalCliques.Add(r);
                return;
            }

            foreach(string v in p.ToList<string>()){
                HashSet<string> pNew = new HashSet<string>(p);
                pNew.IntersectWith(LinkedComputers[v]);

                HashSet<string> xNew = new HashSet<string>(x);
                xNew.IntersectWith(LinkedComputers[v]);

                HashSet<string> rNew = [.. r, v];
                Bron_Kerbosch(pNew, xNew, rNew); 
                p.Remove(v);
                x.Add(v);

            // var pNew = p.Intersect(Graph[v]).ToHashSet();
            // var xNew = x.Intersect(Graph[v]).ToHashSet();
            // var rNew = new HashSet<int>(r); 

        





                // HashSet<string> pClique_2 = new HashSet<string>(pClique);
                // pClique_2.UnionWith([p]);
                // HashSet<string> pPotential_2 = new HashSet<string>(pPotential);
                // pPotential_2.Remove(p);
                // HashSet<string> pExcluded_2 = new HashSet<string>(pExcluded);
                // pExcluded.Remove(p);

            //     Bron_Kerbosch(pClique_2, pPotential_2, pExcluded_2);

            //     pPotential.Remove(p);
            //     pExcluded_2.UnionWith([p]);
            // }


            }
        }

        static bool IsClique(HashSet<string> checkClique){

            foreach(string c in checkClique){
                foreach(string c2 in checkClique){
                    if(c != c2 && LinkedComputers[c].Contains(c2) == false){
                        return false;
                    }
                }
            }

            return true;

        }

        static List<string[]> GetFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string fileName = "PuzzleInput1.txt";

            List<string[]> pairs = new List<string[]>();
            
            string[] fileLines =  File.ReadAllLines(string.Concat(fileDataDir, "/", fileName));
            foreach(string line in fileLines){
                pairs.Add([line.Split('-')[0],line.Split('-')[1]]);
            }

            return pairs;
        }

    }
}
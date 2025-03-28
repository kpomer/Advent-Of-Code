using System;
using System.Collections;
using System.ComponentModel;
using System.Net;

namespace AdventOfCode
{
    public class Program
    {

        public static char[,] FileData = new char[0,0];
        public static HashSet<string> NoRegions = new HashSet<string>();
        public static int verticalCount = 0;
        public static int horizontalCount = 0;
        public static Dictionary<long[],long[]> GardenRegions = new Dictionary<long[],long[]>(); //[startV,startH],[area,perim,sideCount]]
        //public static Dictionary<long[],List<string>> RegionEdges = new Dictionary<long[],List<string>>(); //[startV,startH],[area,perim]]

        static void Main(string[] args)
        {

            GetFileData(); //Grid values from PuzzleInput1.txt

            for(long v = 0; v < verticalCount; v++){
                for(long h = 0; h < horizontalCount; h++){
                    string point = string.Concat($"[{v},{h}]");
                    if(NoRegions.Contains(point)){
                        long regionArea = 0;
                        long regionPerimeter = 0;
                        List<string> regionEdges = new List<string>();
                        AddToRegion(v, h, ref regionArea, ref regionPerimeter, ref regionEdges);
                        
                        long numSides = GetRegionSideCount(FileData[v,h], regionEdges);
                        //RegionEdges.Add([v,h], regionEdges);
                        GardenRegions.Add([v,h], [regionArea, regionPerimeter, numSides]);
                    }
                    
                }
            }


            long price = 0;
            long discountedPrice = 0;
            foreach(long[] plot in GardenRegions.Keys){
                price += GardenRegions[plot][0] * GardenRegions[plot][1]; //Area * Perimeter
                discountedPrice += GardenRegions[plot][0] * GardenRegions[plot][2]; //Area * NumberOfSides
            }

             Console.WriteLine($"Normal Price: {price}\nDiscounted Price: {discountedPrice}");
            
        }

        static void AddToRegion(long pVert, long pHoriz, ref long pRegionArea, ref long pRegionPerimeter, ref List<string> pRegionEdges){

            NoRegions.Remove(string.Concat($"[{pVert},{pHoriz}]"));

            pRegionArea += 1; //1 per point

            //Check Adjacent Points
            long newVert, newHoriz;
        
            //check UP: (-1, 0)
            newVert = pVert - 1;
            newHoriz = pHoriz;
            if(CheckForPlotEdge(pVert, pHoriz, "UP")){
                //Fence Needed
                pRegionPerimeter += 1;
                if(pRegionEdges.Contains(String.Concat($"[{pVert},{pHoriz}]")) == false){
                    pRegionEdges.Add(String.Concat($"[{pVert},{pHoriz}]"));
                }
            }
            else{
                //Add to Region
                if(NoRegions.Contains(string.Concat($"[{newVert},{newHoriz}]"))){
                    AddToRegion(newVert, newHoriz, ref pRegionArea, ref pRegionPerimeter, ref pRegionEdges);
                }
            }

            //check RIGHT: (0, 1)
            newVert = pVert;
            newHoriz = pHoriz + 1;
            if(CheckForPlotEdge(pVert, pHoriz, "RIGHT")){
                //Fence Needed
                pRegionPerimeter += 1;
                if(pRegionEdges.Contains(String.Concat($"[{pVert},{pHoriz}]")) == false){
                    pRegionEdges.Add(String.Concat($"[{pVert},{pHoriz}]"));
                }
            }
            else{
                //Add to Region
                if(NoRegions.Contains(string.Concat($"[{newVert},{newHoriz}]"))){
                    AddToRegion(newVert, newHoriz, ref pRegionArea, ref pRegionPerimeter, ref pRegionEdges);
                }
            }

            //check DOWN: (1, 0)
            newVert = pVert + 1;
            newHoriz = pHoriz;
            if(CheckForPlotEdge(pVert, pHoriz, "DOWN")){
                //Fence Needed
                pRegionPerimeter += 1;
                if(pRegionEdges.Contains(String.Concat($"[{pVert},{pHoriz}]")) == false){
                    pRegionEdges.Add(String.Concat($"[{pVert},{pHoriz}]"));
                }
            }
            else{
                //Add to Region
                if(NoRegions.Contains(string.Concat($"[{newVert},{newHoriz}]"))){
                    AddToRegion(newVert, newHoriz, ref pRegionArea, ref pRegionPerimeter, ref pRegionEdges);
                }
            }

            //check LEFT: (0, -1)
            newVert = pVert;
            newHoriz = pHoriz - 1;
            if(CheckForPlotEdge(pVert, pHoriz, "LEFT")){
                //Fence Needed
                pRegionPerimeter += 1;
                if(pRegionEdges.Contains(String.Concat($"[{pVert},{pHoriz}]")) == false){
                    pRegionEdges.Add(String.Concat($"[{pVert},{pHoriz}]"));
                }
            }
            else{
                //Add to Region
                if(NoRegions.Contains(string.Concat($"[{newVert},{newHoriz}]"))){
                    AddToRegion(newVert, newHoriz, ref pRegionArea, ref pRegionPerimeter, ref pRegionEdges);
                }
            }

        }

        static bool CheckForPlotEdge(long pCurVert, long pCurHoriz, string pDir){
            
            long chgVert = 0;
            long chgHoriz = 0;

            switch(pDir.ToUpper()) 
                {
                case "UP":
                    chgVert = -1;
                    chgHoriz = 0;
                    break;
                case "RIGHT":
                    chgVert = 0;
                    chgHoriz = 1;
                    break;
                case "DOWN":
                    chgVert = 1;
                    chgHoriz = 0;
                    break;
                case "LEFT":
                    chgVert = 0;
                    chgHoriz = -1;
                    break;
                default:
                    throw new Exception($"Invalid Direction: {pDir}");
                }

            long newVert = pCurVert + chgVert;
            long newHoriz = pCurHoriz + chgHoriz;

            return FenceNeeded(newVert, newHoriz, FileData[pCurVert,pCurHoriz]);

        }

        static bool FenceNeeded(long pNewVert, long pNewHoriz, char pCurRegionVal){

            return (pNewVert < 0 || pNewVert >= verticalCount || pNewHoriz < 0 || pNewHoriz >= horizontalCount || FileData[pNewVert,pNewHoriz] != pCurRegionVal);

        }

        static bool sideAlreadyCounted(long pCurVert, long pCurHoriz, string pDir, List<string> pRegionEdges){

            int indexCurrent = pRegionEdges.IndexOf(String.Concat($"[{pCurVert},{pCurHoriz}]"));

            long v = pCurVert;
            long h = pCurHoriz;
            while(v >= 0 && v < verticalCount && h >= 0 && h < horizontalCount && FileData[pCurVert,pCurHoriz] == FileData[v,h] && CheckForPlotEdge(v, h, pDir)){
                
                if (pCurVert != v || pCurHoriz != h){
                    int indexOfAdj = pRegionEdges.IndexOf(String.Concat($"[{v},{h}]"));
                    if(indexOfAdj != -1 && indexOfAdj < indexCurrent){
                        return true;
                    }
                }

                switch(pDir.ToUpper()) 
                {
                case "UP":
                    h += 1;
                    break;
                case "RIGHT":
                    v += 1;
                    break;
                case "DOWN":
                    h += 1;
                    break;
                case "LEFT":
                    v += 1;
                    break;
                default:
                    throw new Exception($"Invalid Direction: {pDir}");
                }

            }

            v = pCurVert;
            h = pCurHoriz;
            while(v >= 0 && v < verticalCount && h >= 0 && h < horizontalCount && FileData[pCurVert,pCurHoriz] == FileData[v,h] && CheckForPlotEdge(v, h, pDir)){
                
                if (pCurVert != v || pCurHoriz != h){
                    int indexOfAdj = pRegionEdges.IndexOf(String.Concat($"[{v},{h}]"));
                    if(indexOfAdj != -1 && indexOfAdj < indexCurrent){
                        return true;
                    }
                }

                switch(pDir.ToUpper()) 
                {
                case "UP":
                    h -= 1;
                    break;
                case "RIGHT":
                    v -= 1;
                    break;
                case "DOWN":
                    h -= 1;
                    break;
                case "LEFT":
                    v -= 1;
                    break;
                default:
                    throw new Exception($"Invalid Direction: {pDir}");
                }

            }

            return false;
        }

        static long GetRegionSideCount(char pRegionValue, List<string> pRegionEdges){
            long sideCount = 0;
            
            //TODO Count Number of Sides
            foreach(string edgePoint in pRegionEdges){
                string[] strPointCoordinates = edgePoint.Replace("[","").Replace("]","").Split(',');
                long curVert = long.Parse(strPointCoordinates[0]);
                long curHoriz = long.Parse(strPointCoordinates[1]);

                //check UP: (-1, 0)
                if(CheckForPlotEdge(curVert, curHoriz, "UP")){
                    //Check if side has already been counted
                    if(sideAlreadyCounted(curVert, curHoriz, "UP", pRegionEdges) == false){
                        sideCount += 1;
                    }
                }


                //check RIGHT: (0, 1)
                if(CheckForPlotEdge(curVert, curHoriz, "RIGHT")){
                    //Check if side has already been counted
                    if(sideAlreadyCounted(curVert, curHoriz, "RIGHT", pRegionEdges) == false){
                        sideCount += 1;
                    }
                }


                //check DOWN: (1, 0)
                if(CheckForPlotEdge(curVert, curHoriz, "DOWN")){
                    //Check if side has already been counted
                    if(sideAlreadyCounted(curVert, curHoriz, "DOWN", pRegionEdges) == false){
                        sideCount += 1;
                    }
                }


                //check LEFT: (0, -1)
                if(CheckForPlotEdge(curVert, curHoriz, "LEFT")){
                    //Check if side has already been counted
                    if(sideAlreadyCounted(curVert, curHoriz, "LEFT", pRegionEdges) == false){
                        sideCount += 1;
                    }
                }
            
            }

            return sideCount; 

        }


        static void GetFileData(){
            //Retrieve All Text from Puzzle Input file
            string fileDataDir = Directory.GetCurrentDirectory().ToString();
            string fileName = "PuzzleInput1.txt";

            string[] fileStringArr = File.ReadAllLines(string.Concat(fileDataDir, "/", fileName)); 

            verticalCount = fileStringArr.Count<string>();
            horizontalCount = fileStringArr[0].ToString().Length;

            FileData = new char[verticalCount,horizontalCount];

            for(int v = 0; v < verticalCount; v++){
                string line = fileStringArr[v].ToString();
                char[] lineStrArr = line.ToCharArray();

                for(int h = 0; h < line.Length; h++){
                    FileData[v,h] = lineStrArr[h];
                    NoRegions.Add(string.Concat($"[{v},{h}]"));
                }
            }

        }

    }
}
# main.py
import sys
import os
import math

# region Access Shared Functions
# Get the absolute path to the directory containing the current script (main.py)
current_dir = os.path.dirname(os.path.abspath(__file__))
parent_dir = os.path.dirname(os.path.dirname(current_dir))
if parent_dir not in sys.path:
    sys.path.append(parent_dir)
# endregion
import util.sharedFunctions as shr #import shared functions from util folder

# Global Variables
Current_Dir = os.path.dirname(__file__) #directory of current folder

CostMatrix = dict()
Nodes = set()
NodeToNum = dict()
DP = dict()


def main():
    fileSA = shr.fileAsStringArray(Current_Dir)
    initializeData(fileSA)

    N = len(Nodes)
    minCost_ForAll = float('inf')

    for startNode in range(N):
        DP = dict()

        # Find distances from 0 to each other node
        for j in range(0,N):
            if (j == startNode):
                continue
            mask = 1 << startNode | 1 << j
            # print(str(mask) + ": " + formatBin(mask, N))
            if(mask not in DP):
                DP[mask] = dict()

            DP[mask][j] = getCost(startNode, j)

        
        for subset_size in range(3, N + 1):
            # print("\n\n")
            # print("Subset Size: " + str(subset_size))
            for mask in range(1, 1 << N):
                # print(str(mask) + ": " + formatBin(mask, N))
                if bin(mask).count('1') == subset_size and ((mask >> startNode) & 1):
                    # print("\n")
                    for j in range(N):
                        # print("j: " + str(j) + " - " + formatBin(j, N))
                        if j == startNode or not ((mask >> j) & 1):
                            continue
                    
                        prev_mask = mask ^ (1 << j)
                        # print(str(prev_mask) + ": " + formatBin(prev_mask, N))

                        minCostTo_j = float('inf') #start at infinite
                        for i in range(N):
                            if i != j and i != startNode and ((prev_mask >> i) & 1):
                                if prev_mask in DP and i in DP[prev_mask]:
                                    cost_i_to_j = DP[prev_mask][i] + getCost(i,j)
                                    minCostTo_j = min(minCostTo_j, cost_i_to_j)
                                else:
                                    Exception("Error!")
                        if mask not in DP:
                            DP[mask] = {}
                        DP[mask][j] = minCostTo_j

        final_mask = (1 << N) - 1
        for p in DP[final_mask]:
            minCost_ForAll = min(minCost_ForAll, DP[final_mask][p])

    print("Part 1: " + str(minCost_ForAll))
    





def initializeData(fileSA):
    for r in fileSA:
        r = str.replace(r, "\n", "")
        
        r_Split = r.split(" ")
        for n in [r_Split[0], r_Split[2]]:
           if(n not in Nodes):
              NodeToNum[n] = len(Nodes)
              Nodes.add(n)
        setCost(NodeToNum[r_Split[0]], NodeToNum[r_Split[2]], r_Split[4])

        

        
   

def setCost(node1, node2, cost):
   setMask = 1 << node1 | 1 << node2
   CostMatrix[setMask] = int(cost)

def getCost(node1, node2):
   setMask = 1 << node1 | 1 << node2
   return CostMatrix[setMask]

def formatBin(val, length):
    bVal = bin(val)
    bVal = bVal.replace("0b", "")
    while(len(bVal) < length):
        bVal = "0" + bVal
    return bVal

main()
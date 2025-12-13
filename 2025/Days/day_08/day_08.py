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


def main():
    fileSA = shr.fileAsStringArray(Current_Dir)
    junctionCoordinates = {}
    junctionBoxNumber = 0
    circuits = {} # circuitID: set(jbNumbers)
    for coordinate in fileSA:
        coordinate = coordinate.replace("\n", "").split(",")
        junctionCoordinates[junctionBoxNumber] = (int(coordinate[0]), int(coordinate[1]), int(coordinate[2]))
        s = set()
        s.add(junctionBoxNumber)
        circuits[junctionBoxNumber] = s
        junctionBoxNumber += 1

    distances = [] # [dist, p, q]
    for jb in junctionCoordinates:
        for jb2 in junctionCoordinates:
            if jb < jb2: # using < means we will never get duplicates like storing both (p,q) and (q,p).  It also stops us from storing (p,p)
                distances.append([euclideanDistance(junctionCoordinates[jb],junctionCoordinates[jb2]), jb, jb2])
    distances.sort(key=sortDistances)

    for jbConnect in range(1000):
        c1 = -1
        c2 = -1
        for c in circuits:
            if distances[jbConnect][1] in circuits[c]:
                c1 = c
            if distances[jbConnect][2] in circuits[c]:
                c2 = c
        if c1 != c2:
            circuits[c1] = circuits[c1].union(circuits[c2])
            circuits.pop(c2)

    circuitLengths = []
    for c in circuits:
        circuitLengths.append(len(circuits[c]))

    circuitLengths.sort(reverse = True)

    product = 1
    for i in range(3):
        product *= circuitLengths[i]
    
    print(f"Part 1: {product}")

def euclideanDistance(p, q):
    # Euclidean distance between 2 points in 3D space using (x,y,z) coordinates
    return math.sqrt(pow(p[0] - q[0], 2) + pow(p[1] - q[1], 2) + pow(p[2] - q[2], 2))

def sortDistances(val):
    # distance is at index 0
    return val[0]
    

main()
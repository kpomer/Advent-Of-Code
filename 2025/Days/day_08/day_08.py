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
    fileSA = shr.fileAsStringArray(Current_Dir, "e")

    junctionCoordinates = {}
    junctionBoxNumber = 0
    for coordinate in fileSA:
        coordinate = coordinate.replace("\n", "").split(",")
        junctionCoordinates[junctionBoxNumber] = (int(coordinate[0]), int(coordinate[1]), int(coordinate[2]))
        junctionBoxNumber += 1

    distances = [] # [dist, p, q]
    for jb in junctionCoordinates:
        for jb2 in junctionCoordinates:
            if jb < jb2: # using < means we will never get duplicates like storing both (p,q) and (q,p).  It also stops us from storing (p,p)
                distances.append([euclideanDistance(junctionCoordinates[jb],junctionCoordinates[jb2]), jb, jb2])
    distances.sort(key=sortDistances)

def euclideanDistance(p, q):
    # Euclidean distance between 2 points in 3D space using (x,y,z) coordinates
    return math.sqrt(pow(p[0] - q[0], 2) + pow(p[1] - q[1], 2) + pow(p[2] - q[2], 2))

def sortDistances(val):
    # distance is at index 0
    return val[0]
    

main()
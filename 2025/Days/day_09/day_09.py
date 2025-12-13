# main.py
import sys
import os

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
    redCoordinates = []

    for line in fileSA:
        line = line.replace("\n","").split(",")
        redCoordinates.append((int(line[0]),int(line[1])))
    
    # Part 1 - Find Max Area between two red corners
    maxArea = 0
    for r1 in redCoordinates:
        for r2 in redCoordinates:
            area = (max(r1[0], r2[0]) - min(r1[0], r2[0])+1) * (max(r1[1], r2[1]) - min(r1[1], r2[1])+1)
            maxArea = max(maxArea, area)
    print(f"Part 1: {maxArea}")


main()
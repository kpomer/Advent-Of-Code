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

FreshRanges = list()
AvailableIDs = list()

def main():
    fileSA = shr.fileAsStringArray(Current_Dir)
    row = 0
    
    # Fresh Ranges
    while fileSA[row] != "\n":
        line = fileSA[row].replace("\n","")
        lineSplit = line.split("-")
        FreshRanges.append([int(lineSplit[0]),int(lineSplit[1])])
        row += 1

    # Available IDs
    for r in range(row+1, len(fileSA)):
        line = fileSA[r].replace("\n","")
        AvailableIDs.append(int(line))

    # Cleanup Range Data
    simplifyRanges()

    idCount = 0
    for id in AvailableIDs:
        if idIsFresh(id):
            idCount += 1

    print(f"Part 1: {idCount}")



def simplifyRanges():
    # Sort
    FreshRanges.sort(key=sortRanges)

    r = 0
    invalid = [-1,-1]
    while r < len(FreshRanges) - 1:

        if FreshRanges[r] == invalid:
            x = 0 # do nothing and continue
        elif FreshRanges[r] == FreshRanges[r+1]:
            FreshRanges[r+1] = invalid # duplicate - remove
        elif FreshRanges[r+1][0] <= FreshRanges[r][1] and FreshRanges[r+1][1] <= FreshRanges[r][1]:
            FreshRanges[r+1] = invalid # fully contained - remove
        elif FreshRanges[r+1][0] <= FreshRanges[r][1] and FreshRanges[r+1][1] > FreshRanges[r][1]:
            FreshRanges[r][1] = FreshRanges[r+1][1] # extends current range
            FreshRanges[r+1] = invalid # accounted for - remove

        r += 1

    # Remove Invalid Ranges
    for checkRange in FreshRanges:
        if checkRange == invalid:
            FreshRanges.remove(checkRange)


def idIsFresh(id):
    for range in FreshRanges:
        if id < range[0]:
            return False
        elif id >= range[0] and id <= range[1]:
            return True
    
    return False


def sortRanges(val):
    return val[0]
        

main()
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
SplitterCoordinates = []
FileGrid = {}
MaxWidth = 0
MaxHeight = 0
UsedSplitterSet = set()

def main():
    global FileGrid, MaxWidth, MaxHeight # Modifying Value of Global Variables
    FileGrid, MaxWidth, MaxHeight = shr.fileAsGrid(Current_Dir)

    startCoordinate = (-1,-1)
    for coordinate in FileGrid:
        if FileGrid[coordinate] == "^":
            SplitterCoordinates.append(coordinate)
        elif FileGrid[coordinate] == "S":
            startCoordinate = coordinate

    # Part 1 - recurisve loop from incoming beam source 'S'
    findSplits(startCoordinate)
    print(f"Part 1: {len(UsedSplitterSet)}")





def findSplits(coordinate):

    if shr.validCoordinate(coordinate, MaxWidth, MaxHeight) == False:
        # Bottom/Side Reached - return
        return
    elif FileGrid[coordinate] == "^":
        # Splitter Found!  Add to set and continue with split beam
        UsedSplitterSet.add(coordinate)
        leftCoordinate = shr.moveCoordinate(coordinate, "<")
        rightCoordinate = shr.moveCoordinate(coordinate, ">")
        findSplits(leftCoordinate)
        findSplits(rightCoordinate)
    elif FileGrid[coordinate] == "." or FileGrid[coordinate] == "S":
        # Empty Space - continue moving downward
        downCoordinate = shr.moveCoordinate(coordinate, "v")
        findSplits(downCoordinate)
    else:
        raise Exception(f"Invalid Value at coordinate {coordinate}: {FileGrid[coordinate]}")


main()
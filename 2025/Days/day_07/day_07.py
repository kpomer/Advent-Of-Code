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
FindSplits_cache = set()
FindPaths_cache = {}

def main():
    global FileGrid, MaxWidth, MaxHeight # Modifying Value of Global Variables
    FileGrid, MaxWidth, MaxHeight = shr.fileAsGrid(Current_Dir)

    startCoordinate = (-1,-1)
    for coordinate in FileGrid:
        if FileGrid[coordinate] == "^":
            SplitterCoordinates.append(coordinate)
        elif FileGrid[coordinate] == "S":
            startCoordinate = coordinate

    # Part 1 - recursive loop from incoming beam source 'S'
    findSplits(startCoordinate)
    print(f"Part 1: {len(UsedSplitterSet)}")

    # Part 2 - recursive loop for ALL paths from incoming beam source 'S'
    pathCount = findPathCount(startCoordinate, 0)
    print(f"Part 2: {pathCount}")


def findSplits(coordinate):

    if coordinate in FindSplits_cache or shr.validCoordinate(coordinate, MaxWidth, MaxHeight) == False:
        # Cached value or Bottom Reached - return
        return
    elif FileGrid[coordinate] == "^":
        # Splitter Found!  Add to set and continue with split beam
        UsedSplitterSet.add(coordinate)
        leftCoordinate = shr.moveCoordinate(coordinate, "<")
        rightCoordinate = shr.moveCoordinate(coordinate, ">")
        findSplits(leftCoordinate)
        findSplits(rightCoordinate)
    elif FileGrid[coordinate] == "." or FileGrid[coordinate] == "S":
        # Start or Empty Space - continue moving downward
        downCoordinate = shr.moveCoordinate(coordinate, "v")
        findSplits(downCoordinate)
    else:
        raise Exception(f"Invalid Value at coordinate {coordinate}: {FileGrid[coordinate]}")

    FindSplits_cache.add(coordinate)


def findPathCount(coordinate, pathsFromStart):

    # Number of paths to the end from the current coordinate
    # This value starts as the number of paths TO the current coordinate from the start 
    pathsFromCurrent = pathsFromStart

    if coordinate in FindPaths_cache:
        # PathsFromCurrent has already been calculated - Return value from Cache
        return FindPaths_cache[coordinate]
    elif shr.validCoordinate(coordinate, MaxWidth, MaxHeight) == False:
        # Bottom Reached - add 1 path
        pathsFromCurrent += 1
    elif FileGrid[coordinate] == "^":
        # Splitter Found!  Add paths from right and left coordinate
        leftCoordinate = shr.moveCoordinate(coordinate, "<")
        rightCoordinate = shr.moveCoordinate(coordinate, ">")
        pathsFromCurrent += findPathCount(leftCoordinate, pathsFromStart)
        pathsFromCurrent += findPathCount(rightCoordinate, pathsFromStart)
    elif FileGrid[coordinate] == "." or FileGrid[coordinate] == "S":
        # Start or Empty Space - add paths from down coordinate
        downCoordinate = shr.moveCoordinate(coordinate, "v")
        pathsFromCurrent += findPathCount(downCoordinate, pathsFromStart)
    else:
        raise Exception(f"Invalid Value at coordinate {coordinate}: {FileGrid[coordinate]}")
    
    # Save calculated pathsFromCurrent coordinate into cache and return
    FindPaths_cache[coordinate] = pathsFromCurrent
    return pathsFromCurrent

main()
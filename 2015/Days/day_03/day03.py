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
    fileSA = shr.fileAsStringArray(Current_Dir)[0]
    # print(fileSA)

    ## PART 1
    location = (0,0)
    housesVisited = {location}

    for d in fileSA:
        location = shr.moveCoordinate(location, d)
        housesVisited.add(location)

    print(f"Part 1: {len(housesVisited)}")


    ## PART 2
    locationSanta = (0,0)
    locationRobo = (0,0)
    housesVisited = {locationSanta}

    for c in range(len(fileSA)):
        if (c%2 == 0):
            locationSanta = shr.moveCoordinate(locationSanta, fileSA[c])
            housesVisited.add(locationSanta)
        elif (c%2 == 1):
            locationRobo = shr.moveCoordinate(locationRobo, fileSA[c])
            housesVisited.add(locationRobo)

    print(f"Part 2: {len(housesVisited)}")



main()
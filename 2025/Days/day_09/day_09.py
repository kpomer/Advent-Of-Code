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
    fileSA = shr.fileAsStringArray(Current_Dir, "e")
    redCoordinates = []
    borderCoordinates = set()

    for line in fileSA:
        # Store red coordinates
        line = line.replace("\n","").split(",")
        redCoordinates.append((int(line[0]),int(line[1])))

    # Store Border Coordinates
    redIndex = -1 # Start at -1 to link last to first
    while redIndex < len(redCoordinates) - 1:
        r1 = redCoordinates[redIndex]
        r2 = redCoordinates[redIndex+1]

        if r1[0] == r2[0]:
            y = max(r1[1], r2[1])
            yMin = min(r1[1], r2[1])
            while y >= yMin:
                borderCoordinates.add((r1[0],y))
                y-=1
        elif r1[1] == r2[1]:
            x = max(r1[0], r2[0])
            xMin = min(r1[0], r2[0])
            while x >= xMin:
                borderCoordinates.add((x,r1[1]))
                x-=1
        else:
            raise Exception(f"Invalid Entry at index {redIndex}")
        redIndex += 1
    
    
    # TODO repeat part 1 logic, but if area > maxArea, make sure all 


    # Get All UNIQUE Rectangles
    for r1 in redCoordinates:
        for r2 in redCoordinates:
            r3 = [r1[0], r2[1]]
            r4 = [r2[0], r1[1]]
            # r1/r2/r3/r4 are ALL coordinates

            # TODO Sort (top left, top right, bottom left, bottom right)
            # TODO Calculate area
            # TODO Store in array that will later be sorted by area [topLeft, topRight, bottomLeft, bottomRight, area]
    # Sort array by area desc - part 1 is array[0]
    # Part2 - loop through array until you find the FIRST value where the all coordinates are within borders

    





    
    # Part 1 - Find Max Area between two red corners
    maxArea = 0
    for r1 in redCoordinates:
        for r2 in redCoordinates:
            area = (max(r1[0], r2[0]) - min(r1[0], r2[0])+1) * (max(r1[1], r2[1]) - min(r1[1], r2[1])+1)
            maxArea = max(maxArea, area)
    print(f"Part 1: {maxArea}")

    # Part 2
    ## Write a function to check if a coordinate is red/green vs other
    ## Do same logic as part 1, but make sure the OTHER two corners are either red/green
    ## If two corners are red and the other corners are either red/green, it should be a valid rectangle
    ## THIS MAY NOT WORK - there could be a chunk taken out of one side... Maybe check every BORDER tile


main()
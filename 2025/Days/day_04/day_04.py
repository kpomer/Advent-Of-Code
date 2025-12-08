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
    fileGrid, width, height = shr.fileAsGrid(Current_Dir)
    
    part1_count = 0
    for coordinate in fileGrid:
        if fileGrid[coordinate] == "@":
            adj_rolls = 0
            for dir in ["^", ">", "v", "<", "^>", "^<", "v>", "v<"]:
                newCoordinate = shr.moveCoordinate(coordinate, dir)
                if shr.validCoordinate(newCoordinate, width, height) and fileGrid[newCoordinate] == "@":
                    adj_rolls += 1

            if adj_rolls < 4:
                part1_count += 1

    print(f"Part 1: {part1_count}")

main()
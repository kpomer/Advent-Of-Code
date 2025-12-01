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

    val = 50
    password1 = 0
    for i in fileSA:
        instruction = i.replace("\n", "")
        dir = instruction[0]
        dist = 0
        if(dir == "R"):
            dist = int(instruction[1:])
        elif(dir == "L"):
            dist = int(instruction[1:])*-1
        
        val+=dist

        # Normalize 0-99
        val = val % 100

        if val == 0:
            password1 += 1

    print(f"Part 1: {password1}")

main()
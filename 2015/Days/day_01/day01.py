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

    counter = 0
    floor = 0
    pos = 1
    for i in fileSA:
        if(i == '('):
           floor += 1
        elif(i == ')'):
           floor -= 1
        counter += 1
        if(pos == 1 and floor < 0):
            pos = counter

    print(f"Part 1: {floor}")
    print(f"Part 2: {pos}")

main()
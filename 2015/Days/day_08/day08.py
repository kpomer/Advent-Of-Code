# main.py
import sys
import os
import re

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


    literalLength = 0
    evalLength = 0
    encodedLength = 0

    for i in fileSA:
        i = str.replace(i, "\n", "")
        encodedStr = "\"" + str.replace(str.replace(i, "\\", "\\\\"), "\"", "\\\"") + "\"" #used in Part2

        literalLength = literalLength + len(i)
        evalLength = evalLength + len(eval(i))
        encodedLength = encodedLength + len(encodedStr)


    print(f"Part 1: {literalLength - evalLength}")
    print(f"Part 2: {encodedLength - literalLength}")

main()
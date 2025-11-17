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
    fileInput = shr.fileAsStringArray(Current_Dir)[0].strip("\n")
    
    re_Pattern = "-?[0-9]+"
    re_Matches = re.findall(re_Pattern, fileInput)

    sum = 0
    for r in re_Matches:
        sum += int(r)

    print("Part 1: " + str(sum))
    
main()
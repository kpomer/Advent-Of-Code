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

#import shared functions from util folder
import util.sharedFunctions as shr

# Example usage:
# print(shr.fileAsStringArray("e"))

def fileAsStringArray():
    # fileName = "PuzzleInput.txt"
    fileName = "2015/Days/day_01/PuzzleInput_example.txt"
   
    with open(os.path.join(os.path.curdir, fileName), 'r') as f:
        data = f.readlines()
        print(data)

        print(os.path.abspath(__file__))


fileAsStringArray()
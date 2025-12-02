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
    fileSA = shr.fileAsStringArray(Current_Dir)[0].split(",")

    part1_sum = 0
    for r in fileSA:
        range = r.split("-")
        start = int(range[0])
        end = (range[1])

        c = int(start)
        while c <= int(end):
            idLength = len(str(c))
            if idLength % 2 == 0:
                splitLength = int(idLength/2)
                c_Str = str(c)
                if c_Str[:splitLength] == c_Str[splitLength:]:
                    part1_sum += c
            
            c+= 1

    print(f"Part 1: {part1_sum}")

        # idLength = len(start)
        # while idLength <= len(end):
        #     if idLength % 2 == 0:
        #         # Even number of digits
        #         id_1 = start
            
        #     idLength += 1

main()
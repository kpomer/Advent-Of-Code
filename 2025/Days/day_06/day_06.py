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
    
    mathProblems = [] # [num1, num2, ..., numN, symbol]
    row = 0

    while row < 4:
        rowVal = fileSA[row].replace("\n","").split()
        col = 0
        while col < len(rowVal):
            if row == 0:
                mathProblems.append([int(rowVal[col]),0,0,""])
            elif row == 1 or row == 2:
                mathProblems[col][row] = int(rowVal[col])
            elif row == 3:
                mathProblems[col][row] = rowVal[col]
            col += 1
        row += 1

    part1_sum = 0
    for problem in mathProblems:
        if problem[3] == "+":
            part1_sum += (problem[0] + problem[1] + problem[2])
        elif problem[3] == "*":
            part1_sum += (problem[0] * problem[1] * problem[2])

    print(f"Part 1: {part1_sum}")


main()
# main.py
import sys
import os
import math

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
    
    mathProblems = [] # [num1, num2, ..., numN, symbol]
    problemElements = len(fileSA) - 1
    
    row = 0
    while row <= problemElements:
        rowVal = fileSA[row].replace("\n","").split()
        col = 0
        while col < len(rowVal):
            if row == 0:
                mathProblems.append([0] * (problemElements+1))
            val = rowVal[col]
            if val.isnumeric():
                mathProblems[col][row] = int(val)
            else:
                mathProblems[col][row] = val
            col += 1
        row += 1

    part1_sum = 0
    for problem in mathProblems:
        if problem[problemElements] == "+": 
            problem.pop(problemElements)   
            part1_sum += sum(problem)
        elif problem[problemElements] == "*":
            problem.pop(problemElements)   
            part1_sum += math.prod(problem)

    print(f"Part 1: {part1_sum}")


main()
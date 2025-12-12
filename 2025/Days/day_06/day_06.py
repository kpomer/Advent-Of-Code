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

    fileSA = shr.fileAsStringArray(Current_Dir) # Set "e" for Example
    fileGrid, fileWidth, fileHeight = shr.fileAsGrid(Current_Dir) # Set "e" for Example

    part1_result = part1(fileSA)
    part2_result = part2(fileGrid, fileWidth, fileHeight)

    print(f"Part 1: {part1_result}")
    print(f"Part 2: {part2_result}")


def part1(fileSA):
    
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

    return part1_sum


def part2(fileGrid, fileWidth, fileHeight):

    mathProblems = [] # [num1, num2, ..., numN, symbol]

    w = fileWidth - 1  
    problem = []  
    while w >= 0:
        h = 0
        val = ""
        while h < fileHeight:
            if fileGrid[w,h] != " ":
                val += fileGrid[w,h]
            h += 1
        if val.isnumeric():
            problem.append(int(val))
            w -= 1
        elif val[:-1].isnumeric():
            problem.append(int(val[:-1]))
            problem.append(val[-1:])
            mathProblems.append(problem)
            problem = []
            w -= 2
        else:
            raise Exception(f"Invalid Value at [{w},{h}]")


    part2_sum = 0
    for problem in mathProblems:
        if problem[len(problem)-1] == "+": 
            problem.pop(len(problem)-1)   
            part2_sum += sum(problem)
        elif problem[len(problem)-1] == "*": 
            problem.pop(len(problem)-1)   
            part2_sum += math.prod(problem)
        else:
            raise Exception(f"Invalid Problem: {problem}")

    return part2_sum
    

main()
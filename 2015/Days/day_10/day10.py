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
    inputValue = shr.fileAsStringArray(Current_Dir)[0]
    processedValue = int(inputValue)
    for i in range(40): # change to 40
        processedValue = lookAndSayConvert(str(processedValue))

    print(f"Part 1: {len(str(processedValue))}")


def lookAndSayConvert(valueToProcess):
    newValue = ""
    l = 0
    while l < len(valueToProcess):
        repeats = 1
        while (l+1 < len(valueToProcess) and valueToProcess[l] == valueToProcess[l+1]):
            repeats+=1
            l+=1
        newValue += str(repeats) + valueToProcess[l]
        l+=1
    return int(newValue)

main()
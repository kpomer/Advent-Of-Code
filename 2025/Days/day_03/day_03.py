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

    part1 = 0
    part2 = 0
    for bank in fileSA:
        bank = bank.replace("\n","")

        part1 += getJoltage([-1]*2, list(bank)) # 2 batteries
        part2 += getJoltage([-1]*12, list(bank)) # 12 batteries

    print(f"Part 1: {part1}")
    print(f"Part 2: {part2}")


def getJoltage(joltageArray, bank):

    for batteryIndex in range(len(joltageArray)):
        start = joltageArray[batteryIndex-1] + 1
        end = len(bank) - len(joltageArray) + batteryIndex

        maxIndex = start
        for i in range(start+1, end+1):
            if int(bank[i]) > int(bank[maxIndex]):
                maxIndex = i
        
        joltageArray[batteryIndex] = maxIndex

    joltageString = ""
    for j in joltageArray:
        joltageString += bank[j]

    return int(joltageString)


main()
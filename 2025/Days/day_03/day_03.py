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

    sum = 0
    for bank in fileSA:
        bank = bank.replace("\n","")
    
        # Part 1
        joltage = [-1,-1]
        index = 0
        for battery in list(bank):
            if int(battery) > joltage[0] and index != len(bank) - 1:
                joltage[0] = int(battery)
                joltage[1] = -1
            elif joltage[0] != -1 and int(battery) > joltage[1]:
                joltage[1] = int(battery)
            
            index+=1

        if joltage[0] == -1 or joltage[1] == -1:
            raise Exception("Invalid")
        
        sum += int("".join(map(str, joltage)))
    
    print(f"Part 1: {sum}")

main()
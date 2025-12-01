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

    val = 50
    password1 = 0
    password2 = 0
    for i in fileSA:
        instruction = i.replace("\n", "")
        dir = instruction[0]
        dist = 0
        if(dir == "R"):
            dist = int(instruction[1:])
        elif(dir == "L"):
            dist = int(instruction[1:])*-1
        
        startVal = val
        val+=dist # Turn the dial

        val = val % 100 # Normalize 0-99

        # Part 1
        if val == 0:
            password1 += 1

        # Part 2
        if(dist == 0):
            continue

        laps = int(abs(dist) / 100) # how many full rotations
        additionalClicks = abs(dist) % 100 
        password2 += laps

        if additionalClicks == 0 or startVal == 0:
            continue
        elif(dir == "R" and (startVal + additionalClicks) >= 100):
            password2 += 1
        elif(dir == "L" and (startVal - additionalClicks) <= 0):
            password2 += 1

    print(f"Part 1: {password1}")
    print(f"Part 2: {password2}")

main()
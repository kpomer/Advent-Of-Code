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
divCache = {}


def main():
    fileSA = shr.fileAsStringArray(Current_Dir)[0].split(",")

    part1_invalid = set()
    part2_invalid = set()
    for r in fileSA:
        range = r.split("-")
        start = int(range[0])
        end = int(range[1])

        idLength = len(str(start))
        while idLength <= len(str(end)):
            for d in divisors(idLength):
                repeatVal = "1" + "0"*(d-1)
                while len(repeatVal) == d:
                    checkNum = combinedNum(repeatVal, int(idLength/d))
                    if checkNum > end:
                        # Exceeded END value
                        break
                    if(checkNum >= start):
                        # ID is within range
                        if int(idLength/d) == 2:
                            # Part1 - invalid only if repeated twice
                            part1_invalid.add(checkNum)    
                        # Part2 - invalid for any number of repeats
                        part2_invalid.add(checkNum)
                    repeatVal = str(int(repeatVal) + 1)

            idLength += 1


    # Part 1
    part1_sum = 0
    for s in part1_invalid:
        part1_sum += s
    print(f"Part 1: {part1_sum}")

    # Part 2
    part2_sum = 0
    for s in part2_invalid:
        part2_sum += s
    print(f"Part 2: {part2_sum}")



def divisors(num):
    if num in divCache:
        return divCache[num]
    
    values = []
    v = 1
    while v <= int(num/2):
        if num % v == 0:
            values.append(v)

        v += 1

    divCache[num] = values
    return values


def combinedNum(repeatVal, count):
    strVal = ""
    for c in range(count):
        strVal += repeatVal
    
    return int(strVal)

main()
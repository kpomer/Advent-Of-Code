# main.py
import sys
import os
import re
import json

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

RE_NumericPattern = r"-?[0-9]+" #Regex for determining numeric values (including negative)

def main():
    fileInput = shr.fileAsStringArray(Current_Dir)[0].strip("\n")
    
    # Part 1
    part1_sum = getRegexSum(RE_NumericPattern, fileInput)
    print("Part 1: " + str(part1_sum))

    # Part 2
    json_val = json.loads(fileInput)
    part2_sum = list_iterate(json_val, 0)
    print("Part 2: " + str(part2_sum))


def json_iterate(json_val, sum):
    if "red" in json_val.values():
        # break early if "red" is in the object values
        return sum
    
    for key in json_val:
        if str(json_val[key])[0] == "[":
            # Array
            sum = list_iterate(json_val[key], sum)
        elif str(json_val[key])[0] == "{":
            # Object
            sum = json_iterate(json_val[key], sum)
        elif re.fullmatch(RE_NumericPattern, str(json_val[key])):
            # Numeric
            sum += int(json_val[key])
        
        # else - ignore other strings

    return sum


def list_iterate(list_val, sum):

    for l in list_val:
        if str(l)[0] == "[":
            # Array
            sum = list_iterate(l, sum)
        elif str(l)[0] == "{":
            # Object
            sum = json_iterate(l, sum)
        elif re.fullmatch(RE_NumericPattern, str(l)):
            # Numeric
            sum += int(l)

        # else - ignore other strings

    return sum



def getRegexSum(re_Pattern, inputString):
    re_Matches = re.findall(re_Pattern, inputString)

    sum = 0
    for r in re_Matches:
        sum += int(r)
    return sum

main()
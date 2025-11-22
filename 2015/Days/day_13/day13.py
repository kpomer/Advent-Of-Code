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
    
    arrangementCosts = getArrangementCosts(fileSA)



def getArrangementCosts(inputList):

    dict = {}
    for i in inputList:
        arr_values = str.split(i, " ")
        name1 = arr_values[0]
        name2 = str.replace(str.replace(arr_values[10], "\n", ""), ".", "")
        num = int(arr_values[3])
        if arr_values[2] == "lose":
            num = num * -1

        namePair = tuple(sorted((name1, name2)))

        if namePair in dict:
            dict[namePair] = dict[namePair] + num
        else:
            dict[namePair] = num

    return dict

main()
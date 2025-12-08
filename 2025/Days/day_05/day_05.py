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

FreshRanges = list()
AvailableIDs = list()

def main():
    fileSA = shr.fileAsStringArray(Current_Dir, "e")
    row = 0
    
    # Fresh Ranges
    while fileSA[row] != "\n":
        line = fileSA[row].replace("\n","")
        lineSplit = line.split("-")
        processRange([int(lineSplit[0]),int(lineSplit[1])])
        formatRanges()
        # TODO Cleanup and Sort FreshRanges
        row += 1

    # Available IDs
    for r in range(row+1, len(fileSA)):
        line = fileSA[r].replace("\n","")
        AvailableIDs.append(int(line))


def processRange(rangeValues):
    
    if rangeValues in FreshRanges:
        # Duplicate Value
        return
    
    if rangeValues[0] > rangeValues[1]:
        raise Exception(f"Invalid Range Order [{rangeValues[0]},{rangeValues[1]}]")

    for r in FreshRanges:
        if rangeValues[0] < r[0] and rangeValues[1] >= r[0] and rangeValues[1] <= r[1]:
            # extend start
            r[0] = rangeValues[0]
            return
        elif rangeValues[0] >= r[0] and rangeValues[1] <= r[1]:
            # within range
            return
        elif rangeValues[0] >= r[0] and rangeValues[0] <= r[1] and rangeValues[1] > r[1]:
            # extend start
            r[1] = rangeValues[1]
            return
        
    # no matches - new range
    FreshRanges.append(rangeValues)

def formatRanges():
    FreshRanges.sort(key=sortRanges)


def sortRanges(val):
    return val[0]
        
        

#     # a = [4, 7, 9, 7, 2, 7]

#     # # Find all indices of the value 7
#     # indices = [i for i, x in enumerate(a) if x == 7]
#     # print("Indices", indices)

#     # Process File
#     lineBreak = False
#     for line in fileSA:
#         if line == "\n": # line break
#             lineBreak = True
#             continue

#         line = line.replace("\n","")
#         if lineBreak == False:
#             rangeValues = line.split("-")
#             # for r in range(int(rangeValues[0]), int(rangeValues[1])+1):
#             #     freshIDs.add(r)
            
#             # if len(filter(extendStart, freshRanges)) > 0:
#             #     i = freshRanges.index(filter(extendStart, freshRanges)[0])
#             #     newRange = [0,0]
#             #     newRange[0] = rangeValues


#         elif lineBreak == True:
#             availableIDs.append(int(line))

# def extendStart(list):
#     print('x')

#     # freshCount = 0
#     # for i in availableIDs:
#     #     if i in freshIDs:
#     #         freshCount += 1
#     # print(freshCount)

main()
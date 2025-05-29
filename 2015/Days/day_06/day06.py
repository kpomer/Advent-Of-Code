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
Lights = {}

# ## PART 1
# def main():
#     fileSA = shr.fileAsStringArray(Current_Dir)

#     # Initialize Values
#     for a in range(1000):
#         for b in range(1000):
#             lightChange(a, b, "0")

    
#     for s in fileSA:
#         executeInstruction(s, Lights)
        

#     c = 0
#     for l in Lights:
#         if Lights[l] == True:
#             c += 1

#     print(f"Part 1: {c}")
    
# def lightChange(x, y, i):

#     if(i == "0"):
#         # off
#         Lights[(x,y)] = False
#     elif(i == "1"):
#         # on
#         Lights[(x,y)] = True
#     elif(i == "t"):
#         # toggle
#         curVal = Lights[(x,y)]
#         Lights[(x,y)] = not curVal
#     else:
#         raise Exception(f"Invalid Light Change input'{i}'.  Allowed values are '0','1', or 't'")
    

# def executeInstruction(instruction, lights):
#     instructionSplit = str.split(instruction, " ")
#     i = ""
#     x1 = 0
#     x2 = 0
#     y1 = 0
#     y2 = 0

#     if(instructionSplit[0] == "toggle"):
#         i = "t"
#         x1 = int(str.split(instructionSplit[1], ",")[0])
#         y1 = int(str.split(instructionSplit[1], ",")[1])
#         x2 = int(str.split(instructionSplit[3], ",")[0])
#         y2 = int(str.split(instructionSplit[3], ",")[1])
#     else:
#         if(instructionSplit[1] == "off"):
#             i = "0"
#         elif(instructionSplit[1] == "on"):
#             i = "1"

#         x1 = int(str.split(instructionSplit[2], ",")[0])
#         y1 = int(str.split(instructionSplit[2], ",")[1])
#         x2 = int(str.split(instructionSplit[4], ",")[0])
#         y2 = int(str.split(instructionSplit[4], ",")[1])


#     for dx in range(max(x1, x2) - min(x1, x2) + 1):
#         for dy in range(max(y1, y2) - min(y1, y2) + 1):
#             xVal = min(x1, x2) + dx
#             yVal = min(y1, y2) + dy

#             lightChange(xVal, yVal, i)


## PART 1
def main():
    fileSA = shr.fileAsStringArray(Current_Dir)

    # Initialize Values
    for a in range(1000):
        for b in range(1000):
            Lights[a,b] = 0

    
    for s in fileSA:
        executeInstruction(s, Lights)
        

    b = 0
    for l in Lights:
        b += Lights[l]

    print(f"Part 2: {b}")
    
def lightChange(x, y, i):

    curVal = Lights[(x,y)]
    Lights[(x,y)] = max(curVal + i, 0) # Min value 0
    

def executeInstruction(instruction, lights):
    instructionSplit = str.split(instruction, " ")
    i = ""
    x1 = 0
    x2 = 0
    y1 = 0
    y2 = 0

    if(instructionSplit[0] == "toggle"):
        i = 2
        x1 = int(str.split(instructionSplit[1], ",")[0])
        y1 = int(str.split(instructionSplit[1], ",")[1])
        x2 = int(str.split(instructionSplit[3], ",")[0])
        y2 = int(str.split(instructionSplit[3], ",")[1])
    else:
        if(instructionSplit[1] == "off"):
            i = -1
        elif(instructionSplit[1] == "on"):
            i = 1

        x1 = int(str.split(instructionSplit[2], ",")[0])
        y1 = int(str.split(instructionSplit[2], ",")[1])
        x2 = int(str.split(instructionSplit[4], ",")[0])
        y2 = int(str.split(instructionSplit[4], ",")[1])


    for dx in range(max(x1, x2) - min(x1, x2) + 1):
        for dy in range(max(y1, y2) - min(y1, y2) + 1):
            xVal = min(x1, x2) + dx
            yVal = min(y1, y2) + dy

            lightChange(xVal, yVal, i)



main()
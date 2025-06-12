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
WireValues = {} #{xx: 111}
InstructionList = []
Operators = {"NOT", "AND", "OR", "LSHIFT", "RSHIFT"}



def main():
    fileSA = shr.fileAsStringArray(Current_Dir)

    part1Result = 0

    ## PART 1 and PART 2
    for p in [1,2]:

        # Clear values
        WireValues.clear()
        InstructionList.clear()

        for i in fileSA:

            # Part 2
            if p == 2 and i[-6:] == " -> b\n":
                i = f"{part1Result} -> b\n"  

            InitializeInstructionList(i)

        while(len(InstructionList) > 0):
            for inst in InstructionList:
                if(inst[1].isnumeric() or inst[1] in WireValues):
                    if(inst[2] == None or inst[2].isnumeric() or inst[2] in WireValues):
                        PerformInstruction(inst)
                        InstructionList.remove(inst)

        if p == 1:
            part1Result = int(WireValues['a'], 2) 
            print(f"Part 1: {part1Result}")
        elif p == 2:
            print(f"Part 2: {int(WireValues['a'], 2)}")

    # a
    # a AND b
    # a LSHIFT b
    # a RSHIFT b
    # NOT b
    # a OR b
    

def InitializeInstructionList(instruction):
    instruction = str.replace(instruction, "\n", "")
    i_split = str.split(instruction)
    
    #[OPERATOR, VAL1, VAL2, RESULT]
    parsedInstruction = [None]*4

    resultVal = False
    for s in i_split:
        if(s in Operators):
            parsedInstruction[0] = s
        elif(s == "->"):
            resultVal = True
        elif(resultVal):
            #Final Value is Result
            parsedInstruction[3] = s
        elif(parsedInstruction[1] == None):
            parsedInstruction[1] = s
        elif(parsedInstruction[2] == None):
            parsedInstruction[2] = s
        else:
            raise Exception(f"Invalid Instruction: '{instruction}'")

    InstructionList.append(parsedInstruction)


def PerformInstruction(inst):
    # a -> c
    # a AND b -> c
    # a LSHIFT b -> c
    # a RSHIFT b -> c
    # NOT b -> c
    # a OR b -> c
    
    a = 0
    b = 0
    
    if(inst[0] == None):
        # a -> c
        WireValues[inst[3]] = ToBinary(NumOrValue(inst[1]))
    elif(inst[0] == "AND"):
        # a AND b -> c
        WireValues[inst[3]] = ToBinary(NumOrValue(inst[1]) & NumOrValue(inst[2]))
    elif(inst[0] == "LSHIFT"):
        # a LSHIFT b -> c
        WireValues[inst[3]] = ToBinary(NumOrValue(inst[1]) << NumOrValue(inst[2]))
    elif(inst[0] == "RSHIFT"):
        # a RSHIFT b -> c
        WireValues[inst[3]] = ToBinary(NumOrValue(inst[1]) >> NumOrValue(inst[2]))
    elif(inst[0] == "NOT"):
        # NOT b -> c
        WireValues[inst[3]] = NOTOp(ToBinary(NumOrValue(inst[1])))
    elif(inst[0] == "OR"):
        # a OR b -> c
        WireValues[inst[3]] = ToBinary(NumOrValue(inst[1]) | NumOrValue(inst[2]))


def NumOrValue(x):
    
    if x.isnumeric():
        return int(x)
    else:
        return int(WireValues[x], 2)
    
def ToBinary(i):
    b = str(bin(i))

    #Set 16 bits
    if(len(b) > 18):
        b = "0b" + b[-16:]
    elif(len(b) < 18):
        b = "0b" + ("0000000000000000" + b[2:])[-16:]

    return b

def NOTOp(b):
    b_lst = list(b)
    b_NOT = ""
    for l in range(len(b_lst)):
        if l < 2:
            b_NOT = b_NOT + b_lst[l]
        elif l >= 2:
            if b_lst[l] == '0':
                b_NOT = b_NOT + '1'
            elif b_lst[l] == '1':
                b_NOT = b_NOT + '0'

    return b_NOT



main()
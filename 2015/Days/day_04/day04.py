# main.py
import sys
import os
import hashlib

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
    file = shr.fileAsStringArray(Current_Dir)[0]
    
    ## PART 1
    for d in range(10000000):
        val = file + str(d)
        hashVal = hashlib.md5(val.encode())
        hexVal = hashVal.hexdigest()
        
        if(hexVal[:5] == "00000"):
            print(f"Part 1: {d}")
            break

    ## PART 2
    for d in range(100000000):
        val = file + str(d)
        hashVal = hashlib.md5(val.encode())
        hexVal = hashVal.hexdigest()
        
        if(hexVal[:6] == "000000"):
            print(f"Part 2: {d}")
            break
        

main()
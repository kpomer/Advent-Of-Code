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
    password = fileSA[0].strip("\n")

    # Part 1
    while meetsRequirements(password) == False:
        password = incrementPassword(password)
    print("Part 1: " + password)

    # Part 2
    password = incrementPassword(password)
    while meetsRequirements(password) == False:
        password = incrementPassword(password)
    print("Part 2: " + password)


def meetsRequirements(passwordValue):
    # Req 1 - 1+ increasing straight3
    # Req 2 - exclude i, o, l
    # Req 3 - 2+ non-overlapping pairs

    #Req 2
    for invalidChar in ["i","o","l"]:
        if passwordValue.find(invalidChar) != -1:
            return False
        
    #Req 1
    straight3 = False
    for l in range(len(passwordValue)-2):
        if(ord(passwordValue[l]) == ord(passwordValue[l+1])-1):
            if(ord(passwordValue[l+1]) == ord(passwordValue[l+2])-1):
                straight3 = True
                break
    if straight3 == False:
        return False
    
    
    #Req 3
    c = 0
    pairCount = 0
    while (pairCount < 2) and (c < len(passwordValue) - 1):
        if passwordValue[c] == passwordValue[c+1]:
            pairCount += 1
            c+=1
        c+=1
    if pairCount < 2:
        return False
    
    return True
    
    

def incrementPassword(password):
    passwordList = list(password)
    i = len(passwordList) - 1
    while passwordList[i] == "z":
        passwordList[i] = "a"
        i-=1
    
    passwordList[i] = chr(ord(passwordList[i])+1)
    return "".join(passwordList)




main()
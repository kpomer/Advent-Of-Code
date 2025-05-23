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
    # print(fileSA)

    niceWords = {}

    #Nice Strings
    vowels = {"a","e","i","o","u"}
    badStrings = {"ab","cd","pq","xy"}

    #loop through strings in input
    for s in fileSA:
        vowelCount = 0
        doubleLetters = False

        #loop through letters in string
        l = 0
        while(l < len(s)):
            if(l != len(s) - 1):
                if (str(s[l] + s[l+1]) in badStrings):
                    break

            if(l in vowels):
                vowelCount += 1

            l += 1
            if(l == len(s) and vowelCount >= 3 and doubleLetters == True):
                niceWords.add(s)
            
            



main()
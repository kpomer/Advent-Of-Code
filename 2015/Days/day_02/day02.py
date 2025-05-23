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
    # print(fileSA)

    fullSurfaceArea = 0
    fullRibbonLength = 0
    for d in fileSA:
        l = int(d.split("x")[0])
        w = int(d.split("x")[1])
        h = int(d.split("x")[2])

        #Surface Area
        surfaceArea = 2*l*w + 2*w*h + 2*h*l
        extraSurfaceArea = min(l*w,l*h,w*h)
        presentTotalSurface = surfaceArea + extraSurfaceArea
        fullSurfaceArea += presentTotalSurface

        #Ribbon
        ribbonLength = 2*min(l+w, w+h, h+l)
        extraRibbon = l*w*h
        presentTotalRibbon = ribbonLength + extraRibbon
        fullRibbonLength += presentTotalRibbon

    print(f"Part 1: {fullSurfaceArea}")
    print(f"Part 2: {fullRibbonLength}")

main()
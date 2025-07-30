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

class Route:
  def __init__(self, pointA, pointB, distance):
    self.p1 = pointA
    self.p2 = pointB
    self.dist = distance

def main():
    fileSA = shr.fileAsStringArray(Current_Dir, "e")
    
    routes = []
    nodes = set()
    nodeDict = dict()

    for r in fileSA:
        r = str.replace(r, "\n", "")
        
        r_Split = r.split(" ")
        routes.append(Route(r_Split[0], r_Split[2], r_Split[4]))
        for i in {r_Split[0], r_Split[2]}:
           if(i not in nodes):
              nodeDict[len(nodes)] = i
              nodes.add(i)



        # Pathfinding needed - maybe Dijkstra's or A*, but must check ALL nodes (not only shortest from A->Z)
        # Traveling Salesman Problem - Held-Karp algorithm 


main()
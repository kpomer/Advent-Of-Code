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
Corners = [(-1, -1),(-1, -1),(-1, -1),(-1, -1)] # [^<, ^>, v>, v<]
CornerIndicies = [-1,-1,-1,-1] # [^<, ^>, v>, v<]
MaxWidth = 0
MaxHeight = 0

greenRangesX = {} # x: [yMin, yMax]
greenRangesY = {} # y: [xMin, xMax]

HorizontalBorders = {} # y: [xStart, xEnd]
HorizontalBorders_Y = [] # [y]
VerticalBorders = {} # x: [yStart, yEnd]
VerticalBorders_X = [] # [x]
PB = set()
BorderCrossingCache = {}


def main():
    # global MaxWidth, MaxHeight
    fileSA = shr.fileAsStringArray(Current_Dir)
    redCoordinates = []

    iterator = 0
    for line in fileSA:
        # Store red coordinates
        line = line.replace("\n","").split(",")
        coordinate = (int(line[0]),int(line[1]))
        redCoordinates.append(coordinate)
        setCorners(coordinate, iterator)
        iterator += 1


    # Part 1 - Find Max Area between two red corners
    maxArea = 0
    count = 0
    for r1 in redCoordinates:
        for r2 in redCoordinates:
            area = (max(r1[0], r2[0]) - min(r1[0], r2[0])+1) * (max(r1[1], r2[1]) - min(r1[1], r2[1])+1)
            maxArea = max(maxArea, area)
            count += 1
    print(f"Part 1: {maxArea}")
    print(f"Count: {count}")


    # Part 2
    r = 0
    while r < len(redCoordinates):
        c1 = redCoordinates[r]
        c2 = redCoordinates[changeIndex(r, 1, len(redCoordinates))]

        if c1[0] == c2[0]:
            # vertical line - Y ranges
            maxVal = max(c1[1], c2[1])
            minVal = min(c1[1], c2[1])
            x = c1[0]
            if x in VerticalBorders:
                raise Exception(f"value already exists {x}")
            else:
                VerticalBorders[x] = [minVal, maxVal]
                VerticalBorders_X.append(x)

        elif c1[1] == c2[1]:
            # horizontal line - X ranges
            maxVal = max(c1[0], c2[0])
            minVal = min(c1[0], c2[0])
            y = c1[1]
            if y in HorizontalBorders:
                raise Exception(f"value already exists {y}")
            else:
                HorizontalBorders[y] = [minVal, maxVal]
                HorizontalBorders_Y.append(y)
        else:
            raise Exception(f"Invalid Values {c1} and {c2}")

        r += 1

    HorizontalBorders_Y.sort()
    VerticalBorders_X.sort()    

    stopHere = 0

    maxArea = 0
    count = 0
    for r1 in redCoordinates:
        for r2 in redCoordinates:
            area = (max(r1[0], r2[0]) - min(r1[0], r2[0])+1) * (max(r1[1], r2[1]) - min(r1[1], r2[1])+1)
            count += 1
            if area > maxArea:
                # Check edges are within borders
                if withinBorders(r1, r2):
                    maxArea = area
    
    # 2469403664 too high
    # 1341397176 too low
    print(f"Part 2: {maxArea}")
    print(f"Count: {count}")

def withinBorders(r1, r2):
    # check that all edges are within borders

    # get corners in correct orientation [^<, ^>, v>, v<]
    [a,b,c,d] = getCornersCoordinates(r1, r2)

    # a -> b (check above)
    p = a
    while p != b:
        if pointHitsBorder(p, "^") == False:
            return False
        p = shr.moveCoordinate(p, ">")

    # b -> c (check right)
    p = b
    while p != c:
        if pointHitsBorder(p, ">") == False:
            return False
        p = shr.moveCoordinate(p, "v")

    # d -> c (check down)
    p = d
    while p != c:
        if pointHitsBorder(p, "v") == False:
            return False
        p = shr.moveCoordinate(p, ">")

    # a -> d (check left)
    p = a
    while p != d:
        if pointHitsBorder(p, "<") == False:
            return False
        p = shr.moveCoordinate(p, "v")

    return True


def pointHitsBorder(p, dir, list):

    if (p, dir) in PB:
        return True

    # if shr.validCoordinate(p, 100000, 100000) == False: # TODO Change to check array size
    if len(list) == 0:
        return False
    elif dir in ["^", "v"] and p[1] in HorizontalBorders and p[0] >= HorizontalBorders[p[1]][0] and p[0] <= HorizontalBorders[p[1]][1]:
        PB.add((p, dir))
        return True
    elif dir in ["<", ">"] and p[0] in VerticalBorders and p[1] >= VerticalBorders[p[0]][0] and p[1] <= VerticalBorders[p[0]][1]:
        PB.add((p, dir))
        return True
    else:
        p = shr.moveCoordinate(p, dir)
        return pointHitsBorder(p, dir)
    
    

def getCornersCoordinates(r1, r2):
    # get corners in correct orientation [^<, ^>, v>, v<]

    if r1[0] <= r2[0]:
        if r1[1] <= r2[1]:
            return [r1, (r2[0],r1[1]), r2, (r1[0],r2[1])]
        else:
            return [(r1[0],r2[1]), r2, (r2[0],r1[1]), r1]
    else:
        if r2[1] <= r1[1]:
            return [r2, (r1[0],r2[1]), r1, (r2[0],r1[1])]
        else:
            return [(r2[0],r1[1]), r1, (r1[0],r2[1]), r2]

def getRemainingBorders(xy_Val, list, high_low):
    if high_low == ">":
        list = filter(lambda z: z > xy_Val, list)
    elif high_low == "<":
        list = filter(lambda z: z < xy_Val, list)
    else:
        raise Exception(f"Invalid high_low value {high_low}")
    return list
    

# def pointWithinBorders(p, dir):

#     if p in WB:
#         return True
#     elif shr.validCoordinate(p, 100000, 100000) == False:
#         return False
#     elif p[0] in VerticalBorders and VerticalBorders[p[0]][0] <= p[1] and VerticalBorders[p[0]][1] >= p[1]:
#         WB.add(p)
#         return True
#     elif p[1] in HorizontalBorders and HorizontalBorders[p[1]][0] <= p[0] and HorizontalBorders[p[1]][1] >= p[0]:
#         WB.add(p)
#         return True
#     else:
#         p = shr.moveCoordinate(p, dir)
#         return pointWithinBorders(p, dir)
    
    


    # ^

    # >

    # v

    # <


    #     if c1[0] == c2[0]:
    #         # vertical line - Y ranges
    #         maxVal = max(c1[1], c2[1])
    #         minVal = min(c1[1], c2[1])
    #         y = minVal
    #         x = c1[0]
    #         while y <= maxVal:
    #             if y in greenRangesY:
    #                 greenRangesY[y].append(x)
    #                 greenRangesY[y].sort()
    #                 greenRangesY[y] = [greenRangesY[y][0], greenRangesY[y][-1]]
    #             else:
    #                 greenRangesY[y] = [x]
    #             y += 1
    #     elif c1[1] == c2[1]:
    #         # horizontal line - X ranges
    #         maxVal = max(c1[0], c2[0])
    #         minVal = min(c1[0], c2[0])
    #         x = minVal
    #         y = c1[1]
    #         while x <= maxVal:
    #             if x in greenRangesX:
    #                 greenRangesX[x].append(y)
    #                 greenRangesX[x].sort()
    #                 greenRangesX[x] = [greenRangesX[x][0], greenRangesX[x][-1]]
    #             else:
    #                 greenRangesX[x] = [y]
    #             x += 1
    #     else:
    #         raise Exception(f"Invalid Values {c1} and {c2}")

    #     r += 1

    # maxArea = 0
    # for r1 in redCoordinates:
    #     for r2 in redCoordinates:
    #         area = (max(r1[0], r2[0]) - min(r1[0], r2[0])+1) * (max(r1[1], r2[1]) - min(r1[1], r2[1])+1)
    #         if area > maxArea:
    #             # Check 4 corners within border
    #             if checkCorners(r1, r2):
    #                 maxArea = area
    
    # # 4618516475 too high
    # print(f"Part 2: {maxArea}")




    # # Top Left, Bottom Left
    # index = changeIndex(0, CornerIndicies[0], len(redCoordinates))
    # coordinate = redCoordinates[index]
    # nextCoordinate = redCoordinates[changeIndex(index, -1, len(redCoordinates))]
    # index = changeIndex(index, -1, len(redCoordinates))
    # while coordinate != Corners[3]:
    #     x = coordinate[0]
    #     y = coordinate[1]
    #     yNext = nextCoordinate[1]
    #     while y <= yNext:
    #         if y in greenRangesY and greenRangesY[y][0] > x:
    #             greenRangesY[y][0] = x
    #         elif y not in greenRangesY:
    #             greenRangesY[y] = [x, -1]
    #         y += 1
    #     coordinate = nextCoordinate
    #     nextCoordinate = redCoordinates[changeIndex(index, -1, len(redCoordinates))]
    #     index = changeIndex(index, -1, len(redCoordinates))

    # # Top Right, Bottom Right
    # index = changeIndex(0, CornerIndicies[1], len(redCoordinates))
    # coordinate = redCoordinates[index]
    # nextCoordinate = redCoordinates[changeIndex(index, 1, len(redCoordinates))]
    # index = changeIndex(index, 1, len(redCoordinates))
    # while coordinate != Corners[2]:
    #     x = coordinate[0]
    #     y = coordinate[1]
    #     yNext = nextCoordinate[1]
    #     while y <= yNext:
    #         if y in greenRangesY and greenRangesY[y][1] < x:
    #             greenRangesY[y][1] = x
    #         elif y not in greenRangesY:
    #             greenRangesY[y] = [x, -1] # Not Necessary line
    #         y += 1
    #     coordinate = nextCoordinate
    #     nextCoordinate = redCoordinates[changeIndex(index, 1, len(redCoordinates))]
    #     index = changeIndex(index, 1, len(redCoordinates))

    # # Top Left, Top Right
    # index = changeIndex(0, CornerIndicies[0], len(redCoordinates))
    # coordinate = redCoordinates[index]
    # nextCoordinate = redCoordinates[changeIndex(index, 1, len(redCoordinates))]
    # index = changeIndex(index, 1, len(redCoordinates))
    # while coordinate != Corners[1]:
    #     x = coordinate[0]
    #     y = coordinate[1]
    #     xNext = nextCoordinate[0]
    #     while x <= xNext:
    #         if x in greenRangesX and greenRangesX[x][0] > y:
    #             greenRangesX[x][0] = x
    #         elif x not in greenRangesX:
    #             greenRangesX[x] = [y, -1]
    #         x += 1
    #     coordinate = nextCoordinate
    #     nextCoordinate = redCoordinates[changeIndex(index, 1, len(redCoordinates))]
    #     index = changeIndex(index, 1, len(redCoordinates))

    # # Bottom Left, Bottom Right
    # index = changeIndex(0, CornerIndicies[3], len(redCoordinates))
    # coordinate = redCoordinates[index]
    # nextCoordinate = redCoordinates[changeIndex(index, -1, len(redCoordinates))]
    # index = changeIndex(index, -1, len(redCoordinates))
    # while coordinate != Corners[2]:
    #     x = coordinate[0]
    #     y = coordinate[1]
    #     yNext = nextCoordinate[1]
    #     while y <= yNext:
    #         if y in greenRangesY and greenRangesY[y][1] < x:
    #             greenRangesY[y][1] = x
    #         elif y not in greenRangesY:
    #             greenRangesY[y] = [x, -1] # Not Necessary line
    #         y += 1
    #     coordinate = nextCoordinate
    #     nextCoordinate = redCoordinates[changeIndex(index, -1, len(redCoordinates))]
    #     index = changeIndex(index, -1, len(redCoordinates))










    # # Part 2
    # index = CornerIndicies[0]

    # # greenRangesX = {} # x: [yMin, yMax]
    # # greenRangesY = {} # y: [xMin, xMax]
    # # greenRange = (redCoordinates[iterator][0],redCoordinates[iterator][0])

    
    # # top left to bottom right
    # x = redCoordinates[index][0]
    # y = redCoordinates[index][1]
    # while index != CornerIndicies[2]: #TODO 
    #     nextIndex = (index + 1) % len(redCoordinates)
    #     current = redCoordinates[index]
    #     next = redCoordinates[nextIndex]
    #     if current[1] == next[1]:
    #         # next is on same row
    #         x = next[0]
    #     elif current[1] < next[1]:
    #         # next extends downward
    #         while y <= next[1]:
    #             greenRangesY[y] = [0,x]
    #             y += 1
    #     else:
    #         raise Exception(f"Invalid at {index}")
    #     index = nextIndex

    # # bottom right to top left
    # x = redCoordinates[index][0]
    # y = redCoordinates[index][1]
    # while index != CornerIndicies[0]:
    #     nextIndex = (index + 1) % len(redCoordinates)
    #     current = redCoordinates[index]
    #     next = redCoordinates[nextIndex]
    #     if current[1] == next[1]:
    #         # next is on same row
    #         x = next[0]
    #     elif current[1] > next[1]:
    #         # next extends downward
    #         while y >= next[1]:
    #             greenRangesY[y][0] = x
    #             y -= 1
    #     else:
    #         raise Exception(f"Invalid at {index}")
    #     index = nextIndex
            
    # a = 1














    # # Store Border Coordinates
    # redIndex = -1 # Start at -1 to link last to first
    # while redIndex < len(redCoordinates) - 1:
    #     r1 = redCoordinates[redIndex]
    #     r2 = redCoordinates[redIndex+1]

    #     if r1[0] == r2[0]:
    #         y = max(r1[1], r2[1])
    #         yMin = min(r1[1], r2[1])
    #         while y >= yMin:
    #             borderCoordinates.add((r1[0],y))
    #             y-=1
    #     elif r1[1] == r2[1]:
    #         x = max(r1[0], r2[0])
    #         xMin = min(r1[0], r2[0])
    #         while x >= xMin:
    #             borderCoordinates.add((x,r1[1]))
    #             x-=1
    #     else:
    #         raise Exception(f"Invalid Entry at index {redIndex}")
    #     redIndex += 1

    # # Store Border Ranges
    # horizontalBorders = {} # {y: (x0,x1), (x2,x3)} ranges
    # verticalBorders = {} # {x: (y0,y1), (y2,y3)} ranges
    # redIndex = -1 # Start at -1 to link last to first
    # while redIndex < len(redCoordinates) - 1:
    #     r1 = redCoordinates[redIndex]
    #     r2 = redCoordinates[redIndex+1]
    #     if r1[0] == r2[0]:
    #         # vertical border
    #         x = r1[0]
    #         yMin = min(r2[1], r1[1])
    #         yMax = max(r2[1], r1[1])
    #         if x in verticalBorders:
    #             borderRanges = verticalBorders[x]
    #             borderRanges.append((yMin, yMax))
    #             borderRanges = sorted(borderRanges, key=lambda a: a[0])
    #             verticalBorders[x] = borderRanges
    #         else:
    #             verticalBorders[x] = [(yMin, yMax)]
    #     elif r1[1] == r2[1]:
    #         # horizontal border
    #         y = r1[1]
    #         xMin = min(r2[0], r1[0])
    #         xMax = max(r2[0], r1[0])
    #         if y in horizontalBorders:
    #             borderRanges = horizontalBorders[y]
    #             borderRanges.append((xMin, xMax))
    #             borderRanges = sorted(borderRanges, key=lambda a: a[0])
    #             horizontalBorders[y] = borderRanges
    #         else:
    #             horizontalBorders[y] = [(xMin, xMax)]
    #     else:
    #         raise Exception(f"Invalid Entry at index {redIndex}")
        
    #     redIndex += 1

    # for h in horizontalBorders:
    #     if len(horizontalBorders[h]) != 1:
    #         print(h)

    # for v in verticalBorders:
    #     if len(verticalBorders[v]) != 1:
    #         print(h)

    # # Part 2
    # maxArea = 0
    # for r1 in redCoordinates:
    #     for r2 in redCoordinates:
    #         area = (max(r1[0], r2[0]) - min(r1[0], r2[0])+1) * (max(r1[1], r2[1]) - min(r1[1], r2[1])+1)
    #         if area > maxArea:
    #             if correctTileColors(r1, r2, horizontalBorders, verticalBorders):
    #                 maxArea = max(maxArea, area)


    # print(f"Part 2: {maxArea}")

    
    
    # # TODO repeat part 1 logic, but if area > maxArea, make sure all 


    # # Get All UNIQUE Rectangles
    # uniqueRectangles = set()
    # rectangleAreas = []
    # for r1 in redCoordinates:
    #     for r2 in redCoordinates:
    #         r3 = (r1[0], r2[1])
    #         r4 = (r2[0], r1[1])
    #         rectangleCoordinates = (r1,r2,r3,r4)
    #         rectangleCoordinates = sorted(rectangleCoordinates, key=lambda x: (x[0], x[1]))
    #         if tuple(rectangleCoordinates) not in uniqueRectangles:
    #             uniqueRectangles.add(tuple(rectangleCoordinates))
    #             area = (max(rectangleCoordinates[0][0], rectangleCoordinates[3][0]) - min(rectangleCoordinates[0][0], rectangleCoordinates[3][0])+1) * (max(rectangleCoordinates[0][1], rectangleCoordinates[3][1]) - min(rectangleCoordinates[0][1], rectangleCoordinates[3][1])+1)
    #             rectangleCoordinates.append(area)
    #             rectangleAreas.append(rectangleCoordinates)

    # rectangleAreas.sort(key=sort, reverse=True)



    


            # sorted()
            # r1/r2/r3/r4 are ALL coordinates

            # TODO Sort (top left, top right, bottom left, bottom right)
            # TODO Calculate area
            # TODO Store in Set of rectanlges (4coordinates) and array (4coordinates and area) that will later be sorted by area [topLeft, topRight, bottomLeft, bottomRight, area]
    # Sort array by area desc - part 1 is array[0]
    # Part2 - loop through array until you find the FIRST value where the all BORDER COORDINATES OF RECTANGLE are within main borders



    # Part 2
    ## Write a function to check if a coordinate is red/green vs other
    ## Do same logic as part 1, but make sure the OTHER two corners are either red/green
    ## If two corners are red and the other corners are either red/green, it should be a valid rectangle
    ## THIS MAY NOT WORK - there could be a chunk taken out of one side... Maybe check every BORDER tile


# def correctTileColors(r1, r2, horizontalBorders, verticalBorders):
#     r3 = (r1[0], r2[1])
#     r4 = (r2[0], r1[1])
#     # r1/r3 and r2/r4 share x coordiante (vertical line)
#     # r1/r4 and r2/r3 share y coordinate (horizontal line)

#     x = 0
#     y = r1[1]
#     green_red = False

#     while x <= max(r1[0],r4[0]):
#         if x in verticalBorders and x >= verticalBorders[x][0] and x <= verticalBorders[x][1]:
#             # crossing border
#             green_red = not(green_red)


#         y += 1


def checkBorderCrossings(r1, r3):
    r2 = (r1[0], r3[1])
    r4 = (r3[0], r1[1])

    # Horizontal Edges
    for [a,b] in [[r2,r3],[r4,r1]]:
        # check for vertical border crossings
        minVal = min(a[0], b[0])
        maxVal = max(a[0], b[0])
        # x = minVal + 1
        # y = a[1]

        # while x < maxVal:
        #     if x in VerticalBorders and y > VerticalBorders[x][0] and y < VerticalBorders[x][1]:
        #         return True
        #     x += 1
        x = minVal
        y = a[1]

        while x <= maxVal:
            if x in VerticalBorders and y > VerticalBorders[x][0] and y < VerticalBorders[x][1]:
                return True
            x += 1
            
    # Vertical Edges
    for [a,b] in [[r1,r2],[r3,r4]]:
        # check for horizontal border crossings
        minVal = min(a[1], b[1])
        maxVal = max(a[1], b[1])
        # x = a[0]
        # y = minVal + 1

        # while y < maxVal:
        #     if y in HorizontalBorders and x > HorizontalBorders[y][0] and x < HorizontalBorders[y][1]:
        #         return True
        #     y += 1
        x = a[0]
        y = minVal

        while y <= maxVal:
            if y in HorizontalBorders and x > HorizontalBorders[y][0] and x < HorizontalBorders[y][1]:
                return True
            y += 1
    
    return False





    for border in [[r1,r2], [r2,r3], [r3,r4], [r4,r1]]:




        
        if border[0][0] == border[1][0]:
            # vertical line - Y ranges
            maxVal = max(c1[1], c2[1])
            minVal = min(c1[1], c2[1])
            # y = minVal
            x = c1[0]
            VerticalBorders[x] = [minVal, maxVal]
            # if x in VerticalBorders:
            #     VerticalBorders[x].append([minVal, maxVal])
            # else:
            #     VerticalBorders[x] = [[minVal, maxVal]]

        elif c1[1] == c2[1]:
            # horizontal line - X ranges
            maxVal = max(c1[0], c2[0])
            minVal = min(c1[0], c2[0])
            # x = minVal
            y = c1[1]
            HorizontalBorders[y] = [minVal, maxVal]
            # if y in HorizontalBorders:
            #     HorizontalBorders[y].append([minVal, maxVal])
            # else:
            #     HorizontalBorders[y] = [[minVal, maxVal]]
        else:
            raise Exception(f"Invalid Values {c1} and {c2}")











def checkCorners(r1, r2):
    corner1 = r1
    corner2 = r2
    corner3 = (r1[0], r2[1])
    corner4 = (r2[0], r1[1])

    for c in [corner1, corner2, corner3, corner4]:
        x = c[0]
        y = c[1]
        if x not in greenRangesX or y < greenRangesX[x][0] or y > greenRangesX[x][1]:
            return False
        elif y not in greenRangesY or x < greenRangesY[y][0] or x > greenRangesY[y][1]:
            return False
        
    return True






def setCorners(checkCoordinate, iterator):
    global MaxWidth, MaxHeight
    MaxWidth = max(MaxWidth, checkCoordinate[0])
    MaxHeight = max(MaxHeight, checkCoordinate[1])
    
    for i in range(4):
        oldCorner = Corners[i]
        if oldCorner == (-1,-1):
            Corners[i] = checkCoordinate
            CornerIndicies[i] = iterator

        elif i == 0:
            # [^<] top left
            if checkCoordinate[1] < oldCorner[1]:
                Corners[i] = checkCoordinate
                CornerIndicies[i] = iterator
            elif checkCoordinate[1] == oldCorner[1] and checkCoordinate[0] < oldCorner[0]:
                Corners[i] = checkCoordinate
                CornerIndicies[i] = iterator
        elif i == 1:
            # [^>] top right
            if checkCoordinate[1] < oldCorner[1]:
                Corners[i] = checkCoordinate
                CornerIndicies[i] = iterator
            elif checkCoordinate[1] == oldCorner[1] and checkCoordinate[0] > oldCorner[0]:
                Corners[i] = checkCoordinate
                CornerIndicies[i] = iterator
        elif i == 2:
            # [v>] bottom right
            if checkCoordinate[1] > oldCorner[1]:
                Corners[i] = checkCoordinate
                CornerIndicies[i] = iterator
            elif checkCoordinate[1] == oldCorner[1] and checkCoordinate[0] > oldCorner[0]:
                Corners[i] = checkCoordinate
                CornerIndicies[i] = iterator
        elif i == 3:
            # [v<] bottom left
            if checkCoordinate[1] > oldCorner[1]:
                Corners[i] = checkCoordinate
                CornerIndicies[i] = iterator
            elif checkCoordinate[1] == oldCorner[1] and checkCoordinate[0] < oldCorner[0]:
                Corners[i] = checkCoordinate
                CornerIndicies[i] = iterator


def changeIndex(startIndex, changeAmount, arrayLength):
    index = startIndex + changeAmount
    index = index % arrayLength
    return index

main()
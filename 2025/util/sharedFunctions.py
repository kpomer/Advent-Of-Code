import os

#Global Variables
PuzzleInputFileName = "PuzzleInput.txt"
PuzzleInputExampleFileName = "PuzzleInput_example.txt"


'''Get File from Directory'''
def getFile(rootDirectory, file = "i"):
    #Determine whether to use example or input file
    if file.lower() == "i":
        fileName = PuzzleInputFileName
    elif file.lower() == "e":
        fileName = PuzzleInputExampleFileName
    else:
        print(file)
        raise Exception(f"Invalid Input Value for file: '{file}'.  Allowed values are 'i' or 'e'")
    return open(os.path.join(rootDirectory, fileName))

'''Return file as String Array'''
def fileAsStringArray(rootDirectory, file = "i"):
    f = getFile(rootDirectory, file)
    return f.readlines()

def fileAsGrid(rootDirectory, file = "i"):
    grid = {}
    fileSA = fileAsStringArray(rootDirectory, file)

    width = len(fileSA[0].replace("\n",""))
    height = len(fileSA)

    for y in range(height):
        for x in range(width):
            grid[x,y] = fileSA[y][x]
    
    return grid, width, height




'''Coordinate Movement'''
def moveCoordinate(startingCoordinate, moveDir):
    #Storing coordinate as Tuple because it will be immutable

    if(moveDir == "^"):
        #move up
        newDir = (startingCoordinate[0], startingCoordinate[1]-1)
    elif(moveDir == ">"):
        #move right
        newDir = (startingCoordinate[0]+1, startingCoordinate[1])
    elif(moveDir == "v"):
        #move down
        newDir = (startingCoordinate[0], startingCoordinate[1]+1)
    elif(moveDir == "<"):
        #move left
        newDir = (startingCoordinate[0]-1, startingCoordinate[1])
    elif(moveDir == "^<" or moveDir == "<^"):
        #move up/left
        newDir = (startingCoordinate[0]-1, startingCoordinate[1]-1)
    elif(moveDir == "^>" or moveDir == ">^"):
        #move up/right
        newDir = (startingCoordinate[0]+1, startingCoordinate[1]-1)
    elif(moveDir == "v<" or moveDir == "<v"):
        #move down/left
        newDir = (startingCoordinate[0]-1, startingCoordinate[1]+1)
    elif(moveDir == "v>" or moveDir == ">v"):
        #move down/right
        newDir = (startingCoordinate[0]+1, startingCoordinate[1]+1)
    else:
        raise Exception(f"Invalid Move Direction: '{moveDir}'.  Allowed values are '^', '>', 'v', '<', '^<', '^>', 'v>', 'v<'")
    return newDir


def validCoordinate(coordinate, gridWidth, gridHeight):

    if coordinate[0] < 0:
        return False
    elif coordinate[0] >= gridWidth:
        return False
    if coordinate[1] < 0:
        return False
    elif coordinate[1] >= gridHeight:
        return False
    
    return True




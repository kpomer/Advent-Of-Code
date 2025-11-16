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
        #move right
        newDir = (startingCoordinate[0], startingCoordinate[1]+1)
    elif(moveDir == "<"):
        #move right
        newDir = (startingCoordinate[0]-1, startingCoordinate[1])
    else:
        raise Exception(f"Invalid Move Direction: '{moveDir}'.  Allowed values are '^', '>', 'v', '<'")
    return newDir




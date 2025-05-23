import os

#Global Variables
PuzzleInputFileName = "PuzzleInput.txt"
PuzzleInputExampleFileName = "PuzzleInput_example.txt"


'''Return file as String Array'''
def fileAsStringArray(rootDirectory, file = "i"):
    #Determine whether to use example or input file
    if file.lower() == "i":
        file = PuzzleInputFileName
    elif file.lower() == "e":
        file = PuzzleInputExampleFileName
    else:
        print(file)
        raise Exception(f"Invalid Input Value for file: '{file}'.  Allowed values are 'i' or 'e'")

    f = open(os.path.join(rootDirectory, file))
    return f.readlines()




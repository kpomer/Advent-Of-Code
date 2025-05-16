PuzzleInputFileName = "PuzzleInput.txt"
PuzzleInputExampleFileName = "PuzzleInput_example.txt"

def greet(name):
    """This function greets the person passed in as a parameter."""
    return f"Hello, {name}!"

def fileAsStringArray(fileType = "i"):
    file = ""
    if fileType == "i":
        file = PuzzleInputFileName
    elif fileType == "e":
        file = PuzzleInputExampleFileName
    else:
        raise Exception(f"Invalid value '{fileType}'.  Parameter must be either 'i' or 'e'")
    
    f = open(file)
    print(f.readlines())


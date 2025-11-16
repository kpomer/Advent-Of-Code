#!/bin/bash
# A simple script to print "Hello World"

# --- Argument Validations ---
if [ "$#" -ne 1 ] || ! [[ "$1" =~ ^[0-9]{4}$ ]]; then
    # Multiple arguments passed, or $1 is not using numeric <YYYY> format
    echo "Error: This script requires exactly one argument (a 4-digit year)."
    echo "Usage: $0 <YYYY>"
    exit 1
elif [ -d "$1" ]; then
    # <YYYY> folder already exists in current directory
    echo "Error: Folder \"$1\" already exists in current directory."
    exit 2
fi

# --- Parameter Settings ---
TEMPLATE_FOLDER="year_template"
NEW_FOLDER="$1"

# --- Source Folder Validations ---
if [ ! -d "$TEMPLATE_FOLDER" ]; then
    # Source template folder is missing
    echo "Error: The required template folder '$TEMPLATE_FOLDER' was not found."
    exit 3
fi

# Copy year_template folder and contents to new folder with input name
echo "Copying '$TEMPLATE_FOLDER' to '$NEW_FOLDER'..."
cp -r "$TEMPLATE_FOLDER" "$NEW_FOLDER"

# --- Check Copy Success ---
if [ $? -ne 0 ]; then
    # Check the exit status of the previous command (cp)
    echo "Fatal Error: Failed to copy the template folder."
    exit 4
fi

# --- Success Output ---
echo "Successfully created new project folder: $NEW_FOLDER"
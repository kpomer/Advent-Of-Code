#!/bin/bash
# A simple script to create a new Day folder for AOC (within current Year folder)

# --- Argument Validations ---
if [ "$#" -ne 1 ] || ! [[ "$1" =~ ^[0-9]{2}$ ]]; then
    # Multiple arguments passed, or $1 is not using numeric <DD> format
    echo "Error: This script requires exactly one argument (ex. '01' for new folder 'day_01')."
    echo "Usage: $0 <DD>"
    exit 1
fi

# --- Parameter Settings ---
DAYS_FOLDER="Days"
TEMPLATE_FOLDER="day_template"
NEW_FOLDER="day_$1"
RENAME_TEXT="day_00_template"

# --- Source Folder Validations ---
if [ ! -d "$DAYS_FOLDER" ]; then
    # Source 'Days' folder is missing
    echo "Error: The required folder '$DAYS_FOLDER' was not found."
    exit 2
elif [ ! -d "$DAYS_FOLDER/$TEMPLATE_FOLDER" ]; then
    # Source Days/day_template folder is missing
    echo "Error: The required folder '$DAYS_FOLDER/$TEMPLATE_FOLDER' was not found."
    exit 3
elif [ -d "$DAYS_FOLDER/$NEW_FOLDER" ]; then
    # Days/day_template folder already exists
    echo "Error: Folder \"$DAYS_FOLDER/$NEW_FOLDER\" already exists."
    exit 4
fi

# Copy year_template folder and contents to new folder with input name
echo "Copying '$DAYS_FOLDER/$TEMPLATE_FOLDER' to '$DAYS_FOLDER/$NEW_FOLDER'..."
cp -r "$DAYS_FOLDER/$TEMPLATE_FOLDER" "$DAYS_FOLDER/$NEW_FOLDER"

# --- Check Copy Success ---
if [ $? -ne 0 ]; then
    # Check the exit status of the previous command (cp)
    echo "Fatal Error: Failed to copy the template folder."
    exit 5
fi

# --- Rename File(s) ---
echo "Renaming copied files to use '$NEW_FOLDER' value..."
for file in "$DAYS_FOLDER/$NEW_FOLDER"/*"${RENAME_TEXT}"*; do
    # Check if the file exists (handles case where no matches found)
    if [ -e "$file" ]; then
        base=$(basename "$file")
        
        # Replace the template text with the new folder name
        new_name="${base//$RENAME_TEXT/$NEW_FOLDER}"
        
        # Rename the file
        mv "$file" "$DAYS_FOLDER/$NEW_FOLDER/$new_name"
        echo "  Renamed: $base -> $new_name"
    fi
done

# --- Success Output ---
echo "Successfully created new project folder: $NEW_FOLDER"
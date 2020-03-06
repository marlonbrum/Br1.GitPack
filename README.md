# Br1.GitPack
Command Line util to generate a Package folder with changes to source.

## Usage:  
    GitPack pointA..pointB buildFolder  
Generate a folder with changes from pointA in code to pointB  
   
    GitPack pointA buildFolder  
Generate a folder with changes from pointA in code to last version

pointA e pointB could be a commit, branch or tag.

## Examples:

    GitPack dev..release ..\builds  
Generates a folder with files that are in dev branch but not in release branch, in a folder on the ..\builds folder.
   
    GitPack v1.2 ..\builds 
Generates a folder with files changed since version v1.2 (A tag in the Git history).
   

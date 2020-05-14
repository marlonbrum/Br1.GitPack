
# Br1.GitPack
Command Line util to generate a Package folder with changes to source.

## Exe File: 
https://github.com/marlonbrum/Br1.GitPack/tree/master/exe

## Usage:  
    GitPack source buildFolder [options]
**source** - The especification of what to get. This parameter will be passed to git diff, so you can see [git diff documentation](https://git-scm.com/docs/git-diff) to find what to use. It can be a commit id, a tag, two commits etc. 

**buildFolder** - The destination folder to copy the files. If the folder does not exists, it will be created. If it exists, all files in it will be deleted, unless the -keep parameter is informed.

**options** 
-keep => If present, existing files in build folder **won't** be deleted.

## Examples:

    GitPack dev..release ..\build  
Generates a folder with files that are in dev branch but not in release branch, in ..\build folder.
   
    GitPack v1.2 ..\build 
Generates a folder with files changed since version v1.2 (A tag in the Git history).
   
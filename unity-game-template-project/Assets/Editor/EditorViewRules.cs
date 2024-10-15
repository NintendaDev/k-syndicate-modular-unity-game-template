public class EditorViewRules
{
    [VFolders.Rule]
    static void FoldersColorRules(VFolders.Folder folder)
    {
        if (folder.name == "Game")
            folder.color = 7;
        
        else if (folder.name == "Scripts")
            folder.color = 1;
        
        else if (folder.name == "Modules")
            folder.color = 5;
        
        else if (folder.name == "Tests")
            folder.color = 8;
    }
}
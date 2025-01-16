using System.Collections.Generic;

namespace FolderManager
{
    [System.Serializable]
    public class TemplateFolder
    {
        public string Name; // Folder name
        public string ParentPath; // Parent folder path
        public List<TemplateFolder> SubFolders = new List<TemplateFolder>(); // Nested folders
    }

    [System.Serializable]
    public class Template
    {
        public string Name; // Template name
        public List<TemplateFolder> FlatFolders = new List<TemplateFolder>(); // Flattened folder list

        // Flatten the hierarchical folder structure
        public void Flatten(TemplateFolder root, string parentPath = "")
        {
            FlatFolders.Add(new TemplateFolder
            {
                Name = root.Name,
                ParentPath = parentPath
            });

            foreach (var subFolder in root.SubFolders)
            {
                Flatten(subFolder, $"{parentPath}/{root.Name}");
            }
        }

        // Reconstruct the hierarchy from the flattened list
        public TemplateFolder ReconstructHierarchy()
        {
            var root = new TemplateFolder { Name = "Root" };
            var folderLookup = new Dictionary<string, TemplateFolder>
            {
                { "", root }
            };

            foreach (var folder in FlatFolders)
            {
                if (!folderLookup.TryGetValue(folder.ParentPath, out var parent))
                    continue;

                var newFolder = new TemplateFolder { Name = folder.Name };
                parent.SubFolders.Add(newFolder);
                folderLookup[$"{folder.ParentPath}/{folder.Name}"] = newFolder;
            }

            return root;
        }
    }

    [System.Serializable]
    public class TemplateList
    {
        public List<Template> Templates = new List<Template>();
    }
}

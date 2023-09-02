using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViabelliWebProject.Packages.Core.CrossCuttingConcerns.SeriLog.ConfigurationModel;

public class FileConfiguration
{
    public string FolderPath { get; set; }
    public FileConfiguration()
    {
        FolderPath = string.Empty;
    }
    public FileConfiguration(string folderPath)
    {
        FolderPath = folderPath;
    }

}

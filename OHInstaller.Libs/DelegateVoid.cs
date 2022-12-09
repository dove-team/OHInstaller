using System;

namespace OHInstaller.Libs
{
    public delegate void ProgressHandler(string progressPercentage);
    public delegate void ProgressCompleteHandler(object sender, bool hasError, Uri address, string filePath);
}
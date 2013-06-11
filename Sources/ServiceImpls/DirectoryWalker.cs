using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ServiceImpls
{
    /// <summary>
    /// Рекурсивный обход каталогов.
    /// Класс создан для минимизации затрат на рекурсию.
    /// Существенно уменьшает вероятность переполнения 
    /// стэка, если директорий окажется с большим кол-вом 
    /// подкаталогов.
    /// </summary>
    class DirectoryWalker
    {
        private List<string> _subfolders;
        private string _currentFolder;
        private Stack<string> _currentFoldersStack;

        public DirectoryWalker(string folderPath)
        {
            _currentFolder = folderPath;
            _subfolders = new List<string>();
            _currentFoldersStack = new Stack<string>();
        }

        public void Run()
        {
            foreach (string dir in Directory.GetDirectories(_currentFolder))
            {
                _subfolders.Add(dir);
                _currentFoldersStack.Push(_currentFolder);
                _currentFolder = dir;
                Run();
            }
            if (_currentFoldersStack.Count > 0)
            {
                _currentFoldersStack.Pop();    
            }
            
        }

        public List<string> Results
        {
            get { return _subfolders; }
        }
    }
}

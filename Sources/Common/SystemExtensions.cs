using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Common.Interfaces;

namespace Common
{
    public static class SystemExtensions
    {
        public static bool ContainsIgnoreCase(this string str, string substr)
        {
            if (str == null)
                return false;
            return (str.IndexOf(substr, 0, StringComparison.CurrentCultureIgnoreCase) >= 0);
        }

        public static bool IsNullOrEmpty(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return true;
            return false;
        }

        /// <summary>
        /// Загружает сборку в адресное пространство домена
        /// </summary>
        /// <param name="domain">Домен в чье пространство нужно загрузить сборку</param>
        /// <param name="fileName">Имя сборки</param>
        /// <param name="fileSystem">Сервис файловой системы</param>
        /// <returns></returns>
        public static Assembly LoadAssembly(this AppDomain domain, string fileName, IFileSystem fileSystem = null)
        {
            if (fileSystem == null)
            {
                fileSystem = AppContext.FileSystem;
            }
            byte[] buffer;
            using (var fs = fileSystem.GetFileStream(fileName))
            {
                 buffer = new byte[(int)fs.Length];
                fs.Read(buffer, 0, buffer.Length);
            }
            return domain.Load(buffer);
        }

        public static bool ContainsIgnoreCase(this ICollection<string> collection, string value)
        {
            if (collection == null || collection.Count == 0)
            {
                return false;
            }

            var q = (from r in collection
                     where r.Equals(value, StringComparison.CurrentCultureIgnoreCase)
                     select r);
            return q.Any();
        }
    }
}
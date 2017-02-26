using FileTypeDetective;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Stinkhorn.Bureau.Utils
{
    public static class MimeType
    {
        public static string GetFileType(this byte[] bytes, bool mimeType = false)
        {
            var array = new byte[560];
            Array.Copy(bytes, array, Math.Min(bytes.Length, array.Length));
            FileType result;
            foreach (var current in typeof(Detective).GetFields()
                .Select(f => f.GetValue(null)).OfType<FileType>())
            {
                int num = 0;
                for (var i = 0; i < current.GetHeader().Length; i++)
                {
                    byte? b = current.GetHeader()[i];
                    if ((b.HasValue ? new int?((int)b.GetValueOrDefault()) : null).HasValue
                        && current.GetHeader()[i] != array[i + current.GetHeaderOffset()])
                    {
                        num = 0;
                        break;
                    }
                    num++;
                }
                if (num == current.GetHeader().Length)
                {
                    result = current;
                    var mime = result.GetMimeType();
                    return mimeType ? mime.Value : mime.Key;
                }
            }
            return null;
        }

        static readonly BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;

        static byte?[] GetHeader(this FileType type)
        {
            var prop = type.GetType().GetProperty("Header", flags);
            return (byte?[])prop.GetValue(type);
        }

        static int GetHeaderOffset(this FileType type)
        {
            var prop = type.GetType().GetProperty("HeaderOffset", flags);
            return (int)prop.GetValue(type);
        }

        static KeyValuePair<string, string> GetMimeType(this FileType fType)
        {
            var type = fType.GetType();
            var eProp = type.GetProperty("Extension", flags);
            var mProp = type.GetProperty("Mime", flags);
            var eTxt = (string)eProp.GetValue(fType);
            var mTxt = (string)mProp.GetValue(fType);
            return new KeyValuePair<string, string>(eTxt, mTxt);
        }
    }
}
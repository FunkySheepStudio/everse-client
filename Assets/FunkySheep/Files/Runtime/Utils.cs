using System.Text.RegularExpressions;

namespace FunkySheep.Files
{
    public static class Utils
    {
        //Get a filename extention
        public static string GetExtension(string fileString)
        {
            return Regex.Match(fileString, @"\.[A-Za-z0-9]+$").Value;
        }
    }
}
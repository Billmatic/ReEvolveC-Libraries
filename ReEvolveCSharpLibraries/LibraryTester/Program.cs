using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReEvolveCSharpLibrary.Extensions;

namespace LibraryTester
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> list = new List<string>{ "1","dds", "b", "g","er","dffs", "1","2","dds"};
            List<string> sortedList = list.RemoveDuplicateItems();         
        }
    }
}

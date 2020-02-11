using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    class Program
    {
        static void Main(string[] args)
        {
            //create a new dictionary
            Dictionary<int, string> myDict = new Dictionary<int, string>();
            myDict.Add(1,"otto");
            myDict.Add(2, "john");
            myDict.Add(3, "mike");
            myDict.Add(4, "kim");
            Query.UpdateAnimal();
            //PointOfEntry.Run();
        }
    }
}

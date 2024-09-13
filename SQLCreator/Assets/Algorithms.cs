using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SQLCreator.Assets
{
    public class Algorithms
    {
        public static bool ContainsAny<T>(in IEnumerable<T> searchSpace, IEnumerable<T> containedElements) where T : IComparable<T>
        {
            var a = searchSpace.ElementAt(12); //Táblák: 
            var b = containedElements.ElementAt(0); //Táblák:

            string t1 = "Táblák:";
            string t2 = "Táblák: ";


            
            foreach (T element in searchSpace)
            {
                if (containedElements.Contains(element))
                    return true;
            }
            return false;

        }
    }
}

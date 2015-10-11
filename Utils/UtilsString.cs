using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class UtilsString
    {
        #region INSERT

        // A REVOIR
        public static string InsertRepetedCharacter(string characterToInster, int tousLesCombiens, string chaine, bool prendreLeReste)
        {
            return String.Join(characterToInster, SubstringChaine(tousLesCombiens, chaine, prendreLeReste).ToList());
        }

        private static IEnumerable<string> SubstringChaine(int tousLesCombiens, string chaine, bool prendreLeReste)
        {
            int i, length = (chaine.Length / tousLesCombiens) * tousLesCombiens;
            for (i = 0; i < length; i += tousLesCombiens)
                yield return chaine.Substring(i, tousLesCombiens);
            if (prendreLeReste && i < chaine.Length)
                yield return chaine.Substring(i);
        }

        private static void tests()
        {
            if ("toto" != "")
                Console.WriteLine("");
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reseau
{
    public static class UtilsReseau
    {
        public static int? ConvertBinaryStringToInt(string binaire)
        {
            int? result = null;

            if (!String.IsNullOrWhiteSpace(binaire))
            { 
                try
                {
                    result = Convert.ToInt32(Convert.ToByte(binaire, 2));
                }
                catch
                {
                    result = null;
                }
            }

            return result;
        }

        public static string ConvertAdresseBinaryToDecimal(string adresse)
        {
            string result = "";

            if (!String.IsNullOrWhiteSpace(adresse))
            {
                try
                {
                    List<string> binary = adresse.Split('.').ToList();
                    List<string> dec = new List<string>();
                    foreach (string bin in binary)
                    {
                        if (ConvertBinaryStringToInt(bin).HasValue)
                            dec.Add(ConvertBinaryStringToInt(bin).Value.ToString());
                        else
                        {
                            dec = null;
                            break;
                        }
                    }

                    if (dec != null && dec.Count > 0)
                        result = String.Join(".", dec);
                }
                catch
                {
                    result = "";
                }
            }

            return result;
        }

        public static string ConvertAdresseDecimalToBinary(string adresse)
        {
            string result = "";

            if(!String.IsNullOrWhiteSpace(adresse))
            {
                try
                {
                    List<string> dec = adresse.Split('.').ToList();
                    List<string> bin = new List<string>();
                    foreach (string d in dec)
                    {
                        bin.Add(Convert.ToString(Convert.ToInt32(d), 2));
                    }

                    if (dec.Count > 0)
                        result = String.Join(".", bin);
                }
                catch
                {
                    result = "";
                }
            }

            return result;
        }

        public static string GetMasqueInverse(string masque)
        {
            string result = "";

            foreach(char c in masque)
            {
                if (c == '1')
                    result += "0";
                else if (c == '0')
                    result += "1";
                else
                    result += ".";
            }

            return result;
        }

        public static string OuBinaireByAdresse(string adresseBianire1, string adresseBianire2)
        {
            string result = "";

            if (!String.IsNullOrWhiteSpace(adresseBianire1) && !String.IsNullOrWhiteSpace(adresseBianire2))
            {
                try
                {
                    List<string> listAdresse1 = adresseBianire1.Split('.').ToList();
                    List<string> listAdresse2 = adresseBianire2.Split('.').ToList();

                    if(listAdresse1.Count == 4 && listAdresse2.Count == 4)
                    {
                        List<string> listResult = new List<string>();

                        for(int i = 0; i < 4; i++)
                        {
                            string ou = OuBinaire(listAdresse1[i], listAdresse2[i]);
                            if (!String.IsNullOrWhiteSpace(ou))
                                listResult.Add(ou);
                            else
                            {
                                listResult = null;
                                break;
                            }
                        }

                        if (listResult != null && listResult.Count > 0)
                            result = String.Join(".", listResult);
                    }
                }
                catch
                {
                    result = "";
                }
            }

            return result;
        }
        private static string OuBinaire(string binaire1, string binaire2)
        {
            string result = "";

            if (!String.IsNullOrWhiteSpace(binaire1) && !String.IsNullOrWhiteSpace(binaire2))
            {
                try
                {
                    int? b1 = ConvertBinaryStringToInt(binaire1);
                    int? b2 = ConvertBinaryStringToInt(binaire2);

                    if (b1.HasValue && b2.HasValue)
                        result = Convert.ToString(Convert.ToInt32((b1 | b2).ToString()), 2);
                }
                catch
                {
                    result = "";
                }
            }

            return result;
        }
    }
}

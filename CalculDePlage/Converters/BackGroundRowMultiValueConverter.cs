using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace CalculDePlage.Converters
{
    public class BackGroundRowMultiValueConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            { 
                if (values != null && values.Length == 2)
                {
                    string adresse = values[0] as string;
                    string poste = values[1] as string;
                    if(!String.IsNullOrWhiteSpace(poste) && !String.IsNullOrWhiteSpace(adresse))
                    {
                        int nbPoste = int.Parse(poste);
                        int nbAdresse = int.Parse(adresse);

                        if (nbPoste * 1.3 >= nbAdresse)
                            return new SolidColorBrush(Colors.Red);
                        else
                            return new SolidColorBrush(Colors.Green);
                    }
                }
            }
            catch
            { 
                return new SolidColorBrush(Colors.White);
            }

            return new SolidColorBrush(Colors.White);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

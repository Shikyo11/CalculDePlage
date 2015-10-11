using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;

namespace Reseau
{
    public class SousReseau : INotifyPropertyChanged
    {
        #region EVENT

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region PROPS

        public Guid Id { get; set; }
        public String NomSousReseau { get; set; }

        private String _NombreDePoste;
        public String NombreDePoste
        {
            get
            {
                return _NombreDePoste;
            }
            set
            {
                float nbPoste = -1;
                if (float.TryParse(value, out nbPoste))
                {
                    _NombreDePoste = value;

                    if (_SetMasque)
                        SetMasque(nbPoste);
                    SetNombreAdresse();
                }
                else
                {
                    MasqueSousReseau = "";
                }

                SetPourcentageUtilise();

                NotifyPropertyChanged();
            }
        }

        public String _MasqueSousReseau { get; set; }
        public String MasqueSousReseau
        {
            get
            {
                return _MasqueSousReseau;
            }
            set
            {
                _MasqueSousReseau = value;

                SetNombreDePoste();
                SetAdresseBroadcast();

                NotifyPropertyChanged();
            }
        }

        private String _NombreAdresse { get; set; }
        public String NombreAdresse
        {
            get
            {
                return _NombreAdresse;
            }
            set
            {
                _NombreAdresse = value;

                SetPourcentageUtilise();

                NotifyPropertyChanged();
            }
        }

        private String _IpDepart { get; set; }
        public String IpDepart
        {
            get
            {
                return _IpDepart;
            }
            set
            {
                _IpDepart = value;

                SetAdresseBroadcast();

                NotifyPropertyChanged();
            }
        }

        private String _AdresseBroadcast { get; set; }
        public String AdresseBroadcast
        {
            get
            {
                return _AdresseBroadcast;
            }
            set
            {
                _AdresseBroadcast = value;

                NotifyPropertyChanged();
            }
        }

        private double _PourcentageAdresseUtilise { get; set; }
        public double PourcentageAdresseUtilise
        {
            get
            {
                return _PourcentageAdresseUtilise;
            }
            set
            {
                _PourcentageAdresseUtilise = value;

                NotifyPropertyChanged();
            }
        }

        #region CALCULS

        private String MasqueSousReseauBinaire { get; set; }
        private String MasqueSousReseauBinaireInverse { get; set; }
        private String IpDepartBinaire { get; set; }
        private String AdresseBroadcastBinaire { get; set; }

        private bool _SetMasque = true;

        #endregion

        #region HELPER

        #endregion

        #endregion

        #region CONSTRUCTEURS

        public SousReseau()
        {
            Id = Guid.NewGuid();
        }

        public SousReseau(Guid id)
        {
            Id = id;
        }

        #endregion

        #region SUB SETTER MASQUE

        private void SetMasque(float nbPoste)
        {
            int nbZero = 1;
            while (Math.Pow(2, nbZero) <= nbPoste)
                nbZero++;

            // Calcul du nombre de 1
            int nbOne = 32 - nbZero;

            // Ecriture 1
            string masque = SetOne(nbOne, true);
            masque = masque + SetOne(nbZero, false);

            // Inversion des ecriture
            string masqueInverse = SetOne(nbOne, false);
            masqueInverse = masqueInverse + SetOne(nbZero, true);

            MasqueSousReseauBinaire = UtilsString.InsertRepetedCharacter(".", 8, masque, true);
            MasqueSousReseauBinaireInverse = UtilsString.InsertRepetedCharacter(".", 8, masqueInverse, true);
            MasqueSousReseau = UtilsReseau.ConvertAdresseBinaryToDecimal(MasqueSousReseauBinaire);
        }
        private string SetOne(int nbCharactere, bool setOne)
        {
            string result = "";

            for (int i = 0; i < nbCharactere; i++)
            {
                if (setOne)
                    result += "1";
                else
                    result += "0";
            }

            return result;
        }

        #endregion

        #region SUB SETTER NOMBRE ADRESSE

        private void SetNombreAdresse()
        {
            if (!String.IsNullOrWhiteSpace(MasqueSousReseauBinaire))
            {
                NombreAdresse = Math.Pow(2, 32 - Regex.Matches(MasqueSousReseauBinaire, "1", RegexOptions.IgnoreCase).Count).ToString();
            }
        }

        #endregion

        #region SUB SETTER ADRESSE BROADCAST

        // CALL QUAND IpDepart ou MASQUESOUSRESEAU SET
        private void SetAdresseBroadcast()
        {
            if (!String.IsNullOrWhiteSpace(IpDepart) && !String.IsNullOrWhiteSpace(MasqueSousReseauBinaireInverse))
            {
                IpDepartBinaire = UtilsReseau.ConvertAdresseDecimalToBinary(IpDepart);
                if (!String.IsNullOrWhiteSpace(IpDepartBinaire))
                {
                    AdresseBroadcastBinaire = UtilsReseau.OuBinaireByAdresse(IpDepartBinaire, MasqueSousReseauBinaireInverse);
                    if (!String.IsNullOrWhiteSpace(AdresseBroadcastBinaire))
                        AdresseBroadcast = UtilsReseau.ConvertAdresseBinaryToDecimal(AdresseBroadcastBinaire);
                }
            }
        }

        #endregion

        #region SUB SETTER DANGER

        private void SetPourcentageUtilise()
        {
            if (!String.IsNullOrWhiteSpace(NombreAdresse) && !String.IsNullOrWhiteSpace(NombreDePoste))
            {
                PourcentageAdresseUtilise = int.Parse(NombreDePoste) * 100 / int.Parse(NombreAdresse);
            }
        }

        #endregion

        #region SUB SETTER NOMBRE DE POSTE

        private void SetNombreDePoste()
        {
            // UNIQUEMENT SI NOMBRE DE POSTE PAS ENCORE SET
            if (String.IsNullOrWhiteSpace(NombreDePoste) && !String.IsNullOrWhiteSpace(MasqueSousReseau))
            {
                _SetMasque = false;

                MasqueSousReseauBinaire = UtilsReseau.ConvertAdresseDecimalToBinary(MasqueSousReseau);
                MasqueSousReseauBinaireInverse = UtilsReseau.GetMasqueInverse(MasqueSousReseauBinaire);
                NombreDePoste = NombreAdresse = Math.Pow(2, 32 - Regex.Matches(MasqueSousReseauBinaire, "1", RegexOptions.IgnoreCase).Count).ToString();

                _SetMasque = true;
            }

            _SetMasque = true;
        }

        #endregion
    }
}
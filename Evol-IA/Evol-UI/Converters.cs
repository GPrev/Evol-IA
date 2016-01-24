using PokeRules;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Evol_UI
{
    public class MoveInListConverter : IMultiValueConverter
    {
        public object Convert(object[] values, System.Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                //verify if appropriate number of values is bound
                if (values != null && values.Length == 2)
                {
                    List<Move> movesList = (values[0] as List<Move>);

                    //if converter is used with appropriate collection type
                    if (movesList != null)
                    {
                        //if there is object ID specified to be found in the collection
                        if (values[1] != null)
                        {
                            Move moveToFindId = values[1] as Move;

                            //return information if the collection contains an item with ID specified in parameter
                            return movesList.Contains(moveToFindId);
                        }
                    }
                }

                //return false if object is not found or converter is used inappropriately
                return false;
            }
            catch
            {
                return false;
            }
        }

        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class PokemonInListConverter : IMultiValueConverter
    {
        public object Convert(object[] values, System.Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                //verify if appropriate number of values is bound
                if (values != null && values.Length == 2)
                {
                    List<Pokemon> pokList = (values[0] as List<Pokemon>);

                    //if converter is used with appropriate collection type
                    if (pokList != null)
                    {
                        //if there is object ID specified to be found in the collection
                        if (values[1] != null)
                        {
                            Pokemon pokToFindId = values[1] as Pokemon;

                            //return information if the collection contains an item with ID specified in parameter
                            return pokList.Contains(pokToFindId);
                        }
                    }
                }

                //return false if object is not found or converter is used inappropriately
                return false;
            }
            catch
            {
                return false;
            }
        }

        public object[] ConvertBack(object value, System.Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }



    public class PokemonToFrontSpriteConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            Pokemon p = value as Pokemon;
            if(p != null)
            {
                return Pokedex.ActivePokedex.GetFrontSpriteSource(p.Name);
            }
            //else
            return null;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class PokemonToBackSpriteConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            Pokemon p = value as Pokemon;
            if (p != null)
            {
                return Pokedex.ActivePokedex.GetBackSpriteSource(p.Name);
            }
            //else
            return null;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

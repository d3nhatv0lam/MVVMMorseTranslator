using MVVMMorseTranslator.Models;
using MVVMMorseTranslator.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Markup;

namespace MVVMMorseTranslator.ViewModels
{
    public class ReferencesViewModel : ViewModelBase
    {
        private ReferencesModel _referencesModel;
        public String Thanks
        {
            get => _referencesModel.Thanks;
        }

        public String Person1
        {
            get => _referencesModel.Person1;
        }

        public String Reason1 
        {
            get => _referencesModel.Reason1; 
        }

        public String Person2
        {
            get => _referencesModel.Person2;
        }

        public String Reason2
        {
            get => _referencesModel.Reason2;
        }




        public ReferencesViewModel() 
        {
            _referencesModel = new ReferencesModel();
        }

    }
}

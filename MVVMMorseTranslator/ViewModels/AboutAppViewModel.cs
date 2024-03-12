using MVVMMorseTranslator.Models;
using MVVMMorseTranslator.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMMorseTranslator.ViewModels
{
    public class AboutAppViewModel : ViewModelBase
    {
        private readonly AboutAppModel _aboutAppModel;

        public String AppPowered
        {
            get => _aboutAppModel.AppPowered;
        }

        public String AppDetails
        {
            get => _aboutAppModel.AppDetails;
        }

        public String Features
        {
            get => _aboutAppModel.Features;
        }

        public String Feature1
        {
            get => _aboutAppModel.Feature1;
        }

        public String Feature2
        {
            get => _aboutAppModel.Feature2;
        }

        public String Feature3
        {
            get => _aboutAppModel.Feature3;
        }

        public String Feature4
        {
            get => _aboutAppModel.Feature4;
        }


        public AboutAppViewModel()
        {
            _aboutAppModel = new AboutAppModel();
        }
    }
}

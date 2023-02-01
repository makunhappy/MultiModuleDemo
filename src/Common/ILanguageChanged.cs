using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class CultureArgs : EventArgs
    {
        public CultureArgs(CultureInfo cultureInfo)
        {
            CultureInfo = cultureInfo;
        }
        public CultureInfo CultureInfo { get; private set; }
    }
    public interface ILanguageChangedService
    {
        event EventHandler<CultureArgs> LanguageChanged;
        void ChangeLanguage();
    }
    public class LanguageChangedService : ILanguageChangedService
    {
        public event EventHandler<CultureArgs> LanguageChanged;
        private void RaiseLanguageChanged()
        {
            LanguageChanged?.Invoke(this, new CultureArgs(CultureInfo.CurrentCulture));
        }
        public void ChangeLanguage()
        {
            RaiseLanguageChanged();
        }
    }
}

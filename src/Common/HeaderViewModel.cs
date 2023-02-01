using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism;
using Prism.Mvvm;

namespace Common
{
    public abstract class ViewModelBase : BindableBase, IActiveAware
    {
        private readonly ILanguageChangedService _languageChangedService;
        public ViewModelBase(ILanguageChangedService languageChangedService,
            string title)
        {
            _languageChangedService = languageChangedService;
            this.Title = title;

            _languageChangedService.LanguageChanged += OnLanguageChanged;
        }


        private string title;

        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }



        public event EventHandler IsActiveChanged;

        private bool isActive;

        public bool IsActive
        {
            get { return isActive; }
            set
            {
                if (SetProperty(ref isActive, value))
                {
                    OnIsActiveChanged();
                }
            }
        }
        private void OnIsActiveChanged()
        {
            IsActiveChangedCore(IsActive);
        }
        /// <summary>
        /// 模块界面切换的处理函数
        /// </summary>
        /// <param name="isActive">是否未激活状态</param>
        protected virtual void IsActiveChangedCore(bool isActive) { }

        private void OnLanguageChanged(object? sender, CultureArgs e)
        {
            LanguageChangedCore(e.CultureInfo);
        }
        /// <summary>
        /// 语言变化的处理函数
        /// </summary>
        /// <param name="cultureInfo">切换后的语言文化</param>
        public virtual void LanguageChangedCore(CultureInfo cultureInfo)
        {
            this.Title = this.title + 1;
        }
    }
}

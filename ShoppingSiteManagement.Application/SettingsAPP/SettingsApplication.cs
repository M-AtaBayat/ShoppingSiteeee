using _0_Framework.Application;
using ShoppingSiteManagement.Application.Contracts.SettingsAPC;
using ShoppingSiteManagement.Domain.SettingsAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingSiteManagement.Application.SettingsAPP
{
    public class SettingsApplication : ISettingsApplication
    {
        private readonly ISiteSettingsRepository _settingsRepository;

        public SettingsApplication(ISiteSettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public OperationResult Edit(EditSettings command)
        {
            var operation = new OperationResult();
            var settings = _settingsRepository.Get(1);

            if (settings == null)
            {
                settings = new SiteSettings(command.ShippingCost, "admin@site.com");
                _settingsRepository.Add(settings);
            }
            else
            {
                settings.ChangeShippingCost(command.ShippingCost);
            }

            _settingsRepository.Save();
            return operation.Success("تنظیمات با موفقیت به‌روزرسانی شد.");
        }

        public SettingsViewModel GetSettings()
        {
            var settings = _settingsRepository.Get(1);
            if (settings == null) return new SettingsViewModel();

            return new SettingsViewModel
            {
                Id = settings.Id,
                ShippingCost = settings.ShippingCost,
                AdminEmail = settings.AdminEmail
            };
        }
    }
}
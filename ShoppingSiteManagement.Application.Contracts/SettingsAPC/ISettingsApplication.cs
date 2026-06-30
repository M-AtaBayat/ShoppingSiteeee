using _0_Framework.Application;

namespace ShoppingSiteManagement.Application.Contracts.SettingsAPC
{
    public interface ISettingsApplication
    {
        OperationResult Edit(EditSettings command);
        SettingsViewModel GetSettings();
    }
}

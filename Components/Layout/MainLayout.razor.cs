using MudBlazor;

namespace Szakdolgozat.Components.Layout
{
    public partial class MainLayout
    {
        private bool _isDarkMode = true;
        bool _drawerOpen = true;
        private MudTheme _theme = new MudTheme();

        private void ToggleTheme()
        {
            _isDarkMode = !_isDarkMode;
        }
        void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }
    }
}

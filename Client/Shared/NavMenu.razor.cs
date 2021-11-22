﻿using Microsoft.AspNetCore.Components;

namespace BlazorApp.Client.Shared
{
    public partial class NavMenu : ComponentBase
    {
        private bool collapseNavMenu = true;

        private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

        private void ToggleNavMenu()
        {
            collapseNavMenu = !collapseNavMenu;
        }
    }
}

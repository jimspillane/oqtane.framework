﻿using Oqtane.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Reflection;
using System;
using Oqtane.Shared;

namespace Oqtane.Services
{
    public class ThemeService : ServiceBase, IThemeService
    {
        private readonly HttpClient _http;

        public ThemeService(HttpClient http) : base(http)
        {
            _http = http;
        }

        private string Apiurl => CreateApiUrl("Theme");

        public async Task<List<Theme>> GetThemesAsync()
        {
            List<Theme> themes = await GetJsonAsync<List<Theme>>(Apiurl);
            return themes.OrderBy(item => item.Name).ToList();
        }

        public Dictionary<string, string> GetThemeTypes(List<Theme> themes)
        {
            var selectableThemes = new Dictionary<string, string>();
            foreach (Theme theme in themes)
            {
                foreach (string themecontrol in theme.ThemeControls.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    selectableThemes.Add(themecontrol, theme.Name + " - " + Utilities.GetTypeNameLastSegment(themecontrol, 0));
                }
            }
            return selectableThemes;
        }

        public Dictionary<string, string> GetPaneLayoutTypes(List<Theme> themes, string themeName)
        {
            var selectablePaneLayouts = new Dictionary<string, string>();
            foreach (Theme theme in themes)
            {
                if (Utilities.GetTypeName(themeName).StartsWith(Utilities.GetTypeName(theme.ThemeName)))
                {
                    foreach (string panelayout in theme.PaneLayouts.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        selectablePaneLayouts.Add(panelayout, theme.Name + " - " + @Utilities.GetTypeNameLastSegment(panelayout, 0));
                    }
                }
            }
            return selectablePaneLayouts;
        }

        public Dictionary<string, string> GetContainerTypes(List<Theme> themes, string themeName)
        {
            var selectableContainers = new Dictionary<string, string>();
            foreach (Theme theme in themes)
            {
                if (Utilities.GetTypeName(themeName).StartsWith(Utilities.GetTypeName(theme.ThemeName)))
                {
                    foreach (string container in theme.ContainerControls.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        selectableContainers.Add(container, theme.Name + " - " + @Utilities.GetTypeNameLastSegment(container, 0));
                    }
                }
            }
            return selectableContainers;
        }

        public async Task InstallThemesAsync()
        {
            await GetJsonAsync<List<string>>($"{Apiurl}/install");
        }

        public async Task DeleteThemeAsync(string themeName)
        {
            await DeleteAsync($"{Apiurl}/{themeName}");
        }
    }
}

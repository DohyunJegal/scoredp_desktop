using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.AspNetCore.Components.WebView.Wpf;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ScoreDp.Desktop.Data;

namespace ScoreDp.Desktop;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        var services = new ServiceCollection();
        services.AddWpfBlazorWebView();
#if DEBUG
        services.AddBlazorWebViewDeveloperTools();
#endif
        services.AddDbContext<ScoreDpDbContext>(options =>
            options.UseSqlite("Data Source=App_Data/scoredp.db"));

        Resources.Add("services", services.BuildServiceProvider());
    }
}


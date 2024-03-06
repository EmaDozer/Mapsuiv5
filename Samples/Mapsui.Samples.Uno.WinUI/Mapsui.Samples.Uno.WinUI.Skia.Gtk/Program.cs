using GLib;
using Uno.UI.Runtime.Skia.Gtk;

namespace Mapsui.Samples.Uno.WinUI.Skia.Gtk;

class Program
{
    static void Main(string[] args)
    {
        ExceptionManager.UnhandledException += delegate (UnhandledExceptionArgs expArgs)
        {
            Console.WriteLine("GLIB UNHANDLED EXCEPTION" + expArgs.ExceptionObject.ToString());
            expArgs.ExitApplication = true;
        };

        var host = new GtkHost(() => new App());

        host.Run();
    }
}

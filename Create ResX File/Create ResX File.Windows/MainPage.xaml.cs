using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ApplicationSettings;






// Namespace
namespace Create_ResX_File
{





    // App zum erstellen von Resx Dateien
    public sealed partial class MainPage : Page
    {





        // Allgemeine Variabel
        string Fehler = "";





        // Wird am Anfang der Seite geladen
        public MainPage()
        {
            // Componenten laden
            this.InitializeComponent();

            // Seperator erstellen
            SetSeparator.Text = "<X>";

            // Copy Buttons erstellen
            ToTranslateCopy.Content = "<c>";
            TranslatedCopy.Content = "<c>";

            // Flyouts erstellen
            SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;
        }





        // Button zum erstellen des Textes der übersetzt werden muss
        private void Click_CreateTextToTranslate(object sender, RoutedEventArgs e)
        {
            // Prüfen ob Quelldatei erstellt wurde
            if (SourceInput.Text.Length > 1)
            {
                // Neue Datei erstellen
                string SetTextToTranslate = "";

                // Quelldatei versuchen zu zerlegen
                string[] splitSoureInput = Regex.Split(SourceInput.Text, "<value>");
                // Wenn splitSourceInput besteht
                if (splitSoureInput.Count() > 1)
                {
                    // Durchlaufen und erstellen
                    for (int i = 0; i < splitSoureInput.Count(); i++)
                    {
                        string[] splitsplitSource = Regex.Split(splitSoureInput[i], "</value>");
                        if (splitsplitSource.Count() > 1)
                        {
                            SetTextToTranslate += SetSeparator.Text + splitsplitSource[0] + "\r\n";
                        }
                    }

                    // Text zum übersetzen ausgeben
                    TextToTranslate.Text = SetTextToTranslate;
                }
                // Wenn nicht besteht
                else
                {
                    // Qelle löschen
                    SourceInput.Text = "";
                    // Fehler ausgeben
                    Fehler = "No correct .resx file text found\n\n" + Fehler;
                    // Fehler in TextBox schreiben
                    ErrorText.Text = Fehler;
                }
            }
        }





        // Button zum erstellen des Textes der neuen Übersetzungsdatei
        private void Click_CreateTranslatedText(object sender, RoutedEventArgs e)
        {
            // Wenn übersetzte Datei vorhanden
            if (TranslatedText.Text.Length > 0)
            {
                try
                {
                    // Quelldatei laden
                    string Source = SourceInput.Text;


                    // Quelldatei zerlegen
                    string[] splitSource = Regex.Split(Source, "</value>");
                    for (int i = 0; i < splitSource.Count(); i++)
                    {
                        string[] splitSplitSource = Regex.Split(splitSource[i], "<value>");
                        if (splitSplitSource.Count() > 1)
                        {
                            splitSource[i] = splitSplitSource[0];
                        }
                    }



                    // Übersetungsdatei laden
                    string Translation = TranslatedText.Text;
                    Translation = Translation.Replace(SetSeparator.Text.ToLower(), SetSeparator.Text);

                    // Neue Übersetzungsdatei erstellen
                    string NewTranslation = "";
                    // Datei in Linien zerlegen
                    string[] Lines = Regex.Split(Translation, "\r\n");
                    // Übersetzungsdatei durchlaufen
                    for (int i = 0; i < Lines.Count(); i++)
                    {
                        // Line beschneiden
                        Lines[i] = Lines[i].Trim();
                        // Prüfen ob Seperator vorhanden
                        string[] CheckSeperator = Regex.Split(Lines[i], SetSeparator.Text);
                        // Wenn Seperator besteht
                        if (CheckSeperator.Count() > 1)
                        {
                            // Seperator löschen und nach vorne setzen
                            Lines[i] = Lines[i].Replace(SetSeparator.Text, "");
                            Lines[i] = SetSeparator.Text + Lines[i];
                        }
                        // Neue Übersetzungsdatei schreiben
                        NewTranslation += Lines[i] + "\r\n";
                    }
                    Translation = NewTranslation;



                    // Neue Übersetzungsdatei verarbeiten
                    string[] splitTranslation = Regex.Split(Translation, "<X>");
                    for (int i = 0; i < splitTranslation.Count(); i++)
                    {
                        // Übersetzungsdatei bearbeiten
                        splitTranslation[i].Trim();
                    }




                    // Zieldatei erstellen
                    string Target = "";

                    // Zieldatei erstellen
                    for (int i = 0; i < splitSource.Count(); i++)
                    {
                        if (i < splitSource.Count() - 1)
                        {
                            Target += splitSource[i].Trim() + "\r\n<value>" + splitTranslation[i + 1].Trim() + "</value>\r\n";
                        }
                        else
                        {
                            Target += splitSource[i].Trim();
                        }
                    }

                    // Zieldatei erstellen
                    TargetText.Text = Target;
                }
                catch
                {
                    // Fehlermeldung erstellen
                    Fehler = "Failed translation!\nPlease read the source file a new and check your translation.\nIf you continue to have problems, watch instructional video\n\n" + Fehler;
                    // Fehler in TextBox schreiben
                    ErrorText.Text = Fehler;
                }
            }
            // Wenn keine übersetzte Datei vorhanden
            else
            {
                // Fehlermeldung erstellen
                Fehler = "No translated text found\n\n" + Fehler;
                // Fehler in TextBox schreiben
                ErrorText.Text = Fehler;
            }
        }





        // Text zum übersetzen kopieren
        private void ToTranslateCopy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Set the DataPackage to clipboard. 
                DataPackage dataPackage = new DataPackage();
                dataPackage.SetText(TextToTranslate.Text);
                Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);
            }
            catch (Exception)
            {
            }
        }





        // Erstellten Text kopieren
        private void TranslatedCopy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Set the DataPackage to clipboard. 
                DataPackage dataPackage = new DataPackage();
                dataPackage.SetText(TargetText.Text);
                Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(dataPackage);
            }
            catch (Exception)
            {
            }
        }





        // Button Link zur Anleitung
        private async void Click_ButtonManual(object sender, RoutedEventArgs e)
        {
            // Link zur Anleitung
            await Launcher.LaunchUriAsync(new Uri("http://h2236721.stratoserver.net/AppData/Windows/resx_translation_helper/index.html"));
        }

        // Button Link zum Video
        private async void Click_Video(object sender, RoutedEventArgs e)
        {
            // Link zur Anleitung
            await Launcher.LaunchUriAsync(new Uri("https://www.youtube.com/watch?v=NUQFk4vFd4Y"));
        }




        // Flyouts erstellen
        private void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {

            args.Request.ApplicationCommands.Add(new SettingsCommand(
                "Privacy policy", "Privacy policy", (handler) => ShowCustomSettingFlyout()));
        }

        public void ShowCustomSettingFlyout()
        {
            PrivacyPolice PrivacyPoliceFlyout = new PrivacyPolice();
            PrivacyPoliceFlyout.Show();
        }




       
    }
}

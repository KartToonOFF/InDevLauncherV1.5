using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace InDevLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string gamePath { get; set; }
        string pathInDev = $@"{Environment.GetFolderPath(Environment.SpecialFolder.UserProfile)}\AppData\Roaming\IndevLauncher\";

#pragma warning disable CS8618
        public MainWindow()
#pragma warning restore CS8618
        {
            InitializeComponent();

            gamename.Content = "Selectionne un logiciel";
            IMG_INDEV.Visibility = Visibility.Visible;

            HideAddGameForm();

            ReadGameAtStart();

            SetActu();



        }

        public void SetActu()
        {
            string url = "https://pastebin.com/raw/eFEpfFzC";
            HttpClient client = new HttpClient();
            {
                using (HttpResponseMessage reponse = client.GetAsync(url).Result)
                {
                    using (HttpContent content = reponse.Content)
                    {
                        var json = content.ReadAsStringAsync().Result;
                        txt.Text=json;
                    }
                }
            }
        }

        public void ReadGameAtStart()
        {
            try
            {
                string[] lines = File.ReadAllLines($@"{pathInDev}Data.txt");
                foreach (string s in lines)
                {
                    if (s.StartsWith("- "))
                    {
                        string[] gamedata_path = s.Split("|");
                        combo_b_game.Items.Add(gamedata_path[0]);
                    }
                }
            }
            catch
            {
                Directory.CreateDirectory(pathInDev);
                File.WriteAllText($@"{pathInDev}Data.txt", $"");
                MessageBox.Show("Création du fichier Data...");
            }
        }



        private void play_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Process.Start(gamePath);
            }
            catch
            {
                MessageBoxResult result = MessageBox.Show($"Le Chemin vers l'executable n'est pas correct :\n - Vous pouvez le Modifier dans le dossier :\n\nC:/Users/{Environment.UserName}/AppData/Roaming/IndevLauncher/Data.txt\n\n - Puis redermarer le launcher en attente de Mise a jour sur le sujet...\nOuvrir l'explorer a cette endroit ?", "ExePath invalide", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        string unevar = $@"{pathInDev}";
                        Process.Start("explorer.exe", string.Format("/select,\"{0}\"", unevar));
                        break;

                    case MessageBoxResult.No:
                        break;
                }
            }

        }


        public void HideAddGameForm()
        {
            NomDuJeu.Visibility = Visibility.Hidden;
            PathToExe.Visibility = Visibility.Hidden;
            GAME_NAME_FIELD.Visibility = Visibility.Hidden;
            PATH_TO_EXE_FIELD.Visibility = Visibility.Hidden;
            Btn_save.Visibility = Visibility.Hidden;

        }

        public void ShowAddGameForm()
        {
            NomDuJeu.Visibility = Visibility.Visible;
            PathToExe.Visibility = Visibility.Visible;
            GAME_NAME_FIELD.Visibility = Visibility.Visible;
            PATH_TO_EXE_FIELD.Visibility = Visibility.Visible;
            Btn_save.Visibility = Visibility.Visible;

        }

        private void Btn_Add_Game_Click(object sender, RoutedEventArgs e)
        {
            GAME_NAME_FIELD.Clear();
            PATH_TO_EXE_FIELD.Clear();
            ShowAddGameForm();
        }


        private void Btn_save_Click(object sender, RoutedEventArgs e)
        {
            string GameName = GAME_NAME_FIELD.Text;
            string GamePath = PATH_TO_EXE_FIELD.Text;

            HideAddGameForm();

            string gameData = $@"{pathInDev}Data.txt";
            File.AppendAllText(gameData, $"- {GameName} -|{GamePath}\n");

            combo_b_game.Items.Add($"- {GameName} -");

        }


        private void combo_b_game_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Object selectedItem = combo_b_game.SelectedItem;

            string[] lines = File.ReadAllLines($@"{pathInDev}Data.txt"); //ya une erreur quand on supprime un element dans GameData.txt (soit refresh la page chaque seconde / soit fermer le launcher et le redemarer)
            foreach (string s in lines)
            {
#pragma warning disable CS8604
                if (s.StartsWith(Convert.ToString(selectedItem)))
                {
                    string[] gamedata_path = s.Split("|");
                    path_to_exe.Content = "Chemin de l'exe : " + gamedata_path[1];
                    gamename.Content = gamedata_path[0];
                    gamePath = gamedata_path[1];


                }
#pragma warning restore CS8604
            }
        }

        private void Btn_Edit_Game_Click(object sender, RoutedEventArgs e)
        {

        }

        private void closebutton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void closebutton_Click_1(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                this.DragMove();
            }
        }
    }
}

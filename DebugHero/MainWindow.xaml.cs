using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using System.Windows.Media;

namespace DeveloperLand
{
    public partial class MainWindow : Window
    {
        public enum KarStatus { Actief, InOnderhoud, NietActief }
        public KarStatus[] DebugHeroKarren = new KarStatus[3] { KarStatus.Actief, KarStatus.Actief, KarStatus.Actief };

        public MainWindow()
        {
            InitializeComponent();
            UpdateKarStatusLabels();
        }

        public int BerekenDebugHeroWachttijd(string sensorXmlPath)
        {
            int wachttijd = 0;
            XmlDocument doc = new XmlDocument();
            doc.Load(sensorXmlPath);
            for (int i = 1; i <= 12; i++)
            {
                string sensorNaam = $"Sensor{(i < 10 ? "0" : "")}{i}";
                var node = doc.SelectSingleNode($"//{sensorNaam}");
                if (node != null && node.InnerText.Trim().ToLower() == "true")
                {
                    if (i == 1 || i == 2)
                        wachttijd += 6;
                    else
                        wachttijd += 4;
                }
            }
            return wachttijd;
        }

        public void ZetKarInOnderhoud(int karIndex)
        {
            if (karIndex >= 0 && karIndex < DebugHeroKarren.Length)
                DebugHeroKarren[karIndex] = KarStatus.InOnderhoud;
        }
        public void ZetKarActief(int karIndex)
        {
            if (karIndex >= 0 && karIndex < DebugHeroKarren.Length)
                DebugHeroKarren[karIndex] = KarStatus.Actief;
        }
        public void ZetKarNietActief(int karIndex)
        {
            if (karIndex >= 0 && karIndex < DebugHeroKarren.Length)
                DebugHeroKarren[karIndex] = KarStatus.NietActief;
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            string sensorXmlPath = "Assets/SensorData/WachtrijSensoren.xml";
            int wachttijd = BerekenDebugHeroWachttijd(sensorXmlPath);
            WachttijdLabel.Text = $"{wachttijd} minuten";
            UpdateKarStatusLabels();
        }

        private void Kar1StatusBtn_Click(object sender, RoutedEventArgs e)
        {
            WisselKarStatus(0, Kar1StatusBtn);
        }
        private void Kar2StatusBtn_Click(object sender, RoutedEventArgs e)
        {
            WisselKarStatus(1, Kar2StatusBtn);
        }
        private void Kar3StatusBtn_Click(object sender, RoutedEventArgs e)
        {
            WisselKarStatus(2, Kar3StatusBtn);
        }

        private void WisselKarStatus(int index, Button btn)
        {
            if (DebugHeroKarren[index] == KarStatus.Actief)
                ZetKarInOnderhoud(index);
            else if (DebugHeroKarren[index] == KarStatus.InOnderhoud)
                ZetKarActief(index);
            else
                ZetKarActief(index);
            UpdateKarStatusLabels();
        }

        private void UpdateKarStatusLabels()
        {
            UpdateKarStatus(0, Kar1StatusLabel, Kar1StatusBtn, Kar1Border);
            UpdateKarStatus(1, Kar2StatusLabel, Kar2StatusBtn, Kar2Border);
            UpdateKarStatus(2, Kar3StatusLabel, Kar3StatusBtn, Kar3Border);
        }

        private void UpdateKarStatus(int index, TextBlock statusLabel, Button statusBtn, Border karBorder)
        {
            switch (DebugHeroKarren[index])
            {
                case KarStatus.Actief:
                    statusLabel.Text = "Actief";
                    statusLabel.Foreground = new SolidColorBrush(Color.FromRgb(56, 142, 60)); // groen
                    statusBtn.Content = "Zet in onderhoud";
                    karBorder.Background = new SolidColorBrush(Color.FromRgb(227, 242, 253)); // lichtblauw
                    break;
                case KarStatus.InOnderhoud:
                    statusLabel.Text = "In onderhoud";
                    statusLabel.Foreground = new SolidColorBrush(Color.FromRgb(251, 192, 45)); // geel
                    statusBtn.Content = "Zet actief";
                    karBorder.Background = new SolidColorBrush(Color.FromRgb(255, 235, 59)); // geel
                    break;
                case KarStatus.NietActief:
                    statusLabel.Text = "Niet actief";
                    statusLabel.Foreground = new SolidColorBrush(Color.FromRgb(97, 97, 97)); // grijs
                    statusBtn.Content = "Zet actief";
                    karBorder.Background = new SolidColorBrush(Color.FromRgb(224, 224, 224)); // grijs
                    break;
            }
        }
    }
}
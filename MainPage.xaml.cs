namespace SpainQuotes
{
    public partial class MainPage : ContentPage
    {
        private List<string> quotes;
        private Random random;

        public MainPage()
        {
            InitializeComponent();
            quotes = new List<string>();
            random = new Random();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadQuotesAsync();
        }

        private async Task LoadQuotesAsync()
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync("quotes.txt");
                using var reader = new StreamReader(stream);
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    quotes.Add(line);
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores (puedes mostrar un mensaje al usuario si lo deseas)
                Console.WriteLine($"Error loading quotes: {ex.Message}");
            }
        }

        private void btnGenerateQuote_Clicked(object sender, EventArgs e)
        {
            if (quotes.Count == 0)
            {
                quote.Text = "No quotes available.";
                return;
            }

            SetRandomBackgroundGradient();

            int index = random.Next(quotes.Count);
            quote.Text = quotes[index];
        }

        private void SetRandomBackgroundGradient()
        {
            var startColor = GenerateRandomColor();
            var endColor = GenerateRandomColor();
            var colors = ColorUtility.ColorControls.GetColorGradient(startColor, endColor, 6);

            var stops = new GradientStopCollection();
            float stopOffset = 0.0f;
            foreach (var color in colors)
            {
                stops.Add(new GradientStop(Color.FromArgb(color.Name), stopOffset));
                stopOffset += 0.2f;
            }

            var gradient = new LinearGradientBrush(stops, new Point(0, 0), new Point(1, 1));
            background.Background = gradient;
        }

        private System.Drawing.Color GenerateRandomColor()
        {
            return System.Drawing.Color.FromArgb(
                random.Next(0, 256),
                random.Next(0, 256),
                random.Next(0, 256));
        }
    }
}

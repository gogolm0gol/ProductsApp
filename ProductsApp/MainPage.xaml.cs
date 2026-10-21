using System.Collections.ObjectModel;
using System.Globalization;

namespace ProductsApp
{
    public partial class MainPage : ContentPage
    {
        private ObservableCollection<Product> products = new ObservableCollection<Product>();

        public MainPage()
        {
            InitializeComponent();

            productsList.ItemsSource = products;
            pickType.SelectedIndex = 0;
            dpPack.Date = DateTime.Today;
            dpExpiry.Date = DateTime.Today.AddDays(30);

            FillSampleData();
        }

        private void FillSampleData()
        {
            products.Add(new Food("Молоко", 42.50m, "Україна", DateTime.Today.AddDays(-2),
                "Пастеризоване 2,5%.", DateTime.Today.AddDays(8), 1, "л"));
            products.Add(new Food("Яблука", 35m, "Польща", DateTime.Today.AddDays(-5),
                "Сорт Гала.", DateTime.Today.AddDays(25), 2, "кг"));
            products.Add(new Food("Сир твердий", 280m, "Нідерланди", DateTime.Today.AddDays(-10),
                "Витриманий.", DateTime.Today.AddDays(90), 0.5m, "кг"));
            products.Add(new Book("Кобзар", 320m, "Україна", DateTime.Today.AddDays(-60),
                "Збірка поезій.", 704, "Фоліо", new List<string> { "Тарас Шевченко" }));
            products.Add(new Book("Лісова пісня", 180m, "Україна", DateTime.Today.AddDays(-90),
                "Драма-феєрія.", 160, "Основи", new List<string> { "Леся Українка" }));
        }

        private void OnTypeChanged(object sender, EventArgs e)
        {
            bool isFood = pickType.SelectedIndex == 0;
            foodPanel.IsVisible = isFood;
            bookPanel.IsVisible = !isFood;
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(entName.Text))
                {
                    await DisplayAlert("Помилка", "Ви не ввели назву.", "OK");
                    return;
                }
                if (string.IsNullOrWhiteSpace(entPrice.Text))
                {
                    await DisplayAlert("Помилка", "Ви не ввели ціну.", "OK");
                    return;
                }
                if (!TryParseDecimal(entPrice.Text, out decimal price))
                {
                    await DisplayAlert("Помилка", "Ціна має бути числом, наприклад 25.50", "OK");
                    return;
                }

                DateTime packDate = ToDate(dpPack.Date);
                Product product;

                if (pickType.SelectedIndex == 0)
                {
                    if (string.IsNullOrWhiteSpace(entQuantity.Text))
                    {
                        await DisplayAlert("Помилка", "Ви не ввели кількість.", "OK");
                        return;
                    }
                    if (!TryParseDecimal(entQuantity.Text, out decimal quantity))
                    {
                        await DisplayAlert("Помилка", "Кількість має бути числом, наприклад 1.5", "OK");
                        return;
                    }

                    product = new Food(entName.Text, price, entCountry.Text ?? "", packDate,
                        entDescription.Text ?? "", ToDate(dpExpiry.Date), quantity, entUnit.Text ?? "");
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(entPages.Text))
                    {
                        await DisplayAlert("Помилка", "Ви не ввели кількість сторінок.", "OK");
                        return;
                    }
                    if (!int.TryParse(entPages.Text.Trim(), NumberStyles.None,
                            CultureInfo.InvariantCulture, out int pages))
                    {
                        await DisplayAlert("Помилка", "Кількість сторінок має бути цілим числом.", "OK");
                        return;
                    }

                    List<string> authors = (entAuthors.Text ?? "")
                        .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                        .ToList();

                    product = new Book(entName.Text, price, entCountry.Text ?? "", packDate,
                        entDescription.Text ?? "", pages, entPublisher.Text ?? "", authors);
                }

                products.Add(product);
                ClearForm();
            }
            catch (ArgumentException ex)
            {
                await DisplayAlert("Помилка", ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Несподівана помилка", ex.Message, "OK");
            }
        }

        private async void OnDeleteClicked(object sender, EventArgs e)
        {
            if (productsList.SelectedItem is not Product selected)
            {
                await DisplayAlert("Увага", "Спочатку оберіть товар у таблиці.", "OK");
                return;
            }

            bool confirm = await DisplayAlert("Видалення",
                $"Видалити \"{selected.Name}\"?", "Так", "Ні");
            if (!confirm) return;

            products.Remove(selected);
            productsList.SelectedItem = null;
        }

        private void ClearForm()
        {
            entName.Text = "";
            entPrice.Text = "";
            entCountry.Text = "";
            entDescription.Text = "";
            entQuantity.Text = "";
            entUnit.Text = "";
            entPages.Text = "";
            entPublisher.Text = "";
            entAuthors.Text = "";
        }

        private static bool TryParseDecimal(string? text, out decimal value)
        {
            value = 0;
            if (string.IsNullOrWhiteSpace(text)) return false;
            return decimal.TryParse(text.Trim().Replace(',', '.'),
                NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out value);
        }

        private static DateTime ToDate(object? value)
        {
            return value is DateTime d ? d : DateTime.Today;
        }
    }
}
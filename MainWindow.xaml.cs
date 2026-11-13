using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using AutoServiceManager.Models;
using AutoServiceManager.Services;

namespace AutoServiceManager;

public partial class MainWindow : Window
{
    private readonly JsonDataService _dataService = new();
    public ObservableCollection<Client> Clients { get; } = new();
    public ObservableCollection<Vehicle> Vehicles { get; } = new();
    public ObservableCollection<RepairOrder> Orders { get; } = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        LoadData();
        RefreshControls();
        UpdateDashboard();
        ShowPage("Home");
    }

    private void LoadData()
    {
        foreach (var item in _dataService.Load<Client>("clients")) Clients.Add(item);
        foreach (var item in _dataService.Load<Vehicle>("vehicles")) Vehicles.Add(item);
        foreach (var item in _dataService.Load<RepairOrder>("orders")) Orders.Add(item);

        // Заполняем приложение демонстрационными данными только при первом запуске.
        // Если пользователь уже работает со своими данными, они не перезаписываются.
        if (Clients.Count == 0 && Vehicles.Count == 0 && Orders.Count == 0)
        {
            CreateDemoData();
            SaveData();
        }

        RefreshVehicleOwners();
    }


    private void CreateDemoData()
    {
        var clients = new[]
        {
            new Client { FullName = "Александр Иванов", Phone = "+7 (900) 111-22-33", Email = "ivanov@example.com" },
            new Client { FullName = "Дмитрий Смирнов", Phone = "+7 (900) 222-33-44", Email = "smirnov@example.com" },
            new Client { FullName = "Елена Кузнецова", Phone = "+7 (900) 333-44-55", Email = "kuznetsova@example.com" },
            new Client { FullName = "Сергей Попов", Phone = "+7 (900) 444-55-66", Email = "popov@example.com" },
            new Client { FullName = "Максим Соколов", Phone = "+7 (900) 555-66-77", Email = "sokolov@example.com" },
            new Client { FullName = "Анна Лебедева", Phone = "+7 (900) 666-77-88", Email = "lebedeva@example.com" },
            new Client { FullName = "Андрей Волков", Phone = "+7 (900) 777-88-99", Email = "volkov@example.com" },
            new Client { FullName = "Михаил Морозов", Phone = "+7 (900) 888-99-00", Email = "morozov@example.com" }
        };
        foreach (var client in clients) Clients.Add(client);

        var vehicles = new[]
        {
            new Vehicle { ClientId = clients[0].Id, Brand = "Toyota", Model = "Camry", RegistrationNumber = "А123ВС750", Year = 2018 },
            new Vehicle { ClientId = clients[0].Id, Brand = "Kia", Model = "Rio", RegistrationNumber = "В456ОР750", Year = 2016 },
            new Vehicle { ClientId = clients[1].Id, Brand = "Volkswagen", Model = "Polo", RegistrationNumber = "Е789КХ790", Year = 2019 },
            new Vehicle { ClientId = clients[2].Id, Brand = "Hyundai", Model = "Solaris", RegistrationNumber = "К321МН750", Year = 2020 },
            new Vehicle { ClientId = clients[3].Id, Brand = "Lada", Model = "Vesta", RegistrationNumber = "М654АВ790", Year = 2021 },
            new Vehicle { ClientId = clients[4].Id, Brand = "BMW", Model = "3 Series", RegistrationNumber = "О987ТУ750", Year = 2015 },
            new Vehicle { ClientId = clients[5].Id, Brand = "Skoda", Model = "Octavia", RegistrationNumber = "Р147ЕС790", Year = 2017 },
            new Vehicle { ClientId = clients[6].Id, Brand = "Ford", Model = "Focus", RegistrationNumber = "С258НК750", Year = 2014 },
            new Vehicle { ClientId = clients[7].Id, Brand = "Renault", Model = "Duster", RegistrationNumber = "Т369РМ790", Year = 2022 },
            new Vehicle { ClientId = clients[2].Id, Brand = "Nissan", Model = "Qashqai", RegistrationNumber = "У741ХС750", Year = 2019 }
        };
        foreach (var vehicle in vehicles)
        {
            vehicle.OwnerName = clients.First(c => c.Id == vehicle.ClientId).FullName;
            Vehicles.Add(vehicle);
        }

        var today = DateTime.Today;
        var orders = new[]
        {
            new RepairOrder { ClientId = clients[0].Id, VehicleId = vehicles[0].Id, ProblemDescription = "Посторонний шум в передней подвеске", WorkDescription = "Диагностика ходовой части, замена стоек стабилизатора", Cost = 8500m, Status = "В работе", CreatedAt = today.AddDays(-1) },
            new RepairOrder { ClientId = clients[1].Id, VehicleId = vehicles[2].Id, ProblemDescription = "Плановое техническое обслуживание", WorkDescription = "Замена масла и масляного фильтра", Cost = 6200m, Status = "Завершён", CreatedAt = today.AddDays(-5) },
            new RepairOrder { ClientId = clients[2].Id, VehicleId = vehicles[3].Id, ProblemDescription = "Неустойчивая работа двигателя", WorkDescription = "Компьютерная диагностика двигателя", Cost = 2500m, Status = "Новый", CreatedAt = today },
            new RepairOrder { ClientId = clients[3].Id, VehicleId = vehicles[4].Id, ProblemDescription = "Скрип при торможении", WorkDescription = "Замена передних тормозных колодок", Cost = 7400m, Status = "В работе", CreatedAt = today.AddDays(-2) },
            new RepairOrder { ClientId = clients[4].Id, VehicleId = vehicles[5].Id, ProblemDescription = "Течь охлаждающей жидкости", WorkDescription = "Поиск утечки и замена патрубка", Cost = 5800m, Status = "Завершён", CreatedAt = today.AddDays(-8) },
            new RepairOrder { ClientId = clients[5].Id, VehicleId = vehicles[6].Id, ProblemDescription = "Вибрация на высокой скорости", WorkDescription = "Проверка и балансировка колёс", Cost = 3600m, Status = "Новый", CreatedAt = today.AddDays(-0) },
            new RepairOrder { ClientId = clients[6].Id, VehicleId = vehicles[7].Id, ProblemDescription = "Сложный запуск двигателя", WorkDescription = "Проверка аккумулятора и системы запуска", Cost = 1800m, Status = "В работе", CreatedAt = today.AddDays(-3) },
            new RepairOrder { ClientId = clients[7].Id, VehicleId = vehicles[8].Id, ProblemDescription = "Замена расходных материалов", WorkDescription = "Замена воздушного и салонного фильтров", Cost = 3100m, Status = "Завершён", CreatedAt = today.AddDays(-10) },
            new RepairOrder { ClientId = clients[2].Id, VehicleId = vehicles[9].Id, ProblemDescription = "Горит индикатор Check Engine", WorkDescription = "Считывание ошибок и диагностика", Cost = 2200m, Status = "Новый", CreatedAt = today.AddDays(-0) },
            new RepairOrder { ClientId = clients[0].Id, VehicleId = vehicles[1].Id, ProblemDescription = "Подготовка автомобиля к сезону", WorkDescription = "Проверка технических жидкостей и тормозной системы", Cost = 4500m, Status = "Завершён", CreatedAt = today.AddDays(-14) }
        };
        foreach (var order in orders)
        {
            var client = clients.First(c => c.Id == order.ClientId);
            var vehicle = vehicles.First(v => v.Id == order.VehicleId);
            order.ClientName = client.FullName;
            order.VehicleName = vehicle.DisplayName;
            Orders.Add(order);
        }
    }

    private void SaveData()
    {
        _dataService.Save("clients", Clients);
        _dataService.Save("vehicles", Vehicles);
        _dataService.Save("orders", Orders);
    }

    private void RefreshControls()
    {
        VehicleClientBox.ItemsSource = Clients;
        OrderClientBox.ItemsSource = Clients;
        if (Clients.Count > 0 && VehicleClientBox.SelectedIndex < 0) VehicleClientBox.SelectedIndex = 0;
        if (Clients.Count > 0 && OrderClientBox.SelectedIndex < 0) OrderClientBox.SelectedIndex = 0;
        RefreshVehicleOwners();
    }

    private void RefreshVehicleOwners()
    {
        foreach (var vehicle in Vehicles)
            vehicle.OwnerName = Clients.FirstOrDefault(c => c.Id == vehicle.ClientId)?.FullName ?? "Клиент удалён";
        VehiclesGrid?.Items.Refresh();
        RefreshOrderVehicleList();
    }

    private void RefreshOrderVehicleList()
    {
        if (OrderClientBox?.SelectedItem is Client selected)
        {
            OrderVehicleBox.ItemsSource = Vehicles.Where(v => v.ClientId == selected.Id).ToList();
            if (OrderVehicleBox.Items.Count > 0) OrderVehicleBox.SelectedIndex = 0;
        }
        else
        {
            OrderVehicleBox.ItemsSource = null;
        }
    }

    private void UpdateDashboard()
    {
        ClientsCount.Text = Clients.Count.ToString();
        VehiclesCount.Text = Vehicles.Count.ToString();
        OrdersCount.Text = Orders.Count.ToString();
    }

    private void ShowPage(string page)
    {
        HomePage.Visibility = page == "Home" ? Visibility.Visible : Visibility.Collapsed;
        ClientsPage.Visibility = page == "Clients" ? Visibility.Visible : Visibility.Collapsed;
        VehiclesPage.Visibility = page == "Vehicles" ? Visibility.Visible : Visibility.Collapsed;
        OrdersPage.Visibility = page == "Orders" ? Visibility.Visible : Visibility.Collapsed;
        AboutPage.Visibility = page == "About" ? Visibility.Visible : Visibility.Collapsed;

        (PageTitle.Text, PageSubtitle.Text) = page switch
        {
            "Home" => ("Главная", "Краткая информация о работе автосервиса"),
            "Clients" => ("Клиенты", "Учёт заказчиков автосервиса"),
            "Vehicles" => ("Автомобили", "Транспортные средства клиентов"),
            "Orders" => ("Заказы на ремонт", "Создание и контроль заказов"),
            _ => ("О программе", "Информация о проекте")
        };
        UpdateDashboard();
    }

    private void Navigation_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string page) ShowPage(page);
    }

    private void AddClient_Click(object sender, RoutedEventArgs e)
    {
        string name = ClientNameBox.Text.Trim();
        string phone = ClientPhoneBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone))
        {
            MessageBox.Show("Укажите ФИО и телефон клиента.", "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        Clients.Add(new Client { FullName = name, Phone = phone, Email = ClientEmailBox.Text.Trim() });
        ClientNameBox.Clear(); ClientPhoneBox.Clear(); ClientEmailBox.Clear();
        SaveData(); RefreshControls(); UpdateDashboard();
    }

    private void DeleteClient_Click(object sender, RoutedEventArgs e)
    {
        if (ClientsGrid.SelectedItem is not Client client)
        {
            MessageBox.Show("Выберите клиента в таблице."); return;
        }
        if (Vehicles.Any(v => v.ClientId == client.Id) || Orders.Any(o => o.ClientId == client.Id))
        {
            MessageBox.Show("Нельзя удалить клиента, пока с ним связаны автомобили или заказы.", "Удаление", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }
        Clients.Remove(client); SaveData(); RefreshControls(); UpdateDashboard();
    }

    private void AddVehicle_Click(object sender, RoutedEventArgs e)
    {
        if (VehicleClientBox.SelectedItem is not Client client)
        {
            MessageBox.Show("Сначала добавьте и выберите клиента."); return;
        }
        string brand = VehicleBrandBox.Text.Trim();
        string model = VehicleModelBox.Text.Trim();
        string number = VehicleNumberBox.Text.Trim().ToUpperInvariant();
        if (brand.Length == 0 || model.Length == 0 || number.Length == 0)
        {
            MessageBox.Show("Заполните марку, модель и государственный номер."); return;
        }
        if (Vehicles.Any(v => string.Equals(v.RegistrationNumber, number, StringComparison.OrdinalIgnoreCase)))
        {
            MessageBox.Show("Автомобиль с таким номером уже существует."); return;
        }
        if (!int.TryParse(VehicleYearBox.Text.Trim(), out int year) || year < 1900 || year > DateTime.Now.Year + 1)
        {
            MessageBox.Show("Укажите корректный год выпуска."); return;
        }
        Vehicles.Add(new Vehicle { ClientId = client.Id, OwnerName = client.FullName, Brand = brand, Model = model, RegistrationNumber = number, Year = year });
        VehicleBrandBox.Clear(); VehicleModelBox.Clear(); VehicleNumberBox.Clear(); VehicleYearBox.Clear();
        SaveData(); RefreshOrderVehicleList(); UpdateDashboard();
    }

    private void DeleteVehicle_Click(object sender, RoutedEventArgs e)
    {
        if (VehiclesGrid.SelectedItem is not Vehicle vehicle)
        {
            MessageBox.Show("Выберите автомобиль в таблице."); return;
        }
        if (Orders.Any(o => o.VehicleId == vehicle.Id))
        {
            MessageBox.Show("Нельзя удалить автомобиль, если по нему существуют заказы."); return;
        }
        Vehicles.Remove(vehicle); SaveData(); RefreshOrderVehicleList(); UpdateDashboard();
    }

    private void OrderClientBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (IsInitialized) RefreshOrderVehicleList();
    }

    private void AddOrder_Click(object sender, RoutedEventArgs e)
    {
        if (OrderClientBox.SelectedItem is not Client client || OrderVehicleBox.SelectedItem is not Vehicle vehicle)
        {
            MessageBox.Show("Выберите клиента и его автомобиль."); return;
        }
        if (string.IsNullOrWhiteSpace(ProblemBox.Text) || string.IsNullOrWhiteSpace(WorkBox.Text))
        {
            MessageBox.Show("Укажите неисправность и планируемые работы."); return;
        }
        if (!decimal.TryParse(CostBox.Text.Trim().Replace(',', '.'), System.Globalization.NumberStyles.Number,
            System.Globalization.CultureInfo.InvariantCulture, out decimal cost) || cost < 0)
        {
            MessageBox.Show("Стоимость должна быть неотрицательным числом."); return;
        }
        Orders.Add(new RepairOrder
        {
            ClientId = client.Id, VehicleId = vehicle.Id,
            ClientName = client.FullName, VehicleName = vehicle.DisplayName,
            ProblemDescription = ProblemBox.Text.Trim(), WorkDescription = WorkBox.Text.Trim(),
            Cost = cost, Status = "Новый", CreatedAt = DateTime.Now
        });
        ProblemBox.Clear(); WorkBox.Clear(); CostBox.Clear();
        SaveData(); UpdateDashboard();
    }

    private void DeleteOrder_Click(object sender, RoutedEventArgs e)
    {
        if (OrdersGrid.SelectedItem is not RepairOrder order)
        {
            MessageBox.Show("Выберите заказ в таблице."); return;
        }
        Orders.Remove(order); SaveData(); UpdateDashboard();
    }

    private void CompleteOrder_Click(object sender, RoutedEventArgs e)
    {
        if (OrdersGrid.SelectedItem is not RepairOrder order)
        {
            MessageBox.Show("Выберите заказ в таблице."); return;
        }
        order.Status = "Завершён";
        OrdersGrid.Items.Refresh();
        SaveData();
    }
}

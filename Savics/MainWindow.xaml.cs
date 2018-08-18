using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Gu.Wpf.NumericInput;

namespace Savics
{
    public class Record
    {
        public int Id;
        public string FirstName;
        public string LastName;
        public string Sex;
        public string Country;
        public string City;
        public short Age;
        public string LivingWD;

        public override string ToString()
        {
            return this.FirstName + " " + this.LastName + " (" + this.Sex + ") " + this.Age.ToString() + " - " + this.City + " (" + this.Country + ")";
        }
    }

    public class Country
    {
        public int Id;
        public string Name;
        public List<City> Cities;
    }

    public class City
    {
        public int Id;
        public string Name;
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        


        private List<Record> Records = new List<Record>();

        private int UpdateId = 0;
        private List<Country> Countries = new List<Country>
        {
            new Country{
                Id = 1,
                Name = "Burkina Faso",
                Cities = new List<City> {
                    new City{ Id = 1, Name = "Ouagadougou"},
                    new City{ Id = 2, Name = "Bobo Dioulasso"},
                }
            },
            new Country{
                Id = 2,
                Name = "Cote d'Ivoire",
                Cities = new List<City> {
                    new City{ Id = 3, Name = "Abidjan"},
                    new City{ Id = 4, Name = "Yamoussoukro"},
                }
            }
        };
        

        public MainWindow()
        {       
            InitializeComponent();           
            Records.Add(
                new Record(){
                    Id = 1,
                    Country = "Burkina Faso",
                    City = "Ouagadougou",
                    FirstName = "Mohamed",
                    LastName = "KONE",
                    Sex = "Male",
                    LivingWD = "No",
                    Age = 27,
                }
            );
            ICollectionView view = CollectionViewSource.GetDefaultView(Records);            
            RecordsList.ItemsSource = Records;
            view.Refresh();
            Country.ItemsSource = Countries.Select(c => c.Name);
            Country.SelectedIndex = 0;
            if (!String.IsNullOrWhiteSpace(Country.Text))
            {
                City.ItemsSource = Countries.First(c => c.Name == Country.Text).Cities.Select(c => c.Name);
                City.SelectedIndex = 0;
            } 
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (UpdateId == 0)
            {
                Record New = new Record
                {
                    Id = Records.Count() + 1,
                    Age = Convert.ToInt16(Age.Text),
                    City = City.Text,
                    Country = Country.Text,
                    FirstName = FirstName.Text,
                    LastName = LastName.Text,
                    LivingWD = LWDN.IsChecked.Value ? "No" : LWDY.IsChecked.Value ? "Yes" : "Unknown",
                    Sex = Male.IsChecked.Value ? "Male" : "Female"
                };

                Records.Add(New);
            }
            else
            {
                Record New = new Record
                {
                    Id = Records.Count() + 1,
                    Age = Convert.ToInt16(Age.Text),
                    City = City.Text,
                    Country = Country.Text,
                    FirstName = FirstName.Text,
                    LastName = LastName.Text,
                    LivingWD = LWDN.IsChecked.Value ? "No" : LWDY.IsChecked.Value ? "Yes" : "Unknown",
                    Sex = Male.IsChecked.Value ? "Male" :  "Female" 
                };
                var ToEdit = Records.First(r=>r.Id == UpdateId);
                ToEdit.Age = New.Age;
                ToEdit.City = New.City;
                ToEdit.Country = New.Country;
                ToEdit.FirstName = New.FirstName;
                ToEdit.LastName = New.LastName;
                ToEdit.LivingWD = New.LivingWD;
                ToEdit.Sex = New.Sex;
            }
            ICollectionView view = CollectionViewSource.GetDefaultView(Records);
            RecordsList.ItemsSource = Records;
            view.Refresh();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            UpdateId = 0;
            FirstName.Text = "";
            LastName.Text = "";
            this.Title = Title.Replace(" *", "");
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if(RecordsList.SelectedIndex > -1) LoadRecord(RecordsList.SelectedIndex);
            this.Title += " *";
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            if (RecordsList != null)
            {
                ICollectionView view = CollectionViewSource.GetDefaultView(Records);
                RecordsList.ItemsSource = Records.Where(r => r.Age < 18);
                view.Refresh();
            }
                
        }

        private void Minor_UnChecked(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(Records);
            RecordsList.ItemsSource = Records;
            view.Refresh();
        }

        private void Country_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            City.ItemsSource = Countries.First(c => c.Name == e.AddedItems[0].ToString()).Cities.Select(c => c.Name);
        }

        private void Minor_Checked(object sender, RoutedEventArgs e)
        {
            ICollectionView view = CollectionViewSource.GetDefaultView(Records);
            RecordsList.ItemsSource = Records.Where(r => r.Age < 18);
            view.Refresh();
        }




        private void LoadRecord(int id)
        {
            var ToEdit = Records.First(r => r.Id == id +1);
            FirstName.Text = ToEdit.FirstName;
            Age.Text = ToEdit.Age.ToString();
            LastName.Text = ToEdit.LastName;
            var C = Countries.First(c => c.Name == ToEdit.Country);
            Country.SelectedItem = C;
            City.SelectedItem = C.Cities.First(c => c.Name == ToEdit.City);
            if (ToEdit.Sex == "Female") Female.IsChecked = true; else Male.IsChecked = true;
            if (ToEdit.LivingWD == "Yes") LWDY.IsChecked = true; else if (ToEdit.LivingWD == "No") LWDN.IsChecked = true; else LWDU.IsChecked = true;
            UpdateId = ToEdit.Id;
        }
    }
}

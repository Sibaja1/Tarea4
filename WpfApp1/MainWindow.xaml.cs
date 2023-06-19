using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using WpfApp;
using static WpfApp1.MainWindow;


namespace WpfApp1
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<Person> people;
        private Person selectedPerson;

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Person> People
        {
            get { return people; }
            set
            {
                people = value;
                NotifyPropertyChanged();
            }
        }


        public Person SelectedPerson
        {
            get { return selectedPerson; }
            set
            {
                selectedPerson = value;
                NotifyPropertyChanged();
            }
        }
        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            People = new ObservableCollection<Person>();
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string idText = idTextBox.Text;
            string name = nameTextBox.Text;
            string ageText = ageTextBox.Text;
            string email = emailTextBox.Text;

            // Validación de datos
            if (string.IsNullOrEmpty(idText) || string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(ageText) || !int.TryParse(ageText, out int age) || !int.TryParse(idText, out int id))
            {
                MessageBox.Show("Por favor, ingrese todos los datos correctamente.");
                return;
            }

            Person person = new Person(id, name, age, email);
            People.Add(person);

            // Limpiar los campos de entrada
            limpiar();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPerson != null)
            {
                // Validar los campos de entrada antes de realizar la modificación
                if (string.IsNullOrEmpty(idTextBox.Text) || string.IsNullOrEmpty(nameTextBox.Text) || string.IsNullOrEmpty(ageTextBox.Text) || string.IsNullOrEmpty(emailTextBox.Text))
                {
                    MessageBox.Show("Por favor, complete todos los campos antes de modificar los datos de la persona.", "Error de validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Obtener el índice de la persona seleccionada en la lista
                int index = People.IndexOf(SelectedPerson);

                // Crear una nuevo objeto de Person con los datos modificados
                Person updatedPerson = new Person
                (
                    Convert.ToInt32(idTextBox.Text),
                    nameTextBox.Text,
                    Convert.ToInt32(ageTextBox.Text),
                    emailTextBox.Text
                );

                // Reemplazar la persona seleccionada con la persona modificada en la lista
                People[index] = updatedPerson;

                // Limpiar los campos de entrada
                limpiar();

                // Mostrar un mensaje de éxito
                MessageBox.Show("Los datos de la persona han sido modificados exitosamente.", "Modificación exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void limpiar()
        {
            idTextBox.Text = "";
            nameTextBox.Text = "";
            ageTextBox.Text = "";
            emailTextBox.Text = "";

        }


        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (SelectedPerson != null)
            {
                People.Remove(SelectedPerson);
                SelectedPerson = null;
            }
        }


        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGrid dataGrid = (DataGrid)sender;
            SelectedPerson = (Person)dataGrid.SelectedItem;

            // Cargar los datos en los campos de entrada
            if (SelectedPerson != null)
            {
                idTextBox.Text = SelectedPerson.ID.ToString();
                nameTextBox.Text = SelectedPerson.Name;
                ageTextBox.Text = SelectedPerson.Age.ToString();
                emailTextBox.Text = SelectedPerson.Email;
            }
        }
    }

}

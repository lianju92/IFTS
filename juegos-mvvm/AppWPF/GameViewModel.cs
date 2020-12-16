using AppWPF.MVVM.Model;
using AppWPF.MVVM.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;


namespace AppWPF.ViewModel
{
    class GameViewModel : ObservableCollection<Game>, INotifyPropertyChanged
    {

        #region Atributos
        private int selectedIndex;

        private int id;
        private string titulo;
        private string desarrolladora;
        private string pais;
        private string consola;
        private int year_lanzamiento;
        private ICommand newGameCommand;
        private ICommand addGameCommand;
        private ICommand delGameCommand;
        private ICommand updateGameCommand;

        //OleDbProperty
        OleDbConnection conector;
        private OleDbDataReader reader;
        private OleDbTransaction tra;
        private OleDbDataAdapter adapter;

        //Conexion con la Base de Datos
        public string ConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\Juegos.mdb";
        #endregion

        #region Propiedades
        public int SelectedIndexOfCollection
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                selectedIndex = value;
                OnPropertyChanged("SelectedIndexOfCollection");
                //Activa el evento OnPropertyChanged de los atributos para refrescar las propiedades segun el indice seleccionado.
                OnPropertyChanged("Id");
                OnPropertyChanged("Titulo");
                OnPropertyChanged("Desarrolladora");
                OnPropertyChanged("Pais");
                OnPropertyChanged("Consola");
                OnPropertyChanged("Year_lanzamiento");
            }
        }

        private ObservableCollection<string> values = new ObservableCollection<string>()//Combobox de Consola
        {
            "PS4", "PC", "Xbox 720", "Nintendo Switch"
        };
        public ObservableCollection<string> Values
        {
            get { return values; }
            set
            {
                values = value;
                OnPropertyChanged("Consola");
            }
        }
        private string selectedValue;
        public string SelectedValue
        {
            get { return selectedValue; }
            set
            {
                selectedValue = value;
                OnPropertyChanged("Consola");
            }
        }

        
        public int Year_lanzamiento
        {
            get
            {
                if (this.SelectedIndexOfCollection > -1)
                {
                    return this.Items[this.SelectedIndexOfCollection].Year_lanzamiento;
                }
                else
                {
                    return year_lanzamiento;
                }
            }
            set
            {
                if (this.SelectedIndexOfCollection > -1)
                {
                    this.Items[this.SelectedIndexOfCollection].Year_lanzamiento = value;
                }
                else
                {
                    year_lanzamiento = value;
                }
                OnPropertyChanged("Year_lanzamiento");
            }
        }

        public int Id
        {
            get
            {
                if (this.SelectedIndexOfCollection > -1)
                {
                    return this.Items[this.SelectedIndexOfCollection].Id;
                }
                else
                {
                    return id;
                }
            }
            set
            {
                if (this.SelectedIndexOfCollection > -1)
                {
                    this.Items[this.SelectedIndexOfCollection].Id = value;
                }
                else
                {
                    id = value;
                }
                OnPropertyChanged("Id");
            }
        }

        public string Titulo
        {
            get
            {
                if (this.SelectedIndexOfCollection > -1)
                {
                    return this.Items[this.SelectedIndexOfCollection].Titulo;
                }
                else
                {
                    return titulo;
                }
            }
            set
            {
                if (this.SelectedIndexOfCollection > -1)
                {
                    this.Items[this.SelectedIndexOfCollection].Titulo = value;
                }
                else
                {
                    titulo = value;
                }
                OnPropertyChanged("Titulo");
            }
        }

        public string Desarrolladora
        {
            get
            {
                if (this.SelectedIndexOfCollection > -1)
                {
                    return this.Items[this.SelectedIndexOfCollection].Desarrolladora;
                }
                else
                {
                    return desarrolladora;
                }
            }
            set
            {
                if (this.SelectedIndexOfCollection > -1)
                {
                    this.Items[this.SelectedIndexOfCollection].Desarrolladora = value;
                }
                else
                {
                    desarrolladora = value;
                }
                OnPropertyChanged("Desarrolladora");
            }
        }
        public string Pais
        {
            get
            {
                if (this.SelectedIndexOfCollection > -1)
                {
                    return this.Items[this.SelectedIndexOfCollection].Pais;
                }
                else
                {
                    return pais;
                }
            }
            set
            {
                if (this.SelectedIndexOfCollection > -1)
                {
                    this.Items[this.SelectedIndexOfCollection].Pais = value;
                }
                else
                {
                    pais = value;
                }
                OnPropertyChanged("Pais");
            }
        }

        public string Consola
        {
            get
            {
                if (this.SelectedIndexOfCollection > -1)
                {
                    return this.Items[this.SelectedIndexOfCollection].Consola;
                }
                else
                {
                    return consola;
                }
            }
            set
            {
                if (this.SelectedIndexOfCollection > -1)
                {
                    this.Items[this.SelectedIndexOfCollection].Consola = value;
                }
                else
                {
                    consola = value;
                }
                OnPropertyChanged("Consola");
            }
        }
    
        #endregion

        #region ICommand
        public ICommand NewGameCommand
        {
            get
            {
                return newGameCommand;
            }
            set
            {
                newGameCommand = value;
            }
        }

        public ICommand AddGameCommand
        {
            get
            {
                return addGameCommand;
            }
            set
            {
                addGameCommand = value;
            }
        }
        public ICommand DelGameCommand
        {
            get
            {
                return delGameCommand;
            }
            set
            {
                delGameCommand = value;
            }
        }

        public ICommand UpdateGametCommand
        {
            get
            {
                return updateGameCommand;
            }
            set
            {
                updateGameCommand = value;
            }
        }
        #endregion

        #region Constructores
        public GameViewModel()
        {
            updateGameCommand = new CommandBase(param => this.UpdateGame());
            AddGameCommand = new CommandBase(param => this.AddGame());
            NewGameCommand = new CommandBase(new Action<Object>(NewGame));
            DelGameCommand = new CommandBase(param => this.DelGame());
            ViewGame();

        }//Fin de constructor.
        #endregion

        #region Interface
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion

        #region Metodos/Funciones
        private void ViewGame() {
            conector = new OleDbConnection(ConnectionString);
            OleDbCommand cmd = new OleDbCommand("select * from tbl_games", conector);
            adapter = new OleDbDataAdapter(cmd);
            conector.Open();
            reader = cmd.ExecuteReader();
            this.Clear();
            while (reader.Read())
            {
                Game vlGame = new Game();
                vlGame.Id = (int)reader["Id"];
                vlGame.Titulo = (string)reader["Titulo"];
                vlGame.Desarrolladora = (string)reader["Desarrolladora"];
                vlGame.Pais = (string)reader["Pais"];
                vlGame.Consola = (string)reader["Consola"];
                vlGame.Year_lanzamiento = (int)reader["Year_lanzamiento"];
                this.Add(vlGame);
            }
            conector.Close();
        }
        private void AddGame()
        {

            Game vlGame = new Game();
            vlGame.Id = Id;
            vlGame.Titulo = Titulo;
            vlGame.Desarrolladora = Desarrolladora;
            vlGame.Pais = Pais;
            vlGame.Consola = Consola;
            vlGame.Year_lanzamiento = Year_lanzamiento;
            if (SelectedIndexOfCollection != -1){
                
                    OleDbCommand cmd = new OleDbCommand();
                    conector.Open();
                    cmd.Connection = conector;
                    cmd.CommandText = "update tbl_games set titulo='" + vlGame.Titulo + "', desarrolladora='" + vlGame.Desarrolladora + "', pais='" + vlGame.Pais + "', consola='" + vlGame.Consola + "', year_lanzamiento='" + vlGame.Year_lanzamiento + "' where Id=" + vlGame.Id;
                    cmd.ExecuteNonQuery();
                    tra = conector.BeginTransaction();
                    cmd.Transaction = tra;
                    conector.Close();
            }else
            {
                if (vlGame.Titulo != "" && vlGame.Desarrolladora != "" && vlGame.Pais != "" && vlGame.Consola != "" && vlGame.Year_lanzamiento != 0) {
                    OleDbCommand cmd = new OleDbCommand();
                    conector.Open();
                    cmd.Connection = conector;
                    cmd.CommandText = "insert into tbl_games (titulo,desarrolladora,pais,consola,year_lanzamiento) Values ('" + Titulo + "','" + Desarrolladora + "','" + Pais + "','" + Consola + "','" + Year_lanzamiento + "')";
                    cmd.ExecuteNonQuery();
                    tra = conector.BeginTransaction();
                    cmd.Transaction = tra;
                    conector.Close();
                }else
                {
                    MessageBox.Show("Debe Completar todos los Campos", "Campos Vacíos");
                }
            }
            ViewGame();
        }

        private void UpdateGame() { }
        private void NewGame(object obj)
        {
            SelectedIndexOfCollection = -1;
            Id = 0;
            Titulo = "";
            Desarrolladora = "";
            Pais = "";
            Consola = "";
            Year_lanzamiento = 0;
        }
        private void DelGame()
        {

            Game vlGame = new Game();
            vlGame.Id = Id;
            vlGame.Titulo = Titulo;
            vlGame.Desarrolladora = Desarrolladora;
            vlGame.Pais = Pais;
            vlGame.Consola = Consola;
            vlGame.Year_lanzamiento = Year_lanzamiento;
            OleDbCommand cmd = new OleDbCommand();
            if (vlGame.Id == 0)
            {
                MessageBox.Show("Por favor seleccione el registro a Eliminar", "Eliminar Registro");
            }
            else
            {
                if (MessageBox.Show("¿Seguro que desea eliminar el juego seleccionado?",
                        "Confirmar eliminacion de registro", MessageBoxButton.YesNo,
                        MessageBoxImage.Warning) == MessageBoxResult.Yes) // Si al mostrar el cuadro de diálogo el usuario presiona el botón "yes" ...
                {
                    conector.Open();
                    cmd.Connection = conector;
                    cmd.CommandText = "delete from tbl_games where Id=" + vlGame.Id;
                    cmd.ExecuteNonQuery();
                    tra = conector.BeginTransaction();
                    cmd.Transaction = tra;
                    conector.Close();
                }
            }
            ViewGame();
        }
        #endregion
    }
}

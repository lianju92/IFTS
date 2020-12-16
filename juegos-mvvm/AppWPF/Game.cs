using System;

namespace AppWPF.MVVM.Model
{
    class Game : NotifyBase
    {
        #region Atributos
        private int id;
        private string titulo;
        private string desarrolladora;
        private string pais;
        private string consola;
        private int year_lanzamiento;
        #endregion

        #region Propiedades
        public int Id
        {
            get
            {
                return id;
            }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }

        public string Titulo
        {
            get
            {
                return titulo;
            }
            set
            {
                titulo = value;
                OnPropertyChanged("Titulo");
            }
        }

        public string Desarrolladora
        {
            get
            {
                return desarrolladora;
            }
            set
            {
                desarrolladora = value;
                OnPropertyChanged("Desarrolladora");
            }
        }

        public string Pais
        {
            get
            {
                return pais;
            }

            set
            {
                pais = value;
                OnPropertyChanged("Pais");
            }
        }

        public string Consola
        {
            get
            {
                return consola;
            }
            set
            {
                consola = value;
                OnPropertyChanged("Consola");
            }
        }

        public int Year_lanzamiento
        {
            get
            {
                return year_lanzamiento;
            }

            set
            {
                year_lanzamiento = value;
                OnPropertyChanged("Year_lanzamiento");
            }
        }
        #endregion
    }
}

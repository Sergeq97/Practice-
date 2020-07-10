using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Model;
namespace WpfApp1.ViewModel
{
    class StudentsVM : BaseHelper
    {
        private AttainmentEntities entities = null;
        private StudentsTBL oldType = null;
        private string text = "";
        RelayCommand editVisibilityCommand;
        RelayCommand editCommand;
        RelayCommand cancelCommand;
        RelayCommand deleteCommand;
        RelayCommand insertCommand;
        StudentsTBL selectedItem = null;
        Visibility visibility = Visibility.Collapsed;
        private ObservableCollection<StudentsTBL> typeAchievement = null;

        public string GetString
        {
            get { return text; }
            set
            {
                text = value;
                OnPropertyChnge(nameof(GetString));
            }
        }
        public Visibility GetVisibility
        {
            get { return visibility; }
            set
            {
                visibility = value;
                OnPropertyChnge(nameof(GetVisibility));
            }
        }

        public RelayCommand EditVisibilityCommand
        {
            get
            {
                return editVisibilityCommand ?? (editVisibilityCommand = new RelayCommand(o =>
               {
                   if (selectedItem != null)
                   {
                       GetString = selectedItem.FirstName;
                       GetVisibility = Visibility.Visible;
                   }
               }));
            }
        }
        public RelayCommand InserVisibilityCommand
        {
            get
            {
                return insertCommand ?? (insertCommand = new RelayCommand(o =>
                {
                    SelectedItem = null;
                    GetString = "";
                    GetVisibility = Visibility.Visible;
                }));
            }

        }
        public StudentsTBL SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                if (value != null)
                {
                    GetString = value.FirstName;
                    oldType = selectedItem.Clone() as StudentsTBL;
                    OnPropertyChnge(nameof(SelectedItem));
                }

            }
        }
        public RelayCommand CancelCommand
        {
            get
            {
                return cancelCommand ??
                   (cancelCommand = new RelayCommand(obj =>
                  {
                      SelectedItem.FirstName = oldType.FirstName;
                      SelectedItem = null;
                      GetVisibility = Visibility.Collapsed;
                  }));
            }

        }
        public ObservableCollection<StudentsTBL> TypeAchievement
        {
            get { return typeAchievement; }
            set
            {
                if (value != null)
                    typeAchievement = value;
                OnPropertyChnge(nameof(TypeAchievement));
            }

        }
        public RelayCommand EditAndInsertCommand
        {
            get
            {
                return editCommand ?? (editCommand = new RelayCommand((o) =>
                {
                    if (SelectedItem != null)
                    {
                        if (!string.IsNullOrEmpty(GetString))
                        {
                            var editItem = entities.StudentsTBLs.Find(SelectedItem.idStudents);
                            if (editItem != null)
                            {

                                editItem.FirstName = GetString;
                                entities.Entry(editItem).State = EntityState.Modified;
                                entities.SaveChanges();
                                
                            }
                        }
                    }
                    else
                    {
                        var inertItem = new StudentsTBL();
                        if (o is string)
                            inertItem.FirstName = (string)o;

                        entities.Entry(inertItem).State = EntityState.Added;
                        entities.StudentsTBLs.Add(inertItem);
                        entities.SaveChanges();
                    }
                    entities.TypeAchievementTBLs.Load();
                    TypeAchievement = entities.StudentsTBLs.Local;
                }));
            }
        }

        public RelayCommand DeleteCommand
        {
            get
            {
                return deleteCommand ?? (deleteCommand = new RelayCommand((o) =>
                {
                    GetVisibility = Visibility.Collapsed;
                    if (selectedItem != null)
                    {
                        var deletedItem = entities.StudentsTBLs.Find(selectedItem.idStudents);
                        if (deletedItem != null)
                        {

                            entities.StudentsTBLs.Remove(deletedItem);
                            entities.Entry(deletedItem).State = EntityState.Deleted;
                            entities.SaveChanges();
                            TypeAchievement.Remove(selectedItem);
                            SelectedItem = null;
                        }
                    }
                }));
            }
        }

        public StudentsVM()
        {
            entities = new AttainmentEntities();
            entities.StudentsTBLs.Load();
            var collect = entities.StudentsTBLs.Local.ToBindingList();
            TypeAchievement = new ObservableCollection<StudentsTBL>(collect);
        }
    }
}

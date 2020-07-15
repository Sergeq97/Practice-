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
        private string textFirstName = "";
        private string textLastName = "";
        private string textGroup = "";
        RelayCommand editVisibilityCommand;
        RelayCommand editCommand;
        RelayCommand cancelCommand;
        RelayCommand deleteCommand;
        RelayCommand insertCommand;
        RelayCommand visibilitiPortCommand;
        StudentsTBL selectedItem = null;
        Visibility visibility = Visibility.Collapsed;
        Visibility visibilityPort = Visibility.Collapsed;
        private ObservableCollection<StudentsTBL> typeAchievement = null;

        public string GetFirstName
        {
            get { return textFirstName; }
            set
            {
                textFirstName = value;
                OnPropertyChnge(nameof(GetFirstName));
            }
        }
        public string GetLastName
        {
            get { return textLastName; }
            set
            {
                textLastName = value;
                OnPropertyChnge(nameof(GetLastName));
            }
        }
        public string GetGroup
        {
            get { return textGroup; }
            set
            {
                textGroup = value;
                OnPropertyChnge(nameof(GetGroup));
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
        public Visibility GetPortVisbility
        {
            get { return visibilityPort; }
            set
            {
                visibilityPort = value;
                OnPropertyChnge(nameof(GetPortVisbility));
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
                       GetFirstName = selectedItem.FirstName;
                       GetLastName = selectedItem.LastName;
                       GetGroup = selectedItem.Groups;
                       GetVisibility = Visibility.Visible;
                   }
               }));
            }
        }
        public RelayCommand VisibilityPortCommand
        {
            get
            {
                return visibilitiPortCommand ?? (visibilitiPortCommand = new RelayCommand(o =>
                {
                    if (selectedItem != null)
                    {
                        GetPortVisbility = Visibility.Visible;
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
                    GetFirstName = "";
                    GetGroup = "";
                    GetLastName = "";
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
                    GetLastName = value.LastName;
                    GetGroup = value.Groups;
                    GetFirstName = value.FirstName;
                    GetPortVisbility = Visibility.Collapsed;
                    oldType = CloneItem();
                    OnPropertyChnge(nameof(SelectedItem));
                }

            }
        }

        private StudentsTBL CloneItem()
        {
            if (SelectedItem == null)
                return null;
            return new StudentsTBL()
            {
                idStudents = SelectedItem.idStudents,
                LastName = SelectedItem.LastName,
                FirstName = SelectedItem.FirstName,
                Groups = SelectedItem.Groups,
                AchievementTBLs = SelectedItem.AchievementTBLs,
            };
        }
        public RelayCommand CancelCommand
        {
            get
            {
                return cancelCommand ??
                   (cancelCommand = new RelayCommand(obj =>
                  {
                      if (oldType != null)
                      {
                          SelectedItem.FirstName = oldType.FirstName;
                          SelectedItem.LastName = oldType.LastName;
                          SelectedItem.Groups = oldType.Groups;
                          SelectedItem = null;
                      }
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
        private bool IsNullStrings(params string[] str)
        {
            foreach (var item in str)
            {
                if (string.IsNullOrEmpty(item))
                    return false;
            }
            return true;
        }
        public RelayCommand EditAndInsertCommand
        {
            get
            {
                return editCommand ?? (editCommand = new RelayCommand((o) =>
                {
                    if (IsNullStrings(GetFirstName, GetGroup, GetLastName))
                    {
                        if (SelectedItem != null)
                        {
                            var editItem = entities.StudentsTBLs.Find(SelectedItem.idStudents);
                            if (editItem != null)
                            {

                                editItem.FirstName = GetFirstName;
                                editItem.Groups = GetGroup;
                                editItem.LastName = GetLastName;
                                entities.Entry(editItem).State = EntityState.Modified;
                                entities.SaveChanges();

                            }
                        }
                        else
                        {
                            var inertItem = new StudentsTBL();
                            inertItem.FirstName = GetFirstName;
                            inertItem.Groups = GetGroup;
                            inertItem.LastName = GetLastName;
                            entities.Entry(inertItem).State = EntityState.Added;
                            entities.StudentsTBLs.Add(inertItem);
                            entities.SaveChanges();
                        }
                    entities.StudentsTBLs.Load();
                    TypeAchievement = entities.StudentsTBLs.Local;
                    }
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

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
    class AchievementsVM : BaseHelper
    {
        private AttainmentEntities entities = null;
        private AchievementTBL oldType = null;
        private StudentsTBL student = null;
        private TypeAchievementTBL type = null;
        private string textTitle = "";
        private string textInfo = "";
        private DateTime? date = null;
        private ObservableCollection<StudentsTBL> studentCollect = null;
        private ObservableCollection<TypeAchievementTBL> typeCollect = null;
        RelayCommand editVisibilityCommand;
        RelayCommand editCommand;
        RelayCommand cancelCommand;
        RelayCommand deleteCommand;
        RelayCommand insertCommand;
        AchievementTBL selectedItem = null;
        Visibility visibility = Visibility.Collapsed;
        private ObservableCollection<AchievementTBL> typeAchievement = null;



        public string GetTitle
        {
            get { return textTitle; }
            set
            {
                textTitle = value;
                OnPropertyChnge(nameof(GetTitle));
            }
        }
        public string GetInfo
        {
            get { return textInfo; }
            set
            {
                textInfo = value;
                OnPropertyChnge(nameof(GetInfo));
            }
        }
        public ObservableCollection<StudentsTBL> GetStudents
        {
            get { return studentCollect; }
            set
            {
                studentCollect = value;
                OnPropertyChnge(nameof(GetStudents));
            }
        }
        public ObservableCollection<TypeAchievementTBL> GetTypes
        {
            get { return typeCollect; }
            set
            {
                typeCollect = value;
                OnPropertyChnge(nameof(GetTypes));
            }
        }
        public DateTime? GetDate
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChnge(nameof(GetDate));
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
                       GetTitle = selectedItem.TitleAchievement;
                       GetInfo = selectedItem.infoAchievement;
                       GetDate = selectedItem.DateReceived;
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
                    GetTitle = "";
                    GetInfo = "";
                    GetDate = null;
                    GetVisibility = Visibility.Visible;
                }));
            }

        }
        public AchievementTBL SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                if (value != null)
                {
                    GetTitle = selectedItem.TitleAchievement;
                    GetInfo = selectedItem.infoAchievement;
                    GetDate = selectedItem.DateReceived;
                    oldType = CLoneItem();
                    OnPropertyChnge(nameof(SelectedItem));
                }

            }
        }

        public StudentsTBL GetStud
        {
            get { return student; }
            set
            {
                if (value != null)
                {
                    student = value;
                    OnPropertyChnge(nameof(GetStud));
                }
            }
        }
        public TypeAchievementTBL TypeGet
        {
            get { return type; }
            set
            {
                if (value != null)
                {
                    type = value;
                    OnPropertyChnge(nameof(TypeGet));
                }
            }
        }
        public AchievementTBL CLoneItem()
        {
            if (SelectedItem == null)
                return null;
            return new AchievementTBL()
            {
                idAchievement = SelectedItem.idAchievement,
                DateReceived = SelectedItem.DateReceived,
                infoAchievement = SelectedItem.infoAchievement,
                Student = SelectedItem.Student,
                StudentsTBL = SelectedItem.StudentsTBL,
                TitleAchievement = SelectedItem.TitleAchievement,
                TypeAchievement = SelectedItem.TypeAchievement,
                TypeAchievementTBL = SelectedItem.TypeAchievementTBL,
            };
        }
        public RelayCommand CancelCommand
        {
            get
            {
                return cancelCommand ??
                   (cancelCommand = new RelayCommand(obj =>
                  {
                      SelectedItem.TitleAchievement = oldType.TitleAchievement;
                      SelectedItem = null;
                      GetVisibility = Visibility.Collapsed;
                  }));
            }

        }
        public ObservableCollection<AchievementTBL> TypeAchievement
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
                    if (SelectedItem != null)
                    {
                        if (IsNullStrings(GetInfo, GetTitle, GetDate.ToString()))
                        {
                            var editItem = entities.AchievementTBLs.Find(SelectedItem.idAchievement);
                            if (editItem != null)
                            {
                                editItem.infoAchievement = GetInfo;
                                editItem.TitleAchievement = GetTitle;
                                editItem.DateReceived = GetDate.Value;
                                editItem.Student = SelectedItem.StudentsTBL.idStudents;
                                editItem.StudentsTBL = SelectedItem.StudentsTBL;
                                editItem.TypeAchievement = SelectedItem.TypeAchievementTBL.idType;
                                editItem.TypeAchievementTBL = SelectedItem.TypeAchievementTBL;
                                entities.Entry(editItem).State = EntityState.Modified;
                                entities.SaveChanges();


                            }
                        }
                    }
                    else
                    {
                        var editItem = new AchievementTBL();

                        editItem.infoAchievement = GetInfo;
                        editItem.TitleAchievement = GetTitle;
                        editItem.DateReceived = GetDate.Value;
                        editItem.Student = GetStud.idStudents;
                        editItem.TypeAchievement = TypeGet.idType;
                        
                        entities.Entry(editItem).State = EntityState.Added;
                        entities.AchievementTBLs.Add(editItem);
                        entities.SaveChanges();
                    }
                    entities.TypeAchievementTBLs.Load();
                    TypeAchievement = entities.AchievementTBLs.Local;
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
                        var deletedItem = entities.AchievementTBLs.Find(selectedItem.idAchievement);
                        if (deletedItem != null)
                        {

                            entities.AchievementTBLs.Remove(deletedItem);
                            entities.Entry(deletedItem).State = EntityState.Deleted;
                            entities.SaveChanges();
                            TypeAchievement.Remove(selectedItem);
                            SelectedItem = null;
                        }
                    }
                }));
            }
        }


        public AchievementsVM()
        {
            entities = new AttainmentEntities();
            entities.AchievementTBLs.Load();
            entities.StudentsTBLs.Load();
            entities.TypeAchievementTBLs.Load();
            var collect = entities.AchievementTBLs.Local.ToBindingList();
            TypeAchievement = new ObservableCollection<AchievementTBL>(collect);
            GetStudents = new ObservableCollection<StudentsTBL>(entities.StudentsTBLs.Local);
            GetTypes = new ObservableCollection<TypeAchievementTBL>(entities.TypeAchievementTBLs.Local);
        }
    }
}

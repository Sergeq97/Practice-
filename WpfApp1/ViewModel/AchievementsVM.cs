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
        private string text = "";
        RelayCommand editVisibilityCommand;
        RelayCommand editCommand;
        RelayCommand cancelCommand;
        RelayCommand deleteCommand;
        RelayCommand insertCommand;
        AchievementTBL selectedItem = null;
        Visibility visibility = Visibility.Collapsed;
        private ObservableCollection<AchievementTBL> typeAchievement = null;

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
                       GetString = selectedItem.TitleAchievement;
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
        public AchievementTBL SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                if (value != null)
                {
                    GetString = value.TitleAchievement;
                    oldType = CLoneItem() ;
                    OnPropertyChnge(nameof(SelectedItem));
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
                            var editItem = entities.AchievementTBLs.Find(SelectedItem.idAchievement);
                            if (editItem != null)
                            {

                                editItem.infoAchievement = GetString;
                                entities.Entry(editItem).State = EntityState.Modified;
                                entities.SaveChanges();

                            }
                        }
                    }
                    else
                    {
                        var inertItem = new TypeAchievementTBL();
                        if (o is string)
                            inertItem.TypeAchievement = (string)o;

                        entities.Entry(inertItem).State = EntityState.Added;
                        entities.TypeAchievementTBLs.Add(inertItem);
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
            var collect = entities.AchievementTBLs.Local.ToBindingList();
            TypeAchievement = new ObservableCollection<AchievementTBL>(collect);
        }
    }
}

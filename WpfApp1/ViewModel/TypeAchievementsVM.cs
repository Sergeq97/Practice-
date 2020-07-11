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
    class TypeAchievementsVM : BaseHelper
    {
        private AttainmentEntities entities = null;
        private TypeAchievementTBL oldType = null;
        private string text = "";
        RelayCommand editVisibilityCommand;
        RelayCommand editCommand;
        RelayCommand cancelCommand;
        RelayCommand deleteCommand;
        RelayCommand insertCommand;
        TypeAchievementTBL selectedItem = null;
        Visibility visibility = Visibility.Collapsed;
        private ObservableCollection<TypeAchievementTBL> typeAchievement = null;

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
                       GetString = selectedItem.TypeAchievement;
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
        public TypeAchievementTBL SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                if (value != null)
                {
                    GetString = value.TypeAchievement;
                    oldType = CloneItem();
                    OnPropertyChnge(nameof(SelectedItem));
                }

            }
        }
        private  TypeAchievementTBL CloneItem()
        {
            if (SelectedItem == null)
                return null;
            return new TypeAchievementTBL()
            {
                idType = selectedItem.idType,
                TypeAchievement = selectedItem.TypeAchievement,
                AchievementTBLs = selectedItem.AchievementTBLs,
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
                          SelectedItem.TypeAchievement = oldType.TypeAchievement;
                      SelectedItem = null;
                      GetVisibility = Visibility.Collapsed;
                  }));
            }

        }
        public ObservableCollection<TypeAchievementTBL> TypeAchievement
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
                            var editItem = entities.TypeAchievementTBLs.Find(SelectedItem.idType);
                            if (editItem != null)
                            {

                                editItem.TypeAchievement = GetString;
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
                    TypeAchievement = entities.TypeAchievementTBLs.Local;
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
                        var deletedItem = entities.TypeAchievementTBLs.Find(selectedItem.idType);
                        if (deletedItem != null)
                        {

                            entities.TypeAchievementTBLs.Remove(deletedItem);
                            entities.Entry(deletedItem).State = EntityState.Deleted;
                            entities.SaveChanges();
                            TypeAchievement.Remove(selectedItem);
                            SelectedItem = null;
                        }
                    }
                }));
            }
        }


        //public RelayCommand AddCommand
        //{

        //    get
        //    {
        //        return addCommand ??
        //            (addCommand = new RelayCommand((o) =>
        //            {
        //                //PhoneViewModel vm = new PhoneViewModel();
        //                phoneWindow = new PhoneWindow(new Phone());
        //                if (phoneWindow.ShowDialog() == true)
        //                {
        //                    TypeAchievementTBL type = phoneWindow.Phone;
        //                    entities.TypeAchievementTBLs.Add(type);
        //                    entities.SaveChanges();
        //                }
        //            }));
        //    }
        //}

        public TypeAchievementsVM()
        {
            entities = new AttainmentEntities();
            entities.TypeAchievementTBLs.Load();
            var collect = entities.TypeAchievementTBLs.Local.ToBindingList();
            TypeAchievement = new ObservableCollection<TypeAchievementTBL>(collect);
        }
    }
}

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
        RelayCommand addCommand;
        RelayCommand editVisibilityCommand;
        RelayCommand editCommand;
        RelayCommand cancelCommand;
        RelayCommand deleteCommand;
        TypeAchievementTBL selectedItem = null;
        Visibility visibility = Visibility.Collapsed;
        private ObservableCollection<TypeAchievementTBL> typeAchievement = null;

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
                   if(selectedItem != null)
                     GetVisibility = Visibility.Visible;
               }));
            }

        }
        public TypeAchievementTBL SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (value != null)
                {
                    selectedItem = value;
                    oldType = selectedItem.Clone() as TypeAchievementTBL;
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
            }

        }
        public RelayCommand EditCommand
        {
            get
            {
                return editCommand ?? (editCommand = new RelayCommand((o) =>
                {
                    if (selectedItem != null && !string.IsNullOrEmpty(selectedItem.TypeAchievement))
                    {
                        var editItem = entities.TypeAchievementTBLs.Find(selectedItem.idType);
                        if (editItem != null)
                        {
                            editItem.TypeAchievement = editItem.TypeAchievement;
                            entities.Entry(editItem).State = EntityState.Modified;
                            entities.SaveChanges();
                        }
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
                    OnPropertyChnge(nameof(GetVisibility));
                    if (selectedItem != null)
                    {
                        var deletedItem = entities.TypeAchievementTBLs.Find(selectedItem.idType);
                        if (deletedItem != null)
                        {

                            entities.TypeAchievementTBLs.Remove(deletedItem);
                            entities.Entry(deletedItem).State = EntityState.Deleted;
                            entities.SaveChanges();
                            TypeAchievement.Remove(selectedItem);
                            selectedItem = null;
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

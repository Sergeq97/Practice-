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
        RelayCommand addCommand;
        RelayCommand editCommand;
        RelayCommand cancelCommand;
        RelayCommand deleteCommand;
        TypeAchievementTBL selectedItem = null;

        private ObservableCollection<TypeAchievementTBL> typeAchievement = null;

        public Visibility GetVisibility { get; set; } = Visibility.Collapsed;

        public TypeAchievementTBL SelectedItem
        {
            get { return selectedItem; }
            set
            {
                if (value != null)
                {
                    selectedItem = value;
                    GetVisibility = Visibility.Visible;
                    OnPropertyChnge(nameof(SelectedItem));
                    OnPropertyChnge(nameof(GetVisibility));
                }
            }
        }
        public RelayCommand CancelCommand
        {
            get
            {
                return cancelCommand ??
                   (cancelCommand = new  RelayCommand(obj =>
                   {
                      
                       GetVisibility = Visibility.Collapsed;
                       SelectedItem = null;
                       OnPropertyChnge(nameof(GetVisibility));
                       OnPropertyChnge(nameof(SelectedItem));
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

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
namespace WpfApp1.ViewModel
{
    class TypeAchievementsVM
    {
        private AttainmentEntities entities = null;

        private ObservableCollection<TypeAchievementTBL> typeAchievement = null;
        public ObservableCollection<TypeAchievementTBL> TypeAchievement
        {
            get { return typeAchievement; }
            set
            {
                if(value != null)
                  typeAchievement =  value;
            }

        }


        public TypeAchievementsVM()
        {
            entities = new AttainmentEntities();
            entities.TypeAchievementTBLs.Load();
            var collect = entities.TypeAchievementTBLs.Local.ToBindingList();
            TypeAchievement = new ObservableCollection<TypeAchievementTBL>(collect);
        }
    }
}

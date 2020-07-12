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
    class СlassificationVM : BaseHelper
    {

        AttainmentEntities entites = null;
        private TypeAchievementTBL selected = null;
        public List<AchievementTBL> tb { get; set; }
        public List<TypeAchievementTBL> type { get; set; }
        public TypeAchievementTBL Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                OnPropertyChnge(nameof(Selected));
            }
        }
        public СlassificationVM()
        {
            entites = new AttainmentEntities();
            entites.TypeAchievementTBLs.Load();
            type = new List<TypeAchievementTBL>(entites.TypeAchievementTBLs.Local);
            
        }

    }



}

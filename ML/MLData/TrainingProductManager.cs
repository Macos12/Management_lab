using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLData
{
    public class TrainingProductManager
    {
        public List<cooler_> Get()
        {
            List<cooler_> ret = new List<cooler_>();
            ret = CreateMockData();
            return ret;
        }
        public db_managment_labEntities db = new db_managment_labEntities();
        private List<cooler_> CreateMockData()
        {
            db_managment_labEntities db = new db_managment_labEntities();
            List<cooler_> ret;
            ret.Add();
            return from db_ in db.cooler_
                   select ; 
        }
    }
}

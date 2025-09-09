using DemkaADO.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DemkaADO.Models;
using System.Windows.Navigation;
using System.IO;

namespace DemkaADO.Services
{
    internal class ServiceService
    {
        public static demka_test2Entities1 db = Helper.GetContext();

        public List<Service> GetServices()
        {
            List<Service> services = db.Service.ToList();
            for (int i = 0; i < services.Count; i++)
            {
                services[i].DurationInSeconds = services[i].DurationInSeconds / 60;
                services[i].MainImagePath = "/Images/" + services[i].MainImagePath; //добавление директории к существующему пути в базе на локальном клиенте для корректного вывода изображений
                decimal discount_ = ((Convert.ToDecimal(services[i].Discount.Value) * Convert.ToDecimal(services[i].Cost)) / 100);
                services[i].FinalCost = services[i].Cost - Convert.ToInt32(discount_);
            }
            return services;
        }

        public List<Service> GetFilterByLowCostServices()
        {
            List<Service> services = db.Service.OrderBy(x => x.FinalCost).ToList();
            for (int i = 0; i < services.Count; i++)
            {
                services[i].MainImagePath = "/Images/" + services[i].MainImagePath; //добавление директории к существующему пути в базе на локальном клиенте для корректного вывода изображений
                decimal discount_ = ((Convert.ToDecimal(services[i].Discount.Value) * Convert.ToDecimal(services[i].Cost)) / 100);
                services[i].FinalCost = services[i].Cost - Convert.ToInt32(discount_);
            }
            return services;
        }

        public List<Service> GetFilterByUpCostServices()
        {
            List<Service> services = db.Service.OrderByDescending(x => x.FinalCost).ToList();
            for (int i = 0; i < services.Count; i++)
            {
                services[i].MainImagePath = "/Images/" + services[i].MainImagePath; //добавление директории к существующему пути в базе на локальном клиенте для корректного вывода изображений
                decimal discount_ = ((Convert.ToDecimal(services[i].Discount.Value) * Convert.ToDecimal(services[i].Cost)) / 100);
                services[i].FinalCost = services[i].Cost - Convert.ToInt32(discount_);
            }
            return services;
        }

        public List<Service> GetFilterByDiscount(int min, int max)
        {
            List<Service> services = db.Service.Where(x => x.Discount >= min && x.Discount < max).OrderBy(x => x.Discount).ToList();
            for (int i = 0; i < services.Count; i++)
            {
                services[i].MainImagePath = "/Images/" + services[i].MainImagePath;
                if (services[i].Discount != null)
                {
                    double discount_ = ((services[i].Discount.Value * Convert.ToDouble(services[i].Cost)) / 100);
                    services[i].Cost = services[i].Cost - Convert.ToInt32(discount_);
                }
            }
            return services;
        }

    }
}

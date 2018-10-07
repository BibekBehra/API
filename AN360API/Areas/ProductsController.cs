
using AN360API.Formatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AutoMapper;
using System.Web.Http.OData;
using Newtonsoft.Json;
using System.ComponentModel;
using System.Globalization;

namespace AN360API.Areas
{
    public class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Company { get; set; }
        public string FatherName { get; set; }
        public string Dept { get; set; }
        public string Gender { get; set; }
        public string maritialstatus { get; set; }
    }
    public class Patient
    {
        public int PatientID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Job { get; set; }
        public string FatherName { get; set; }
        public string salary { get; set; }
        public string Gender { get; set; }
        public string maritialstatus { get; set; }

    }

    public class ProductsController : ApiController
    {
        class GeoPointConverter : TypeConverter
        {
            public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            {
                if (sourceType == typeof(string))
                {
                    return true;
                }
                return base.CanConvertFrom(context, sourceType);
            }

            public override object ConvertFrom(ITypeDescriptorContext context,
                CultureInfo culture, object value)
            {
                if (value is string)
                {
                    GeoPoint point;
                    if (GeoPoint.TryParse((string)value, out point))
                    {
                        return point;
                    }
                }
                return base.ConvertFrom(context, culture, value);
            }
        }
        [TypeConverter(typeof(GeoPointConverter))]
        public class GeoPoint
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }

            public static bool TryParse(string s, out GeoPoint result)
            {
                result = null;

                var parts = s.Split(',');
                if (parts.Length != 2)
                {
                    return false;
                }

                double latitude, longitude;
                if (double.TryParse(parts[0], out latitude) &&
                    double.TryParse(parts[1], out longitude))
                {
                    result = new GeoPoint() { Longitude = longitude, Latitude = latitude };
                    return true;
                }
                return false;
            }
        }

        public bool map()
        {
            User uobj = new User();
            uobj.ID = 100;
            uobj.Name = "Susant";
            uobj.Address = "Bangalore";
            uobj.Company = "Accenture";
            uobj.Dept = "IT";
            uobj.Dept = "susant@gmail.com";
            uobj.FatherName = "Mohan";
            uobj.Gender = "Male";
            uobj.maritialstatus = "Unmarried";


            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<User, Patient>().ForMember(x => x.PatientID, opt => opt.MapFrom(src => src.ID))

               .ForMember(x => x.Job, opt => opt.MapFrom(src => src.Company)

               );
            });

            IMapper mapper = config.CreateMapper();
            var dest = mapper.Map<User, Patient>(uobj);
            return true;
        }
        private static List<Product> _products = new List<Product>()
            {

                new Product() { Id = 1, Name = "Honda Civic", Description = "Luxury Model 2013" },
                new Product() { Id = 2, Name = "Honda Accord", Description = "Deluxe Model 2012" },
                new Product() { Id = 3, Name = "BMW", Description = "V6 Engine Luxury 2013" },
                new Product() { Id = 4, Name = "Audi", Description = "V8 Engine 2013" },
                new Product() { Id = 5, Name = "Mercedes M3", Description = "Basic Model 2013" }
            };

        public ProductsController()
        {   
        }

        [EnableQuery]
        [HttpGet]
        public IEnumerable<Product> GetProducts(GeoPoint location)
        {
            //map();
            return _products.AsQueryable();
        }
        [HttpPost]
        public HttpResponseMessage PostProducts(string multiple, List<Product> p)
        {
            if (p == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            _products.AddRange(p);
            return new HttpResponseMessage(HttpStatusCode.Created);
        }
        [HttpGet]
        public Product GetProduct(int id)
        {
            Product pro = _products.Find(p => p.Id == id);

            if (pro == null)
                throw new HttpResponseException(HttpStatusCode.NotFound);
            else
                return pro;
        }

        [HttpGet]
        public IEnumerable<Product> GetProductsBySearch(string search)
        {
            var products = _products.Where(p => p.Description.Contains(search));

            if (products.ToList().Count > 0)
                return products;
            else
                throw new HttpResponseException(HttpStatusCode.BadRequest);
        }

        [HttpPost]
        public HttpResponseMessage PostProduct(Product p)
        {
            if (p == null)
                return new HttpResponseMessage(HttpStatusCode.BadRequest);

            _products.Add(p);
            return new HttpResponseMessage(HttpStatusCode.Created);
        }

        [HttpDelete]
        public IEnumerable<Product> DeleteProduct(int id)
        {
            Product pro = _products.Find(p => p.Id == id);
            _products.Remove(pro);

            return _products;
        }

      
        public HttpResponseMessage PutProduct(Product p)
        {
            Product pro = _products.Find(pr => pr.Id == p.Id);

            if (pro == null)
                return new HttpResponseMessage(HttpStatusCode.NotFound);

            pro.Id = p.Id;
            pro.Name = p.Name;
            pro.Description = p.Description;

            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}
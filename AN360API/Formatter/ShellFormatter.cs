using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace AN360API.Formatter
{
   
    [DataContract]
    public class Product
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }
       
    }

    public class ProductFormatter : BufferedMediaTypeFormatter
    {
        public ProductFormatter()
        {
            SupportedMediaTypes.Add(
                                new MediaTypeHeaderValue("application/custom-product-type"));
        }

        public override bool CanReadType(Type type)
        {
            // for single product object
            if (type == typeof(Product))
                return true;
            else
            {
                // for multiple product objects
                Type _type = typeof(IEnumerable<Product>);
                return _type.IsAssignableFrom(type);
            }
        }

        public override bool CanWriteType(Type type)
        {
            //for single product object
            if (type == typeof(Product))
                return true;
            else
            {
                // for multiple product objects
                Type _type = typeof(IEnumerable<Product>);
                return _type.IsAssignableFrom(type);
            }
        }

        public override void WriteToStream(Type type,
                                           object value,
                                           Stream writeStream,
                                           HttpContent content)
        {
           // Encoding effectiveEncoding = SelectCharacterEncoding(content.Headers);

            using (StreamWriter writer = new StreamWriter(writeStream))
            {
                /* In this code, we are going to serialize product object to 
                 * "application/custom-product-type" format string
                 */
                var products = value as IEnumerable<Product>;
                if (products != null)
                {
                    foreach (var product in products)
                    {
                        writer.Write(String.Format("[{0},\"{1}\",\"{2}\"]",
                                                    product.Id,
                                                    product.Name,
                                                    product.Description));
                    }
                }
                else
                {
                    var pro = value as Product;
                    if (pro == null)
                    {
                        throw new InvalidOperationException("Cannot serialize type");
                    }
                    writer.Write(String.Format("[{0},\"{1}\",\"{2}\"]",
                                                pro.Id,
                                                pro.Name,
                                                pro.Description));
                }
            }
        }

        public override object ReadFromStream(Type type,
                                              Stream readStream,
                                              HttpContent content,
                                              IFormatterLogger formatterLogger)
        {
            using (StreamReader reader = new StreamReader(readStream))
            {
                /* Following code is not in good shape. In this code we make the basic plumbing to parse 
                 * the input "application/custom-product-type" format string and deserialize it to Product 
                 * objects.
                 */
                String productString = reader.ReadToEnd().ParseProductsString();
                String[] productArray = productString.Split(new string[] { "}{" },
                                                            StringSplitOptions.RemoveEmptyEntries);

                List<Product> products = new List<Product>();
                foreach (string s in productArray)
                {
                    String[] productInterim = s.Split(new char[] { ',' });
                    string _name = productInterim[1].Replace("\"", String.Empty);
                    string _description = productInterim[2].Replace("\"", String.Empty);

                    products.Add(new Product()
                    {
                        Id = Int32.Parse(productInterim[0]),
                        Name = _name,
                        Description = _description
                    });
                }
                return products;
            }
        }
    }

    public static class ExtensionMethods
    {
        public static string ParseProductsString(this string original)
        {
            return original.Replace("][", "}{")
                           .Replace("[", string.Empty)
                           .Replace("]", string.Empty);
        }

        public static string ReplaceExtraQuotes(this string original)
        {
            return original.Replace("\"", String.Empty);
        }
    }
}

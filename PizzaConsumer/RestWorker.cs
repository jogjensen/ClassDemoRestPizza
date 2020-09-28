using PizzaLib.model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PizzaConsumer
{
    internal class RestWorker
    {
        private const String URI = "http://localhost:54395/api/Pizzas/";

        public RestWorker()
        { }
        
        public async void Start()
        {
            List<Pizza> allPizzas = await GetAllPizzas();
            foreach (Pizza pizza in allPizzas)
            {
                Console.WriteLine(pizza);
            }

            OpretNyPizza();

            try
            {
                Pizza pizza1 = await GetOnePizza(3);
                Console.WriteLine("pizza " + pizza1);
            }
            catch (KeyNotFoundException knfe)
            {
                Console.WriteLine(knfe.Message);
            }
        }

        private async Task<Pizza> GetOnePizza(int nr)
        {
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage resp = await client.GetAsync(URI + nr);

                if (resp.IsSuccessStatusCode)
                {
                    string json = await resp.Content.ReadAsStringAsync();
                    Pizza pizza = JsonConvert.DeserializeObject<Pizza>(json);
                    return pizza;
                }

                throw new KeyNotFoundException($"Fejl kode={resp.StatusCode} message={resp.Content}");

            }

        }

        private async void OpretNyPizza()
        {
            using (HttpClient client = new HttpClient())
            {
                Pizza piz = new Pizza(23,"Dejlig vesus",true,90);

                StringContent content = new StringContent(
                    JsonConvert.SerializeObject(piz),
                    Encoding.UTF8,
                    "application/json");

                HttpResponseMessage resp = await client.PostAsync(URI, content);

                if (resp.IsSuccessStatusCode)
                {
                    return;
                }

                throw new ArgumentException("Opret fejlede");

            }

        }

        public async Task<List<Pizza>> GetAllPizzas()
        {
            using (HttpClient client = new HttpClient())
            {
                string json = await client.GetStringAsync(URI);

                List<Pizza> pizzas = JsonConvert.DeserializeObject<List<Pizza>>(json);
                return pizzas;
            }

        }
    }
}

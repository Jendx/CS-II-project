using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Datalayer
{
    public class JSONAPI
    {
        // Ukládá účtenky
        async public Task<List<List<Polozka>>> save(List<Polozka> uctenka) 
        {
            List<List<Polozka>> temp = new List<List<Polozka>>();
            try
            {
                using FileStream openStream = File.OpenRead("Uctenky.json");
                temp = await JsonSerializer.DeserializeAsync<List<List<Polozka>>>(openStream);
                openStream.Close();
            }
            catch (Exception e) 
            { 
            
            };

            temp.Add(uctenka);

            using FileStream createStream = File.OpenWrite("Uctenky.json");
            await JsonSerializer.SerializeAsync(createStream, temp, new JsonSerializerOptions() { WriteIndented = true});

            createStream.Close();

            return temp;
        }

        // Načítá zboží
        async public ValueTask<Polozka> GetItem(int id)
        {
            
            using FileStream openStream = File.OpenRead("Zbozi.json");
            var temp = await Task.Run( () => JsonSerializer.DeserializeAsync<List<Polozka>>(openStream));

            foreach(var a in temp.Result)
            {
                if (a.id == id)
                    { return a; }
            }

            return temp.Result[0];
        }

        async public Task<List<Polozka>> GetItems()
        {
            using FileStream openStream = File.OpenRead("Zbozi.json");
            var temp = await Task.Run(() => JsonSerializer.DeserializeAsync<List<Polozka>>(openStream));

            return temp.Result;
        }

        async public ValueTask<List<List<Polozka>>> GetUctenky()
        {

            using FileStream openStream = File.OpenRead("Uctenky.json");
            var temp = await Task.Run(() => JsonSerializer.DeserializeAsync <List<List<Polozka>>> (openStream));

            return temp.Result;
        }

        static async public Task<List<User>> GetUsers()
        {
            //ValueTask<List<User>> temp;
            FileStream openStream = File.OpenRead("Users.json");
            
            var temp = await Task.Run(() => JsonSerializer.DeserializeAsync<List<User>>(openStream));

            
            return temp.Result;
        }

        static async public void AddUsers(User user)
        {
            List<User> temp;
            try
            {
                temp = await Task.Run(() => GetUsers());
            }
            catch
            {
                temp = new List<User>();
            }
            temp.Add(user);

            using (FileStream createStream = File.OpenWrite("Users.json"))
            {
                await JsonSerializer.SerializeAsync(createStream, temp, new JsonSerializerOptions() { WriteIndented = true });
            }
            
        }

        static async public Task<List<List<Polozka>>> Delete(int i)
        {
            JSONAPI json = new JSONAPI();

            List<List<Polozka>> temp = await json.GetUctenky();
            temp.RemoveAt(i);


            using FileStream createStream = File.Create("Uctenky.json");
            await JsonSerializer.SerializeAsync(createStream, temp, new JsonSerializerOptions() { WriteIndented = true });

            createStream.Close();

            return temp;
        }
    }
}
